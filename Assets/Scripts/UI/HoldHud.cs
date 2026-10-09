using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoldHud : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;
    [SerializeField] private Text ammoText;

    private bool visible;

    private void Awake()
    {
        SetVisible(false);
    }

    private void Update()
    {
        if (playerHealth != null)
        {
            healthSlider.maxValue = playerHealth.MaxHealth;
            healthSlider.value = playerHealth.CurrentHealth;
            healthText.text = $"PV {playerHealth.CurrentHealth}/{playerHealth.MaxHealth}";
        }

        if (playerWeapon != null)
        {
            string suffix = playerWeapon.IsReloading ? " RECH." : string.Empty;
            ammoText.text = $"MUNITIONS {playerWeapon.AmmoInMagazine}/{playerWeapon.MagazineSize}{suffix}";
        }
    }

    public void OnShowHud(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SetVisible(true);
        }
        else if (context.canceled)
        {
            SetVisible(false);
        }
    }

    private void SetVisible(bool value)
    {
        visible = value;
        if (group == null)
        {
            return;
        }

        group.alpha = visible ? 1f : 0f;
        group.interactable = visible;
        group.blocksRaycasts = visible;
    }
}
