using System;
using System.Collections;
using UnityEngine;

namespace GMTK2020
{
    [RequireComponent(typeof(WeaponBearer))]
    public class Boss : Enemy
    {
        [SerializeField] protected WeaponBearer _weaponBearer;
        [SerializeField] private AudioClip[] _footstepClips;

        protected float _moveStart;
        protected bool _isMoving;
        protected float _lastShot;
        protected float _lastTeleport;
        protected bool _isTeleporting;
        protected CameraFollow _camera;
        private int _nextFootstep;
        private float _lastFootstep;
        private AudioSource _source;

        protected override void Awake()
        {
            base.Awake();
            //_weaponBearer.GetComponent<WeaponBearer>();

            _camera = Camera.main.GetComponent<CameraFollow>();
            _source = GetComponent<AudioSource>();
        }

        protected override void Attack()
        {
            var difference = _player.transform.position - transform.position;
            var angle = (Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + 360) % 360;
            if (_weaponBearer.Shoot(angle))
                _camera.Shake(3F);
        }

        protected override void Move()
        {
            if (Time.time - _moveStart < 1.75F && !_isTeleporting)
                base.Move();
            else
                _rigidbody.velocity = Vector3.zero;
        }

        private IEnumerator TeleportToPlayer()
        {
            _lastTeleport = Time.time;
            _isTeleporting = true;
            _canContact = false;

            transform.localScale = new Vector3(1F, 1F, 1F);

            for (float i = 0; i < 0.1F; i += Time.deltaTime)
            {
                _camera.Shake(2F);
                transform.localScale = new Vector3(1F - i / 0.1F, 1F + i / 0.1F, 1F);
                yield return null;
            }

            transform.localScale = new Vector3(0F, 2F, 1F);
            transform.position = _player.transform.position;

            yield return new WaitForSeconds(0.8F);

            for (float i = 0; i < 0.1F; i += Time.deltaTime)
            {
                _camera.Shake(2F);
                transform.localScale = new Vector3(i / 0.1F, 1F + (1F - i / 0.1F), 1F);
                yield return null;
            }

            transform.localScale = new Vector3(1F, 1F, 1F);

            _isTeleporting = false;
            _canContact = true;
        }

        protected override void MakeAIDecision()
        {
            if (!_isTeleporting && _rigidbody.velocity.magnitude > 0.2F && Time.time - _lastFootstep > 0.3F)
            {
                _camera.Shake(1F);
                _lastFootstep = Time.time;
                _source.PlayOneShot(_footstepClips[_nextFootstep], 0.4F);
                _nextFootstep = (_nextFootstep + 1) % _footstepClips.Length;
            }

            if (!_isTeleporting && IsCanShot())
                Attack();
            else if ((_player.transform.position - transform.position).magnitude > 5F && Time.time - _lastTeleport > 8F && !_isTeleporting)
                StartCoroutine(TeleportToPlayer());
            else
            {
                if (!_isMoving && Time.time - _moveStart > 2.75F)
                {
                    _isMoving = true;
                    _moveStart = Time.time;
                }

                Move();

                if (Time.time - _moveStart < 1.75F && _isMoving)
                    _isMoving = false;
            }
        }
        

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

            if (_weaponBearer.IsShotInProgress() || Time.time - _lastShot < 0.5F)
            {
                if (_weaponBearer.IsShotInProgress())
                    _lastShot = Time.time;
                animType = AnimType.Shoot;
            }

            var vecDiff = (vector == Vector3.zero || _weaponBearer.IsShotInProgress())
                ? _player.transform.position - transform.position
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
        
        private bool IsEnemySeesPlayer()
        {
            var raycast = Physics2D.Raycast(transform.position,
                (_player.transform.position - transform.position).normalized, 750f, ~LayerMask.GetMask("Enemy"));

            if (!raycast.collider)
                return false;
            
            return raycast.collider.gameObject.CompareTag("Player");
        }
    }
}