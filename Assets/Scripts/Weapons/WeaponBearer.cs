using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBearer : MonoBehaviour
{
    public Weapon CurrentWeapon;
    public int CurrentRounds;

    public bool IsReloading { get; private set; }

    private float _reloadStart;

    public void Update()
    {
        
    }
}
