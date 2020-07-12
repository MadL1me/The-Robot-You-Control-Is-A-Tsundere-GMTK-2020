using GMTK2020;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GMTK2020.HUD;
using UnityEngine;


[RequireComponent(typeof(Actor), typeof(AudioSource))]
public class WeaponBearer : MonoBehaviour
{
    public const float SINGLE_TAP_ANIMATION_DURATION = 0.15F;

    public Weapon CurrentWeapon => Arsenal.Length != 0 ? Arsenal[ActiveWeapon] : null;
    public ProjectileSide Side => _actor.Side;
    public bool IsReloading { get; private set; }
    
    public WeaponConfig[] InitialArsenal;
    public Weapon[] Arsenal;
    public int ActiveWeapon;
    
    private float _lastShoot;
    private float _reloadStart;
    private Actor _actor;
    [SerializeField] private AudioSource _audioSource;

    public void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        Arsenal = InitialArsenal.Select(x => new Weapon(x)).ToArray();
    }

    public void Start()
    {
        _actor = GetComponent<Actor>();
    }

    public AudioSource GetImpactSoundSource() =>
        _audioSource;

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
        if (weaponId < Arsenal.Length && weaponId >= 0 && !IsReloading)
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
    }

    public bool Shoot(float direction)
    {
        if (!CanShoot())
            return false;

        _lastShoot = Time.time;
        _audioSource.clip = CurrentWeapon.WeaponConfig.GetRandomShotClip();
        _audioSource.Play();
        return CurrentWeapon.Shoot(this, direction);
    }

    public void Reload()
    {
        if (!CanReload())
        {
            Debug.Log("Cant reload!");
            return;
        }
        
        IsReloading = true;
        _reloadStart = Time.time;
    }
}