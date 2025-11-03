using UnityEngine;
using System.Collections;


//TODO reload perbullet

public class PlayerWeapon : MonoBehaviour
{
    [Header("Refs")] public PlayerContext ctx;

    public GameObject bulletPrefab;
   
    public UIManager ui;
    public WeponStats currentWeapon;

    [Header("WeaponStats")] public bool isAuto = true;
    public Transform firePoint;
    public WeaponType gunType;
    public float damage = 0f;
    public float fireRate = 0f;
    public int magazine = 0;
    public int maxMagazine = 0;
    public float reloadTime = 0f;
    public float critRate = 0f;
    public float critMultiplier = 0f;
    public bool magazineRelode = false;
    public FiringMode firingModes;
    public float stability = 0f;

    private float baseSwaySpeed = 0f;
    private float baseMaxSwayAngle = 0f;
    private float baseReturnSpeed = 0f;
    float swaySpeed;     
    float maxSwayAngle;  
    float returnSpeed;
    
    float swayTimer = 0f;
    Quaternion firePointOriginalRot;


    [Header("Burst Settings")] 
    public int burstCount = 3;
    public float burstInterval = 0.08f;

    float nextFireTime;
    bool isReloading;
    bool isFiring;
    bool isBursting;

    FiringMode firingMode = FiringMode.Auto;

    void Start()
    {
        if (!ctx) ctx = GetComponent<PlayerContext>();
        
        gunType = currentWeapon.WeaponType;
        firingModes  = currentWeapon.firingModes;
        damage = currentWeapon.damage + ctx.baseDamage;
        fireRate = currentWeapon.fireRate;
        magazine = currentWeapon.magazine;
        maxMagazine = currentWeapon.maxMagazine;
        reloadTime = currentWeapon.reloadTime;
        critRate = currentWeapon.critRate + ctx.basecritRate;
        critMultiplier = currentWeapon.critMultiplier + ctx.basecritMultiplier;
        magazineRelode = currentWeapon.magazineRelode;
        stability = currentWeapon.stability;
        
        baseSwaySpeed = currentWeapon.baseSwaySpeed;
        baseMaxSwayAngle = currentWeapon.baseMaxSwayAngle;
        baseReturnSpeed = currentWeapon.baseReturnSpeed;
        
        float k = 0.1f;
        float stabilityFactor = 1f / (1f + stability * k);
        
        swaySpeed    = baseSwaySpeed    * stabilityFactor;             
        maxSwayAngle = baseMaxSwayAngle * stabilityFactor;             
        returnSpeed  = baseReturnSpeed  * (1f + stability * k);        
        
        if (!firePoint)
        {
            Debug.LogError("PlayerWeapon: firePoint is missing.");
        }
        else
        {
            firePointOriginalRot = firePoint.localRotation;
        }

        if (!ui) ui = FindObjectOfType<UIManager>();
        ui?.UpdateAmmoText(magazine, maxMagazine);
        
      
    }
    void OnDisable()
    {
        isFiring = false;
        isBursting = false;
        StopAllCoroutines();
    }

    public void SetFiring(bool value)
    {
        switch (firingMode)
        {
            case FiringMode.Semi:
                if (value) TryShoot();
                isFiring = false; 
                break;

            case FiringMode.Auto:
                isFiring = value;
                break;

            case FiringMode.Burst:
                if (value && !isBursting)
                    StartCoroutine(FireBurst());
                isFiring = false;
                break;
        }

    }

    void Update()
    {
        
        
        if (firingMode == FiringMode.Auto && isFiring)
        {
            if (Time.time >= nextFireTime)
                TryShoot();
        }

      
        if (firePoint)
        {
            if (isFiring && firingMode == FiringMode.Auto)
            {
                swayTimer += Time.deltaTime * swaySpeed;

                float angle = Mathf.Sin(swayTimer) * maxSwayAngle;
                firePoint.localRotation = firePointOriginalRot * Quaternion.Euler(0f, angle, 0f);
            }
            else
            {
                swayTimer = Mathf.MoveTowards(swayTimer, 0f, Time.deltaTime * swaySpeed);
                firePoint.localRotation = Quaternion.Lerp(
                    firePoint.localRotation,
                    firePointOriginalRot,
                    Time.deltaTime * returnSpeed
                );
            }
        }

    }

    public void TryShoot()
    {
        
        if (isReloading) { Debug.Log("TryShoot blocked: reloading"); return; }
        if (Time.time < nextFireTime) { /* ยังไม่ถึงเวลา */ return; }

        if (magazine <= 0) {
            Debug.Log("TryShoot blocked: out of ammo");
            if (magazineRelode) TryReload();
            return;
        }
        
        if (!bulletPrefab) { Debug.LogError("TryShoot blocked: bulletPrefab is null"); return; }
        if (!firePoint)    { Debug.LogError("TryShoot blocked: firePoint is null"); return; }

      
        nextFireTime = Time.time + fireRate;

        magazine--;
        ui?.UpdateAmmoText(magazine, maxMagazine);

        var go = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var b  = go.GetComponent<Bullet>();
        b.Initialize(
            shooter: transform,
            shooterPosition: firePoint.position,
            gunType: gunType,        
            baseDamage: damage,
            critRate: critRate,
            critMult: critMultiplier
        );
        b.SetDirection(firePoint.forward);
        
        
    }

    public void TryReload()
    {
        if (isReloading) return;
        if (magazine >= maxMagazine) return;
        StartCoroutine(ReloadRoutine());
    }

    System.Collections.IEnumerator ReloadRoutine()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        magazine = maxMagazine;
        ui?.UpdateAmmoText(magazine, maxMagazine);
        isReloading = false;
    }

    IEnumerator FireBurst()
    {
        if (isReloading) yield break;
        isBursting = true;

        int shots = burstCount;
        while (shots > 0)
        {
            
            if (Time.time < nextFireTime)
            {
                yield return null;
                continue;
            }

            TryShoot();
            shots--;
            
            if (shots > 0) yield return new WaitForSeconds(burstInterval);
        }

        isBursting = false;
    }
}