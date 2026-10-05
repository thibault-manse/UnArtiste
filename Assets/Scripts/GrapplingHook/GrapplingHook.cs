using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingHook : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float maxDistance = 8f;
    [SerializeField] private float pullSpeed = 13f;
    [SerializeField] private float stopDistance = 0.8f;
    [SerializeField] private AudioSource grappleAudio;

    private LineRenderer line;
    private Coroutine activeRoutine;
    private Transform target;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;

        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }

        if (playerBody == null)
        {
            playerBody = GetComponent<Rigidbody2D>();
        }
    }

    private void LateUpdate()
    {
        if (target != null && firePoint != null)
        {
            line.SetPosition(0, firePoint.position);
            line.SetPosition(1, target.position);
        }
    }

    public void OnGrapple(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryGrapple();
        }
    }

    public void TryGrapple()
    {
        Vector2 origin = firePoint != null ? firePoint.position : transform.position;
        Vector2 aim = controller != null ? controller.AimDirection : Vector2.right;
        GrapplePoint[] points = FindObjectsByType<GrapplePoint>();
        Transform bestTarget = null;
        float bestScore = 0.72f;

        foreach (GrapplePoint point in points)
        {
            Vector2 toPoint = (Vector2)point.transform.position - origin;
            float distance = toPoint.magnitude;
            if (distance > maxDistance || distance < 0.2f)
            {
                continue;
            }

            float score = Vector2.Dot(aim.normalized, toPoint.normalized);
            if (score > bestScore)
            {
                bestScore = score;
                bestTarget = point.transform;
            }
        }

        if (bestTarget == null)
        {
            return;
        }

        if (activeRoutine != null)
        {
            StopCoroutine(activeRoutine);
        }

        target = bestTarget;
        line.positionCount = 2;
        grappleAudio?.Play();
        activeRoutine = StartCoroutine(PullRoutine());
    }

    private IEnumerator PullRoutine()
    {
        while (target != null && Vector2.Distance(playerBody.position, target.position) > stopDistance)
        {
            Vector2 direction = ((Vector2)target.position - playerBody.position).normalized;
            playerBody.linearVelocity = direction * pullSpeed;
            yield return new WaitForFixedUpdate();
        }

        target = null;
        line.positionCount = 0;
        activeRoutine = null;
    }
}
