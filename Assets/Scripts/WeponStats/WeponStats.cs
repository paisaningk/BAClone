using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WeponStats", menuName = "Scriptable Objects/WeponStats")]
public class WeponStats : ScriptableObject

{
    [Header("Base Stats")]
   
    
    public WeaponType WeaponType;
    public FiringMode firingModes;
    public float damage = 0f;
    public float fireRate = 0f;
    public int magazine = 0;
    public int maxMagazine = 0;
    public float reloadTime = 0f;
    public float critRate = 0f;            
    public float critMultiplier = 0f;
    public bool magazineRelode = false;
    public float stability = 0f;

    [Header("RecoilSettings")]
    
    public float baseSwaySpeed = 0f;
    public float baseMaxSwayAngle = 0f;
    public float baseReturnSpeed = 0f;


}
