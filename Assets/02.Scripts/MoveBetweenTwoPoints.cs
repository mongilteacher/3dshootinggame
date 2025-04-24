using UnityEngine;

public class MoveBetweenTwoPoints : MonoBehaviour
{
    public Transform pointA; // 시작 위치
    public Transform pointB; // 끝 위치
    public float speed = 1.0f; // 이동 속도

    void Update()
    {
        // 시간에 따라 PingPong 값을 계산 (0~1 사이)
        float time = Mathf.PingPong(Time.time * speed, 1f);

        // A와 B 사이를 Lerp로 보간
        transform.position = Vector3.Lerp(pointA.position, pointB.position, time);
    }
}