using System;
using UnityEngine;


// 인공지능: 사랑처럼 똑똑하게 행동하는 알고리즘
// - 반응형/계획형 -> 규칙 기반 인공지능 (전통적인 방식)
//               ->   ㄴ 제어문(조건문, 반복문)

public class Enemy : MonoBehaviour
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
    private Vector3 _startPosition;                   // 시작 위치
    
    public float FindDistance     = 5f;     // 플레이어 발견 범위
    public float ReturnDistance   = 5f;     // 적 복귀 범위
    public float AttackDistance   = 2.5f;   // 플레이어 공격 범위
    public float MoveSpeed        = 3.3f;   // 이동 속도
    public float AttackCooltime   = 2f;     // 공격 쿨타임
    private float _attackTimer    = 0f;     // ㄴ 체크기
    public int Health             = 100;
    public float DamagedTime      = 0.5f;   // 경직 시간
    private float _damagedTimer   = 0f;     // ㄴ 체크기
    
    private void Start()
    {
        _startPosition = transform.position;
        _characterController = GetComponent<CharacterController>();
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

            case EnemyState.Damaged:
            {
                Damaged();
                break;
            }

            case EnemyState.Die:
            {
                Die();
                break;
            }
        }
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;
        
        Debug.Log($"상태전환: {CurrentState} -> Damaged");

        _damagedTimer = 0f;
        CurrentState = EnemyState.Damaged;
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
            CurrentState = EnemyState.Attack;
            return;
        }
        
        // 행동: 플레이어를 추적한다.
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Return()
    {
        // 전이: 시작 위치와 가까워 지면 -> Idle
        if(Vector3.Distance(transform.position, _startPosition) <= _characterController.minMoveDistance)
        {
            Debug.Log("상태전환: Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }
        
        // 전이: 플레이어와 가까워 지면 -> Trace
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Return -> Trace");
            CurrentState = EnemyState.Trace;
        }
    
        
        // 행동: 시작 위치로 되돌아간다.
        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        // 전이: 공격 범위 보다 멀어지면 -> Trace
        if(Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환: Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 0f;
            return;
        }
        
        // 행동: 플레이어를 공격한다.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            Debug.Log("플레이어 공격!");
            _attackTimer = 0f;
        }
    }

    private void Damaged()
    {
        // 행동: 일정 시간동안 멈춰있다가 -> Trace
        _damagedTimer += Time.deltaTime;
        if (_damagedTimer >= DamagedTime)
        {
            _damagedTimer = 0f;
            Debug.Log("상태전환: Damaged -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    private void Die()
    {
        // 행동 죽는다.
    }
    
}
