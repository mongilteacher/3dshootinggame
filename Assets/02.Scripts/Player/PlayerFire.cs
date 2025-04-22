using System;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    
    // 필요 속성
    // - 발사 위치
    public GameObject FirePosition;
    // - 폭탄 프리팹
    public GameObject BombPrefab;
    // - 던지는 힘
    public float ThrowPower = 15f;
    
    
    // 목표: 마우스의 왼쪽 버튼을 누르면 카메라가 바라보는 방향으로 총을 발사하고 싶다.
    public ParticleSystem BulletEffect;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }
    
    private void Update()
    {
        // 2. 오른쪽 버튼 입력 받기
        // - 0: 왼쪽, 1: 오른쪽, 2: 휠
        if (Input.GetMouseButtonDown(1))
        {
            // 3. 발사 위치에 수류탄 생성하기
            GameObject bomb = Instantiate(BombPrefab);
            bomb.transform.position = FirePosition.transform.position;
            
            // 4. 생성된 수류탄을 카메라 방향으로 물릭적인 힘 가하기
            Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
            bombRigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
            bombRigidbody.AddTorque(Vector3.one);
        }
        
        // 총알 발사(레이저 방식)
        // 1. 왼쪽 버튼 입력 받기
        if (Input.GetMouseButtonDown(0))
        {
            // 2. 레이를 생성하고 발사 위치와 진행 방향을 설정
            Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
            
            // 3. 레이와 부딛힌 물체의 정보를 저장할 변수를 생성
            RaycastHit hitInfo = new RaycastHit();

            // 4. 레이를 발사한 다음,            
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit) //데이터가 있다면(부딛혔다면
            {
                // 피격 이펙트 생성(표시)
                BulletEffect.transform.position = hitInfo.point;
                BulletEffect.transform.forward  = hitInfo.normal; // 법선 벡터: 직선에 대하여 수직인 벡터
                BulletEffect.Play();
                
                // 게임 수학: 선형대수학(스칼라, 벡터, 행렬) 기하학(삼각함수 ..)
            }
        }
        // Ray: 레이저(시작위치, 방향)
        // RayCast: 레이저를 발사
        // RayCastHit: 레이저가 물체와 부딛혔다면 그 정보를 저장하는 구조체
        
    }
}
