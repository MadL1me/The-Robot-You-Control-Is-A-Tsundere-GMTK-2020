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
        
        protected override void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            base.Awake();
        }

        protected override void PlayActorAnimations()
        {
            var animType = AnimType.Idle;
            var watchDir = WatchDirection.Down;
            var vector = _movement.MoveVector;
            
            
            if (Math.Abs(vector.x) >= 0.2F || Math.Abs(vector.y) >= 0.2f)
                animType = AnimType.Move;
                
            if (Input.GetButtonDown("Fire1"))
                animType = AnimType.Shoot;

            var mousePosition = Input.mousePosition;
            var screenHeight = Screen.currentResolution.height;
            var screenWidth = Screen.currentResolution.height;
            var aspectRatio = Camera.main.aspect;
            
            if (mousePosition.y > screenHeight/2 && mousePosition.x * aspectRatio >= mousePosition.y)
                watchDir = WatchDirection.Up;
            else if (mousePosition.y < screenHeight/2 && mousePosition.x * aspectRatio >= mousePosition.y)
                watchDir = WatchDirection.Down;
            else if (mousePosition.x > screenWidth/2 && mousePosition.y * aspectRatio >= mousePosition.x)
                watchDir = WatchDirection.Right;
            else
                watchDir = WatchDirection.Left;
            
            AnimationController.AnimateActor(watchDir, animType);
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