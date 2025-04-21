using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        // interpoling, smoothing 기법이 들어갈 예쩡
        transform.position = Target.position;
    }
}
