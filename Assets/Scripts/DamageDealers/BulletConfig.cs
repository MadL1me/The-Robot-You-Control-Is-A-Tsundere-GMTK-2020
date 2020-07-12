using UnityEngine;

namespace GMTK2020
{
    [CreateAssetMenu(fileName = "BulletConfig", menuName = "ScriptableObject/BulletConfig", order = 0)]
    public class BulletConfig : ScriptableObject
    {
        public float BulletSpeed;
        public int BulletDamage = 1;
        public Sprite Sprite;
        public Bullet BulletPrefab;
        public AudioClip[] ImpactSounds;
    }
}