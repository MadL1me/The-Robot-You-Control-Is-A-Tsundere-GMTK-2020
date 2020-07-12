using System;
using UnityEngine;

namespace GMTK2020
{
    [RequireComponent(typeof(WeaponBearer))]
    public class RobotShooter : Enemy
    {
        [SerializeField] protected WeaponBearer _weaponBearer;

        protected override void Awake()
        {
            base.Awake();
            _weaponBearer.Reload();
            //_weaponBearer.GetComponent<WeaponBearer>();
        }

        protected override void Attack()
        {
            var difference = _player.transform.position - transform.position;
            var angle = (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + 360) % 360;
            _weaponBearer.Shoot(angle);
        }

        protected override void Move()
        {
            base.Move();
        }

        protected override void MakeAIDecision()
        {
            if (IsCanShot())
                Attack();
            else 
                Move();
        }

     //   private void OnDrawGizmos()
     //   {
          //  var raycast = Physics2D.Raycast(transform.position,
              //  (_player.transform.position - transform.position).normalized, 100f, ~LayerMask.GetMask("Enemy"));
            //Gizmos.DrawLine(transform.position, raycast.point);
    //    }

        public bool IsCanShot()
        {
            if (_weaponBearer.RequiresReload())
            {
              //  Debug.Log("Reuire to reload!");
                _weaponBearer.Reload();
                return false;
            }

            if (_weaponBearer.IsReloading)
            {
              //  Debug.Log("IS reloading!");
                return false;
            }

            if (!_weaponBearer.CanShoot())
            {
              //  Debug.Log("CantShoot!!");
                return false; }

            if (!IsEnemySeesPlayer())
            {
              //  Debug.Log("enemy not sees player!");
                return false;
            }
            
          //  Debug.Log("YESSS IT CA SHOOT");
            return true;
        }

        protected override void PlayActorAnimations()
        {
            if (Time.time - _lastAnimChange < 0.15F)
                return;

            _lastAnimChange = Time.time;

            var animType = AnimType.Idle;
            var watchDir = WatchDirection.Down;
            var vector = _directionToCurrentWaypoint;
            var speed = 1F;
            
            if (Mathf.Abs(_rigidbody.velocity.x) >= 0.2F || Mathf.Abs(_rigidbody.velocity.y) >= 0.2f)
                animType = AnimType.Move;

            if (_weaponBearer.IsShotInProgress())
            {
                animType = AnimType.Shoot;
                speed = 1F / WeaponBearer.SINGLE_TAP_ANIMATION_DURATION;
            }

            var direction = (Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg + 360) % 360;

            if (direction >= 360F * 0.625F && direction < 360F * 0.875F)
                watchDir = WatchDirection.Down;
            else if (direction >= 360F * 0.125F && direction < 360F * 0.375F)
                watchDir = WatchDirection.Up;
            else if (direction >= 360F * 0.875F || direction < 360F * 0.125F)
                watchDir = WatchDirection.Right;
            else if (direction >= 360F * 0.375F && direction < 360F * 0.625F)
                watchDir = WatchDirection.Left;

            AnimationController.AnimateActor(watchDir, animType, speed);
        }
        
        private bool IsEnemySeesPlayer()
        {
            var raycast = Physics2D.Raycast(transform.position,
                (_player.transform.position - transform.position).normalized, 100f, ~LayerMask.GetMask("Enemy"));

            if (!raycast.collider)
                return false;
            
            return raycast.collider.gameObject.CompareTag("Player");
        }
    }
}