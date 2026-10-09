using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRestartInput : MonoBehaviour
{
    public void OnRestart(InputAction.CallbackContext context)
    {
        GameManager.Instance?.OnRestart(context);
    }
}
