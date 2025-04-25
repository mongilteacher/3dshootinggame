using UnityEngine;

public class Drum : MonoBehaviour, IDamageable
{
    /**
     과제 2. 드럼통(Barrel) 구현
        총으로 쏘면 폭발한다. (폭발 이펙트, 체력이 있다.)
        폭발하면 주위 적&플레이어에게 데미지를 가한다.
        ㄴ Physics.OverlapSphere 이용
        폭발하면 드럼통은 어딘가로 날라가며 n초 후 사라진다.
     */

    public GameObject ExplosionEffectPrefab;
    public int Health = 20;
    public float ExplodePower = 1500f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;

        if (Health <= 0)
        {
            Explode();
            Destroy(gameObject, 3f);
        }
    }

    private void Explode()
    {
        if (ExplosionEffectPrefab != null)
        {
            GameObject explosionPrefab = Instantiate(ExplosionEffectPrefab);
            explosionPrefab.transform.position = transform.position;
        }
        
        _rigidbody.AddForce(Vector3.up * ExplodePower, ForceMode.Impulse);
        _rigidbody.AddTorque(Vector3.up, ForceMode.Impulse);
    }
    
}
