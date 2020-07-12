using Pathfinding;
using System;
using UnityEngine;

namespace GMTK2020
{
    [RequireComponent(typeof(Seeker))]
    public abstract class Enemy : Actor, IDamageDealer
    {
        public int GetDamage => _damageFromTouch;
        
        [SerializeField] protected int _damageFromTouch;
        [SerializeField] protected float _pathUpdatingRate;

        protected const float WAYPOINT_DISTANCE = 0.5F;

        protected float _lastAnimChange;
        [SerializeField] protected float _agroRadius = 10F;
        protected bool _isAgroed;
        protected Player _player;
        protected Path _path;
        protected Seeker _seeker;
        [SerializeField] protected int _currentWayPoint;
        protected bool _reachedEndOfThePath;
        protected Vector3 _directionToCurrentWaypoint;
        protected bool _canContact;
        
        protected override void Awake()
        {
            FindPlayer();
            _seeker = GetComponent<Seeker>();
            base.Awake();
        }

        protected void Start()
        {
            UpdatePath();
            InvokeRepeating("UpdatePath", 1F, _pathUpdatingRate);
        }

        protected void FindPlayer() => _player = FindObjectOfType<Player>();

        protected void OnPathComplete(Path path)
        {
            if (!path.error)
            {
                _path = path;
                _currentWayPoint = 0;
            }
        }

        protected void UpdatePath()
        {
            if(_seeker.IsDone())
                _seeker.StartPath(_rigidbody.position, _player.transform.position, OnPathComplete);
        }

        protected void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _agroRadius);
        }

        protected virtual void FixedUpdate()
        {
            if (!_isAgroed && Vector3.Distance(transform.position, _player.transform.position) < _agroRadius)
                _isAgroed = true;

            if (!_isAgroed || !HandleAstarPath())
                return;
            
            MakeAIDecision();
            PlayActorAnimations();
        }

        protected bool HandleAstarPath()
        {
            if (_path != null && _currentWayPoint >= _path.vectorPath.Count)
                _reachedEndOfThePath = true;
            else
                _reachedEndOfThePath = false;

            if (_path == null || _path.vectorPath.Count < _currentWayPoint+1)
                return false;

            if (!_reachedEndOfThePath)
            {
                _directionToCurrentWaypoint = _path.vectorPath[_currentWayPoint] - transform.position;
                _directionToCurrentWaypoint.Normalize();
            }
            else
                _directionToCurrentWaypoint = Vector3.zero;

            var distance = Vector3.Distance(_rigidbody.position, _path.vectorPath[_currentWayPoint]);

            //Debug.Log($"Distance between: {distance}");
            if (distance <= WAYPOINT_DISTANCE)
                _currentWayPoint++;

            return true;
        }

        private void OnCollisionStay2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") && _canContact)
                other.gameObject.GetComponent<Player>().Damage(_damageFromTouch);
        }
        
        protected virtual void Move() => _rigidbody.velocity = _directionToCurrentWaypoint.normalized * _movingSpeed * Time.fixedDeltaTime
        ;

        protected override void PlayActorAnimations()
        {
            if (Time.time - _lastAnimChange < 0.15F)
                return;

            _lastAnimChange = Time.time;

            var animType = AnimType.Idle;
            var watchDir = WatchDirection.Down;
            var vector = _directionToCurrentWaypoint;
            var speed = 1F;
            
            if (Math.Abs(_rigidbody.velocity.x) >= 0.2F || Math.Abs(_rigidbody.velocity.y) >= 0.2f)
                animType = AnimType.Move;

            var vecDiff = vector == Vector3.zero
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
            
            AnimationController.AnimateActor(watchDir, animType);
        }
        
        protected abstract void Attack();
        protected abstract void MakeAIDecision();
        public override void Die()
        {
            base.Die();
            Destroy(gameObject);
        }
    }
}