using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 offset = new Vector2(3.8f, 0.45f);
    [SerializeField] private float smoothTime = 0.18f;
    [SerializeField] private Vector2 minPosition = new Vector2(-3.5f, -0.1f);
    [SerializeField] private Vector2 maxPosition = new Vector2(34f, 5.6f);

    private Vector3 velocity;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 wanted = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);
        wanted.x = Mathf.Clamp(wanted.x, minPosition.x, maxPosition.x);
        wanted.y = Mathf.Clamp(wanted.y, minPosition.y, maxPosition.y);
        transform.position = Vector3.SmoothDamp(transform.position, wanted, ref velocity, smoothTime);
    }
}
