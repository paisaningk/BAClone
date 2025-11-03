using UnityEngine;

[CreateAssetMenu(menuName = "Character Stats")]
public class CharacterStats : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHP = 100;
    public float maxStamina = 80;
    public float Damage = 10;
    public float critMultiplier = 5;
    public float armor = 1;
    public float critRate = 1;

    // [Header("Level Scaling")]
    // public AnimationCurve HPPerLevel;
    // public AnimationCurve StaminaPerLevel;
    // public AnimationCurve AttackPerLevel;
}