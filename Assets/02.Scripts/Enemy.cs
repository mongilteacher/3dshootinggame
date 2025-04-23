using System;
using UnityEngine;

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

    private GameObject _player;           // 플레이어
    public float FindDistance = 7f;       // 플레이어 발견 범위

    private void Start()
    {
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
    
    // 3. 상태 함수들을 구현한다.
    
    private void Idle()
    {
        // 행동: 가만히 있는다. 
        
        if(Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환: Idle -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    private void Trace()
    {
        // 행동: 플레이어를 추적한다.
    }

    private void Return()
    {
        // 행동: 처음 자리로 되돌아간다.
    }

    private void Attack()
    {
        // 행동: 플레이어를 공격한다.
    }

    private void Damaged()
    {
        // 행동: 공격을 당한다.
    }

    private void Die()
    {
        // 행동 죽는다.
    }
    
}
