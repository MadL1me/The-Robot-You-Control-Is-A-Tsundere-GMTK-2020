using GMTK2020;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GMTK2020.HUD;
using UnityEngine;


[RequireComponent(typeof(Actor))]
public class WeaponBearer : MonoBehaviour
{
    public const float SINGLE_TAP_ANIMATION_DURATION = 0.15F;

    public WeaponConfig[] InitialArsenal;

    [SerializeField] private WeaponBearerView _view;
    
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
        CurrentWeapon?.CurrentRounds != CurrentWeapon.WeaponConfig.MagazineRounds && !IsReloading;

    public bool RequiresReload() =>
        CurrentWeapon?.CurrentRounds == 0 && !IsReloading;

    public float GetReloadProgress() =>
        (Time.time - _reloadStart) / CurrentWeapon.WeaponConfig.ReloadDuration;

    public bool TrySetWeapon(int weaponId)
    {
        if (weaponId < Arsenal.Length && weaponId >= 0)
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
            if (Time.time - _reloadStart > CurrentWeapon?.WeaponConfig.ReloadDuration)
            {
                IsReloading = false;
                CurrentWeapon.Refill();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            TrySetWeapon(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            TrySetWeapon(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            TrySetWeapon(2);
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0f ) // forward
            TrySetWeapon(ActiveWeapon+1);
        
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f ) // backwards
            TrySetWeapon(ActiveWeapon-1);;
        
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
