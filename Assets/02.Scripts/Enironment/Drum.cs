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
    public int Damage = 100;
    public float ExplodeRange = 5f;
    public float ExplodePower = 1500f;

    private Rigidbody _rigidbody;
    
    private bool _isExploded = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;

        if (Health <= 0 && !_isExploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        _isExploded = true;
        
        if (ExplosionEffectPrefab != null)
        {
            GameObject explosionPrefab = Instantiate(ExplosionEffectPrefab);
            explosionPrefab.transform.position = transform.position;
        }
        
        _rigidbody.AddForce(Vector3.up * ExplodePower, ForceMode.Impulse);
        _rigidbody.AddTorque(Vector3.up, ForceMode.Impulse);
        
        // 드럼통을 감지 안하고 싶고
        // 유니티는 레이어를 넘버링하는게 아니라 비트로 관리
        // 2진수 ->   0000 0000
        //       1 : 0000 0001
        //       2 : 0000 0010
        //       3 : 0000 0011
        //      17 : 0001 0001 
        //     255 : 1111 1111
        //  비트 단위로 on/off를 관리할 수 있다.
        // int 0000 0000 0000 0000 ... 000 (32비트)
        // bool 8비트 -> true/false
        
        // 비트 연산&마스클 아는 사람만 이해할 수 있다.
        // ㄴ 자습시간에 따로 공부를해야하고
        // ㄴ 중요한게 아니다.
        
         //Collider[] colls = Physics.OverlapSphere(transform.position, ExplodeRange, ~(1 << 9));
         //                                                                      1111  1110 1111 1111
         Collider[] colls = Physics.OverlapSphere(transform.position, ExplodeRange, ~LayerMask.NameToLayer("Drum"));

         foreach (Collider coll in colls)
         {
             if (coll.TryGetComponent(out IDamageable damageable))
             {
                 Damage damage = new Damage();
                 damage.Value = Damage;
                 
                 damageable.TakeDamage(damage);
             }
         }
 
         
         // 드럼통만 감지하고 싶어요.
         // 만약에 주위에 드럼통이 있다면 드럼통도 폭발
         //Collider[] drums = Physics.OverlapSphere(transform.position, ExplodeRange, 1 << 9);
         Collider[] drums = Physics.OverlapSphere(transform.position, ExplodeRange, LayerMask.NameToLayer("Drum"));
         //                                                                      0000  0001 0000 0000
         foreach (Collider drumCol in drums)
         {
             if (drumCol.TryGetComponent(out Drum drum))
             {
                 drum.Explode();
             }
         }
         
         Destroy(gameObject, 3f);
    }
    
}
