using GMTK2020;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class WeaponConfig : ScriptableObject
{
    public BulletConfig Ammo;
    public int MagazineRounds;
    public float ReloadDuration;
    public float ShootDelay;
    public bool IsAutomatic;
}
