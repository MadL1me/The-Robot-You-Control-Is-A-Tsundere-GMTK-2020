using GMTK2020;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class WeaponBearer : MonoBehaviour
{
    public WeaponConfig[] InitialArsenal;

    public Weapon[] Arsenal;
    public int ActiveWeapon;
    public Weapon CurrentWeapon => Arsenal.Length != 0 ? Arsenal[ActiveWeapon] : null;

    public bool IsReloading { get; private set; }
    public ProjectileSide Side => _actor.Side;

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

    public bool CanShoot() =>
        CurrentWeapon?.CanShoot() == true && !IsReloading;

    public bool CanReload() =>
        CurrentWeapon?.CurrentRounds != CurrentWeapon.WeaponType.MagazineRounds && !IsReloading;

    public bool RequiresReload() =>
        CurrentWeapon?.CurrentRounds == 0 && !IsReloading;

    public float GetReloadProgress() =>
        (Time.time - _reloadStart) / CurrentWeapon.WeaponType.ReloadDuration;

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
