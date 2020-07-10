using GMTK2020;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu]
public class Weapon : ScriptableObject
{
    public Bullet Ammo;
    public int MagazineRounds;
    public float ReloadSpeed;
}
