using GMTK2020;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class WeaponBearer : MonoBehaviour
{
    public Weapon CurrentWeapon;
    public int CurrentRounds;

    public bool IsReloading { get; private set; }

    private float _reloadStart;
    private Actor _actor;

    public void Start()
    {
        _actor = GetComponent<Actor>();
    }

    public void Shoot(float direction)
    {
        var pos = transform.position
            + new Vector3(Mathf.Cos(direction * Mathf.PI * 2), Mathf.Sin(direction * Mathf.PI * 2)) * 0.5F;

        var angle = Quaternion.Euler(0F, 0F, direction * 360F);

        var bullet = Instantiate(CurrentWeapon.Ammo.BulletPrefab, pos, angle);
        bullet.Side = _actor.Side;
    }
}
