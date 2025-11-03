using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable, IHasArmor
{
    PlayerContext ctx;

    public float max;
    public float current;
    public float armor;
    bool invincible;
    float IHasArmor.Armor => armor;

    public bool IsAlive => current > 0f;

    void Awake()
    {
        armor = ctx.basearmor;
        max = ctx.basemaxHealth;
        current = max;
    }

    public void SetInvincible(bool v) => invincible = v;

    public void TakeDamage(float dmg)
    {
        if (!IsAlive || invincible) return;
        current = Mathf.Max(0f, current - Mathf.Max(0f, dmg));
        Debug.Log($"[PLAYER] HP {current}/{max}");
        if (!IsAlive) Die();
    }

    void Die()
    {
        
        Debug.Log($"{name} has been defeated!");
    }
}
