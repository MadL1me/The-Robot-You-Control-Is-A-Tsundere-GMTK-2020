using Pathfinding;
using UnityEngine;

namespace GMTK2020
{
    public abstract class Enemy : Actor, IDamageDealer
    {
        public int GetDamage => _damageFromTouch;

        protected AstarPath _path;
        protected Seeker _seeker;
        
        [SerializeField] protected int _damageFromTouch;

        protected override void Awake()
        {
            _seeker = GetComponent<Seeker>(); 
            base.Awake();
        }

        public abstract void Attack();
        protected abstract void Move();
        public abstract void MakeAIDecision();
        
        public override void Die() { Destroy(gameObject); }
    }
}