using UnityEngine;

namespace GMTK2020
{
    public abstract class Enemy : Actor, IDamageDealer
    {
        [SerializeField] protected int _damageFromTouch;
        
        public int GetDamage => _damageFromTouch;

        public abstract void Attack();
        protected abstract void Move();
        public abstract void MakeAIDecision();
        
        public override void Die() { Destroy(gameObject); }
    }
}