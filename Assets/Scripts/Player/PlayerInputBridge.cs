using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputBridge : MonoBehaviour
{
    [SerializeField] private InputActionAsset actions;
    [SerializeField] private PlayerController controller;
    [SerializeField] private PlayerWeapon weapon;
    [SerializeField] private GrapplingHook hook;
    [SerializeField] private HoldHud hud;
    [SerializeField] private PlayerRestartInput restartInput;

    private InputActionMap playerMap;
    private InputAction move;
    private InputAction aim;
    private InputAction jump;
    private InputAction fire;
    private InputAction grapple;
    private InputAction reload;
    private InputAction showHud;
    private InputAction restart;

    private void Awake()
    {
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }

        if (weapon == null)
        {
            weapon = GetComponent<PlayerWeapon>();
        }

        if (hook == null)
        {
            hook = GetComponent<GrapplingHook>();
        }

        if (hud == null)
        {
            hud = FindAnyObjectByType<HoldHud>();
        }

        if (restartInput == null)
        {
            restartInput = GetComponent<PlayerRestartInput>();
        }

        playerMap = actions.FindActionMap("Player", true);
        move = playerMap.FindAction("Move", true);
        aim = playerMap.FindAction("Aim", true);
        jump = playerMap.FindAction("Jump", true);
        fire = playerMap.FindAction("Fire", true);
        grapple = playerMap.FindAction("Grapple", true);
        reload = playerMap.FindAction("Reload", true);
        showHud = playerMap.FindAction("ShowHud", true);
        restart = playerMap.FindAction("Restart", true);
    }

    private void OnEnable()
    {
        if (playerMap == null)
        {
            return;
        }

        move.performed += controller.OnMove;
        move.canceled += controller.OnMove;
        aim.performed += controller.OnAim;
        aim.canceled += controller.OnAim;
        jump.performed += controller.OnJump;
        fire.performed += weapon.OnFire;
        grapple.performed += hook.OnGrapple;
        reload.performed += weapon.OnReload;
        showHud.performed += hud.OnShowHud;
        showHud.canceled += hud.OnShowHud;
        restart.performed += restartInput.OnRestart;
        playerMap.Enable();
    }

    private void OnDisable()
    {
        if (playerMap == null)
        {
            return;
        }

        move.performed -= controller.OnMove;
        move.canceled -= controller.OnMove;
        aim.performed -= controller.OnAim;
        aim.canceled -= controller.OnAim;
        jump.performed -= controller.OnJump;
        fire.performed -= weapon.OnFire;
        grapple.performed -= hook.OnGrapple;
        reload.performed -= weapon.OnReload;
        showHud.performed -= hud.OnShowHud;
        showHud.canceled -= hud.OnShowHud;
        restart.performed -= restartInput.OnRestart;
        playerMap.Disable();
    }
}
