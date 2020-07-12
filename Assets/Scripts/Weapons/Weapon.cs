using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon
{
    public WeaponConfig WeaponConfig { get; private set; }
    public int CurrentRounds { get; private set; }

    private float _lastShot;

    public Weapon(WeaponConfig config)
    {
        WeaponConfig = config;

        Refill();
    }

    public void Refill() =>
        CurrentRounds = WeaponConfig.MagazineRounds;

    public void DecreaseAmmo()
    {
        if (CurrentRounds > 0)
            CurrentRounds--;
    }

    public bool CanShoot() =>
        CurrentRounds > 0 && Time.time - _lastShot > WeaponConfig.ShootDelay;

    public bool Shoot(WeaponBearer bearer, float direction)
    {
        if (!CanShoot())
            return false;

        for (int i = 0; i < WeaponConfig.RoundsPerShot; i++)
        {
            var pos = bearer.transform.position
                + new Vector3(Mathf.Cos(Mathf.Deg2Rad * direction), Mathf.Sin(Mathf.Deg2Rad * direction)) * 0.5F;

            var directionWithSpread = direction + UnityEngine.Random.Range(-WeaponConfig.Spread, WeaponConfig.Spread);

            var angle = Quaternion.Euler(0F, 0F, directionWithSpread);

            var bullet = UnityEngine.Object.Instantiate(WeaponConfig.Ammo.BulletPrefab, pos, angle);
            Debug.Log("Shoot!");
            bullet.Config = WeaponConfig.Ammo;
            bullet.Angle = directionWithSpread;
            bullet.Side = bearer.Side;
            bullet.SoundSource = bearer.GetImpactSoundSource();
            bullet.UpdateSprite();
        }

        DecreaseAmmo();
        _lastShot = Time.time;

        return true;
    }
}