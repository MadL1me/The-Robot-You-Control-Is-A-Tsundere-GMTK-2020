using GMTK2020;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[RequireComponent(typeof(Actor))]
public class WeaponBearer : MonoBehaviour
{
    public const float SINGLE_TAP_ANIMATION_DURATION = 0.15F;

    public WeaponConfig[] InitialArsenal;

    public Weapon[] Arsenal;
    public int ActiveWeapon;
    public Weapon CurrentWeapon => Arsenal.Length != 0 ? Arsenal[ActiveWeapon] : null;

    public bool IsReloading { get; private set; }
    public ProjectileSide Side => _actor.Side;

    private float _lastShoot;
    private float _reloadStart;
    private Actor _actor;

    public void Awake()
    {
        Arsenal = InitialArsenal.Select(x => new Weapon(x)).ToArray();
    }

    public void Start()
    {
        _actor = GetComponent<Actor>();
    }

    public bool IsShotInProgress() =>
        CurrentWeapon != null && Time.time - _lastShoot <= SINGLE_TAP_ANIMATION_DURATION;

    public bool CanShoot() =>
        CurrentWeapon?.CanShoot() == true && !IsReloading;

    public bool CanReload() =>
        CurrentWeapon?.CurrentRounds != CurrentWeapon.WeaponType.MagazineRounds && !IsReloading;

    public bool RequiresReload() =>
        CurrentWeapon?.CurrentRounds == 0 && !IsReloading;

    public float GetReloadProgress() =>
        (Time.time - _reloadStart) / CurrentWeapon.WeaponType.ReloadDuration;

    public bool TrySetWeapon(int weaponId)
    {
        if (weaponId < Arsenal.Length)
        {
            ActiveWeapon = weaponId;
            return true;
        }

        return false;
    }

    private void Update()
    {
        if (IsReloading)
        {
            if (Time.time - _reloadStart > CurrentWeapon?.WeaponType.ReloadDuration)
            {
                IsReloading = false;
                CurrentWeapon.Refill();
            }
        }
    }

    public bool Shoot(float direction)
    {
        if (!CanShoot())
            return false;

        _lastShoot = Time.time;

        return CurrentWeapon.Shoot(this, direction);
    }

    public void Reload()
    {
        if (!CanReload())
            return;

        IsReloading = true;
        _reloadStart = Time.time;
    }
}
