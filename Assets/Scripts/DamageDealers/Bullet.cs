using UnityEngine;

namespace GMTK2020
{
    public interface IDamageDealer
    { 
        int GetDamage { get; }
    }

    public enum ProjectileSide
    {
        Friend,
        Opponent
    }
    
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour, IDamageDealer
    {
        public int GetDamage => _damage;
        [SerializeField] protected int _damage;

        public float Speed;
        public ProjectileSide Side;

        private void Update()
        {
            
        }
    }
}