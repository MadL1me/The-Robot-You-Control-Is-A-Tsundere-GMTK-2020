using UnityEngine;

namespace GMTK2020
{
    [RequireComponent(typeof(WeaponBearer))]
    public class RobotShooter : Enemy
    {
        protected WeaponBearer _weaponBearer;
        [SerializeField] protected float _shootTimeout;
        
        protected override void Attack()
        {
            var difference = _player.transform.position - transform.position;
            var angle = Mathf.Atan2(difference.x, difference.y);
            _weaponBearer.Shoot(angle);
        }

        protected override void Move()
        {
        }

        protected override void MakeAIDecision()
        {
            
            
        }

        public bool IsCanShot()
        {
            return false;
        }
    }
}