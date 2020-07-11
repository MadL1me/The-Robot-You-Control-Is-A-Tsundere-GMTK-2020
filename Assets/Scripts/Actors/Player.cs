using System;
using System.Collections;
using System.Collections.Generic;
using GMTK2020;
using UnityEngine;

namespace GMTK2020
{
    public class Player : Actor
    {
        [SerializeField] private float _invisibleTime;

        private PlayerMovement _movement;
        private WeaponBearer _bearer;
        
        protected override void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _bearer = GetComponent<WeaponBearer>();
            base.Awake();
        }

        protected override void PlayActorAnimations()
        {
            var animType = AnimType.Idle;
            var watchDir = WatchDirection.Down;
            var vector = _movement.MoveVector;
            var speed = 1F;
            
            if (Math.Abs(vector.x) >= 0.2F || Math.Abs(vector.y) >= 0.2f)
                animType = AnimType.Move;

            if (_bearer.IsShotInProgress())
            {
                animType = AnimType.Shoot;
                speed = 1F / WeaponBearer.SINGLE_TAP_ANIMATION_DURATION;
            }

            var mainCamera = Camera.main;

            var vecDiff = (vector == Vector3.zero || _bearer.IsShotInProgress())
                ? (Input.mousePosition - new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight) / 2F)
                : vector;

            var direction = (Mathf.Atan2(vecDiff.y, vecDiff.x) * Mathf.Rad2Deg + 360) % 360;

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

        public override void Die()
        {
            base.Die();
            Debug.Log("PLAYER DEATH");
        }

        public override bool Damage(int amount)
        {
            if (base.Damage(amount))
            {
                HealthStats.SetInvisibleForTime(_invisibleTime);
                return true;
            }
            return false;
        }
    }   
}