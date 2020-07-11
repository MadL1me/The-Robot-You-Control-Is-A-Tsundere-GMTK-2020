using GMTK2020;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu]
public class WeaponConfig : ScriptableObject
{
    public BulletConfig Ammo;
    public int MagazineRounds;
    public float ReloadDuration;
    public float ShootDelay;
    public bool IsAutomatic;
    public int RoundsPerShot;
    public float Spread;
    public Sprite WeaponSprite;
    public float ScreenShakeAmount;
    public string WeaponName;
    public AudioClip[] BulletSoundClips;

    public AudioClip GetRandomShotClip() => BulletSoundClips[Random.Range(0,BulletSoundClips.Length)];
}
