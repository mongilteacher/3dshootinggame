using UnityEngine;

public class EnemyAttackEvent : MonoBehaviour
{
    public Enemy MyEnemy;

    public void AttackEvent()
    {
        //MyEnemy.Attack();
        Debug.Log("플레이어 공격!");
    }
}
