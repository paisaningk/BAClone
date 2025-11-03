using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    [Header("Core")]
    public Rigidbody rb;
    public CharacterController cc;


    [Header("Character_BaseStatus")]
    
    public CharacterStats baseStats;
    
    public float baseDamage => baseStats.Damage;
    public float basearmor => baseStats.armor;
    public float basemaxHealth => baseStats.maxHP;
    public float basecritRate => baseStats.critRate;
    public float basecritMultiplier => baseStats.critMultiplier;
    public float baseStamina => baseStats.maxStamina;
    
    
    [Header("Weapons")]
    public PlayerWeapon weapon;
    
    [Header("Modules")]
    public PlayerHealth health;
    public PlayerStamina stamina;
    public PlayerDash dash;
    public PlayerMovementCC movement;
    
    [Header("Aim Target Reference")]
    public Transform aimTarget;

    [Header("Input Values")]
    public Vector2 moveInput;
    public Vector2 lookInput;

    public bool isPC = true;
    void Awake()
    {
        if (!cc) cc = GetComponent<CharacterController>();
        if (!health)  health  = GetComponent<PlayerHealth>();
        if (!stamina) stamina = GetComponent<PlayerStamina>();
        if (!dash)    dash    = GetComponent<PlayerDash>();
        if (!movement)movement= GetComponent<PlayerMovementCC>();
        if (!weapon)  weapon  = GetComponent<PlayerWeapon>();
    }
}
