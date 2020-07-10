using UnityEngine;

namespace GMTK2020
{
    public interface IDamageDealer
    { 
        int GetDamage { get; }
    }
    
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour, IDamageDealer
    {
        [SerializeField] private BulletConfig _bulletConfig;
        
        public int GetDamage => _bulletConfig.BulletDamage;
    }
}