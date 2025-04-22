using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 목표: 마우스의 오른쪽 버튼을 누르면 카메라가 바라보는 방향으로 수류탄을 던지고 싶다.
    // 1. 수류탄 오브젝트 만들기
    // 2. 오른쪽 버튼 입력 받기
    // 3. 발사 위치에 수류탄 생성하기
    // 4. 생성된 수류탄을 카메라 방향으로 물릭적인 힘 가하기

    public GameObject ExplosionEffectPrefab;
    
    // 충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;
        
        Destroy(gameObject);
    }
}
