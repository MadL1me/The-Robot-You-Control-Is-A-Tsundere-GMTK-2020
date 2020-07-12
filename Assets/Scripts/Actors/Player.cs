using System;
using System.Collections;
using System.Collections.Generic;
using GMTK2020;
using UnityEngine;
using random = UnityEngine.Random;

namespace GMTK2020
{
    public class Player : Actor
    {
        private const float BLINKING_RATE = 0.5F;
        
        [SerializeField] private float _invisibleTime;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _getDamageClips;
            
        private PlayerMovement _movement;
        private WeaponBearer _bearer;
        
        protected override void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _bearer = GetComponent<WeaponBearer>();
            base.Awake();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _bearer.TrySetWeapon(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                _bearer.TrySetWeapon(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                _bearer.TrySetWeapon(2);

            if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
                _bearer.TrySetWeapon(_bearer.ActiveWeapon + 1);

            else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
                _bearer.TrySetWeapon(_bearer.ActiveWeapon - 1); ;

            base.Update();
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
                _audioSource.clip = _getDamageClips[random.Range(0, _getDamageClips.Length)];
                _audioSource.Play();
                HealthStats.SetInvisibleForTime(_invisibleTime);
                StartCoroutine(BlinkForTime(_invisibleTime));
                Debug.Log($"Damage amount: {amount}");
                Debug.Log($"Player health is: {HealthStats.Health}");
                return true;
            }
            return false;
        }

        private IEnumerator BlinkForTime(float time)
        {
            var wait = new WaitForSeconds(BLINKING_RATE);
            var colorBefore = _spriteRenderer.color;
            
            for (float i = 0; i < time; i += BLINKING_RATE*2)
            {
                colorBefore = _spriteRenderer.color;
                _spriteRenderer.color = Color.clear;
                yield return wait;
                _spriteRenderer.color = colorBefore;
                yield return wait;
            }
        }
    }   
}