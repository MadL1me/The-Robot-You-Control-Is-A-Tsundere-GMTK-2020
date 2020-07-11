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
        
        protected Player _player;
        protected Path _path;
        protected Seeker _seeker;
        [SerializeField] protected int _currentWayPoint;
        protected bool _reachedEndOfThePath;
        protected Vector3 _directionToCurrentWaypoint;
        
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
        
        protected virtual void FixedUpdate()
        {
            if (!HandleAstarPath())
                return;
            
            MakeAIDecision();
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
            if (other.gameObject.CompareTag("Player"))
                other.gameObject.GetComponent<Player>().Damage(_damageFromTouch);
        }

        protected abstract void Attack();
        protected abstract void Move();
        protected abstract void MakeAIDecision();
        public override void Die() { Destroy(gameObject); }
    }
}