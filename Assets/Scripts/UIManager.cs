using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private PlayerContext ctx;   

    [Header("Texts")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI staminaText;

    void Awake()
    {
        if (!ctx) ctx = FindAnyObjectByType<PlayerContext>(); 
        if (!ammoText) Debug.LogWarning("[UI] ammoText not assigned");
        if (!staminaText) Debug.LogWarning("[UI] staminaText not assigned");
    }

    void Start()
    {
       
        if (ctx && ctx.stamina != null)
        {
            ctx.stamina.OnStaminaChanged += UpdateStamina;
            
            UpdateStamina(ctx.stamina.current, ctx.stamina.max);
        }

       
    }

    public void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        if (!ammoText) return;
        ammoText.text = $"{currentAmmo}/{maxAmmo}";
        ammoText.color = (currentAmmo == 0) ? Color.red : Color.white;
    }

    public void UpdateStamina(float currentStamina, float maxStamina)
    {
        if (!staminaText) return;
        staminaText.text = $"{currentStamina:0}/{maxStamina:0}";
   
    }
}