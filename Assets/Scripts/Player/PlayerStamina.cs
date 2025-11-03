using System;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    PlayerContext ctx;
    public float max = 100f;
    public float current = 100f;
    public float regenRate = 15f;
    public float regenDelay = 1f;
    
    public float bonusFlat = 0f;   // +20
    public float bonusMult = 1f;   // x1.2
    

    float unblockAtTime;
    public System.Action<float, float> OnStaminaChanged; // (current,max)
    
    public bool Spend(float cost)
    {
        if (current < cost) return false;
        current -= cost;
        unblockAtTime = Time.time + regenDelay;
        OnStaminaChanged?.Invoke(current, max);
        return true;
    }

    void Awake()
        {
            ctx = GetComponent<PlayerContext>();
            RecalculateMax();
            current = max;                    
            OnStaminaChanged?.Invoke(current, max);
        }
    public void RecalculateMax()
    {
        max = Mathf.Max(1f, (ctx.baseStamina + bonusFlat) * Mathf.Max(0.01f, bonusMult));
        current = Mathf.Min(current, max); // ถ้าลด max ให้ current ไม่เกิน
        OnStaminaChanged?.Invoke(current, max);
    }
    
   

    void Update()
        {
            if (Time.time < unblockAtTime) return;
            if (current >= max) return;
            current = Mathf.Min(max, current + regenRate * Time.deltaTime);
            OnStaminaChanged?.Invoke(current, max);
        }
}


  