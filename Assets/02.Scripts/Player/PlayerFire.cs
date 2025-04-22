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
        
        
        
    }
}
