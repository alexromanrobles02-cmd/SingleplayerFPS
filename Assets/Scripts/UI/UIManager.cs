using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI maxAmmoText;

    [Header("Health UI")]
    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetAmmo(int ammo)
    {
        ammoText.text = ammo.ToString();
    }

    public void SetMaxAmmo(int ammo)
    {
        maxAmmoText.text = ammo.ToString();
    }

    public void SetHealth(int currentHealth, int maxHealth)
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }
        
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }
    }
}
