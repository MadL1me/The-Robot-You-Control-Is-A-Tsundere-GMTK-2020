using UnityEngine;

namespace GMTK2020
{
    public class FollowerEnemy : Enemy
    {
        private Transform _toFollow;
        
        public override void Attack() {}

        protected override void Move()
        {
            
        }

        public override void MakeAIDecision() => Move();
    }
}