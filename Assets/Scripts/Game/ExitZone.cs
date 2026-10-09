using UnityEngine;

public class ExitZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerHealth _))
        {
            GameManager.Instance?.CompletePrototype();
        }
    }
}
