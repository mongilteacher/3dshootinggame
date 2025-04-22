using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 목표: wasd를 누르면 캐릭터을 카메라 방향에 맞게 이동시키고 싶다.
    // 필요 속성:
    // - 이동속도
    public float MoveSpeed = 7f;
    public float JumpPower = 5f;
    
    private const float GRAVITY = -9.8f; // 중력
    private float _yVelocity = 0f;       // 중력가속도
    
    private bool _isJumping = false;
    
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    // 구현 순서:
    // 
    void Update()
    {
        // 1. 키보드 입력을 받는다.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. 입력으로부터 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized; 
        
        // 2-1. 메인 카메라를 기준으로 방향을 변환한다.
        dir = Camera.main.transform.TransformDirection(dir);

        
        // 캐릭터가 땅 위에 있다면..
        if (_characterController.isGrounded)
        // = if (_characterController.collisionFlags == CollisionFlags.Below)
        //
        {
            _isJumping = false;
        }
        
        // 3. 점프 적용  
        if (Input.GetButtonDown("Jump")  && _isJumping == false)
        {
            _yVelocity = JumpPower;
            
            _isJumping = true;
        }
        
        
        // 4. 중력 적용
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;
        
        
        // 4. 방향에 따라 플레이어를 이동한다.
        //transform.position += dir * MoveSpeed * Time.deltaTime;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }
}
