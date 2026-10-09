using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private Transform muzzle;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int magazineSize = 12;
    [SerializeField] private float fireCooldown = 0.16f;
    [SerializeField] private float reloadDuration = 1.1f;
    [SerializeField] private AudioSource shotAudio;
    [SerializeField] private AudioSource reloadAudio;

    private int ammoInMagazine;
    private float nextShotTime;
    private bool reloading;

    public int AmmoInMagazine => ammoInMagazine;
    public int MagazineSize => magazineSize;
    public bool IsReloading => reloading;

    private void Awake()
    {
        ammoInMagazine = magazineSize;
        if (controller == null)
        {
            controller = GetComponent<PlayerController>();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryFire();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryReload();
        }
    }

    public void TryFire()
    {
        if (reloading || Time.time < nextShotTime || ammoInMagazine <= 0 || projectilePrefab == null || muzzle == null)
        {
            return;
        }

        nextShotTime = Time.time + fireCooldown;
        ammoInMagazine--;

        Vector2 direction = controller != null ? controller.AimDirection : Vector2.right;
        Projectile projectile = Instantiate(projectilePrefab, muzzle.position, Quaternion.identity);
        projectile.Fire(direction);

        if (shotAudio != null)
        {
            shotAudio.pitch = Random.Range(0.9f, 1.08f);
            shotAudio.Play();
        }
    }

    public void TryReload()
    {
        if (!reloading && ammoInMagazine < magazineSize)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    private IEnumerator ReloadRoutine()
    {
        reloading = true;
        reloadAudio?.Play();
        yield return new WaitForSeconds(reloadDuration);
        ammoInMagazine = magazineSize;
        reloading = false;
    }
}
