using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;


// 인공지능: 사랑처럼 똑똑하게 행동하는 알고리즘
// - 반응형/계획형 -> 규칙 기반 인공지능 (전통적인 방식)
//               ->   ㄴ 제어문(조건문, 반복문)

public class Enemy : MonoBehaviour, IDamageable
{
    // 1. 상태를 열거형으로 정의한다.
    public enum EnemyState
    {
        Idle,
        Trace,
        Return,
        Attack,
        Damaged,
        Die
    }

    // 2. 현재 상태를 지정한다.
    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;                       // 플레이어
    private CharacterController _characterController; // 캐릭터 컨트롤러
    private NavMeshAgent _agent;                      // 네비메시 에이전트
    private Vector3 _startPosition;                   // 시작 위치
    private Animator _animator;
    
    public float FindDistance     = 5f;     // 플레이어 발견 범위
    public float ReturnDistance   = 5f;     // 적 복귀 범위
    public float AttackDistance   = 2.5f;   // 플레이어 공격 범위
    public float MoveSpeed        = 3.3f;   // 이동 속도
    public float AttackCooltime   = 2f;     // 공격 쿨타임
    private float _attackTimer    = 0f;     // ㄴ 체크기
    public int Health             = 100;
    public float DamagedTime      = 0.5f;   // 경직 시간
    public float DeathTime        = 1f;
    
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed; 
        
        _startPosition = transform.position;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    
    
    private void Update()
    {
        // 나의 현재 상태에 따라 상태 함수를 호출한다.
        switch (CurrentState)
        {
            case EnemyState.Idle:
            {
                Idle();
                break;
            }

            case EnemyState.Trace:
            {
                Trace();
                break;
            }

            case EnemyState.Return:
            {
                Return();
                break;
            }

            case EnemyState.Attack:
            {
                Attack();
                break;
            }
        }
    }

    public void TakeDamage(Damage damage)
    {
        // 사망했거나 공격받고 있는 중이면..
        if(CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }
        
        Health -= damage.Value;

        if (Health <= 0)
        {
            CurrentState = EnemyState.Die;
            Debug.Log($"상태전환: {CurrentState} -> Die");
            CurrentState = EnemyState.Die;
            _animator.SetTrigger("Die");
            StartCoroutine(Die_Coroutine());
            return;
        }
        
        
        Debug.Log($"상태전환: {CurrentState} -> Damaged");
        
        _animator.SetTrigger("Hit");
        CurrentState = EnemyState.Damaged;
        StartCoroutine(Damaged_Coroutine());
    }
    
    
    // 3. 상태 함수들을 구현한다.
    
    private void Idle()
    {
        // 행동: 가만히 있는다. 
        
        // 전이: 플레이어와 가까워 지면 -> Trace
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            CurrentState = EnemyState.Trace;
            _animator.SetTrigger("IdleToMove");
        }
    }

    private void Trace()
    {
        // 전이: 플레이어와 멀어지면 -> Return
        if(Vector3.Distance(transform.position, _player.transform.position) > ReturnDistance)
        {
            Debug.Log("상태전환: Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }
        
        // 전이: 공격 범위 만큼 가까워 지면 -> Attack
        if(Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("상태전환: Trace -> Attack");
            _animator.SetTrigger("MoveToAttackDelay");
            CurrentState = EnemyState.Attack;
            return;
        }
        
        // 행동: 플레이어를 추적한다.
        // Vector3 dir = (_player.transform.position - transform.position).normalized;
        // _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_player.transform.position);
    }

    private void Return()
    {
        // 전이: 시작 위치와 가까워 지면 -> Idle
        if(Vector3.Distance(transform.position, _startPosition) <= _characterController.minMoveDistance)
        {
            Debug.Log("상태전환: Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            _animator.SetTrigger("MoveToIdle");
            return;
        }
        
        // 전이: 플레이어와 가까워 지면 -> Trace
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Return -> Trace");
            CurrentState = EnemyState.Trace;
        }
    
        
        // 행동: 시작 위치로 되돌아간다.
        // Vector3 dir = (_startPosition - transform.position).normalized;
        // _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_startPosition);
    }

    private void Attack()
    {
        // 전이: 공격 범위 보다 멀어지면 -> Trace
        if(Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 0f;
            _animator.SetTrigger("AttackDelayToMove");
            return;
        }
        
        // 행동: 플레이어를 공격한다.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            _animator.SetTrigger("AttackDelayToAttack");
            
            _attackTimer = 0f;
        }
    }

    private IEnumerator Damaged_Coroutine()
    {
        // 행동: 일정 시간동안 멈춰있다가 -> Trace
        /*_damagedTimer += Time.deltaTime;
        if (_damagedTimer >= DamagedTime)
        {
            _damagedTimer = 0f;
            Debug.Log("상태전환: Damaged -> Trace");
            CurrentState = EnemyState.Trace;
        }*/
        
        // 코루틴 방식으로 변경
        _agent.isStopped = true;
        _agent.ResetPath();
        yield return new WaitForSeconds(DamagedTime);
        Debug.Log("상태전환: Damaged -> Trace");
        CurrentState = EnemyState.Trace;
    }

    private IEnumerator Die_Coroutine()
    {
        yield return new WaitForSeconds(DeathTime);
        gameObject.SetActive(false);
    }
}
