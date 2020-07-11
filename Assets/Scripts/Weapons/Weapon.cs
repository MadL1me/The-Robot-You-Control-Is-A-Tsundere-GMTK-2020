using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon
{
    public WeaponConfig WeaponType;
    public int CurrentRounds { get; private set; }

    private float _lastShot;

    public Weapon(WeaponConfig type)
    {
        WeaponType = type;

        Refill();
    }

    public void Refill() =>
        CurrentRounds = WeaponType.MagazineRounds;

    public void DecreaseAmmo()
    {
        if (CurrentRounds > 0)
            CurrentRounds--;
    }

    public bool CanShoot() =>
        CurrentRounds > 0 && Time.time - _lastShot > WeaponType.ShootDelay;

    public bool Shoot(WeaponBearer bearer, float direction)
    {
        if (!CanShoot())
            return false;

        for (int i = 0; i < WeaponType.RoundsPerShot; i++)
        {
            var pos = bearer.transform.position
                + new Vector3(Mathf.Cos(Mathf.Deg2Rad * direction), Mathf.Sin(Mathf.Deg2Rad * direction)) * 0.5F;

            var directionWithSpread = direction + UnityEngine.Random.Range(-WeaponType.Spread, WeaponType.Spread);

            var angle = Quaternion.Euler(0F, 0F, directionWithSpread);

            var bullet = UnityEngine.Object.Instantiate(WeaponType.Ammo.BulletPrefab, pos, angle);
            bullet.Angle = directionWithSpread;
            bullet.Side = bearer.Side;
        }

        DecreaseAmmo();
        _lastShot = Time.time;

        return true;
    }
}