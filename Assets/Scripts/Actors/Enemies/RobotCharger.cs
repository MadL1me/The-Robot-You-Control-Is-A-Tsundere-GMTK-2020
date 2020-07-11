using UnityEngine;

namespace GMTK2020
{
    public class RobotCharger : Enemy
    {
        protected override void Attack(){}

        protected override void Move()
        {
            Debug.Log("Move");
           // Debug.Log($"Direction is: {_directionToCurrentWaypoint}");
            _rigidbody.AddForce(_directionToCurrentWaypoint.normalized * _movingSpeed * Time.fixedDeltaTime);
        }

        protected override void MakeAIDecision()
        {
            Move();
        }
    }
}