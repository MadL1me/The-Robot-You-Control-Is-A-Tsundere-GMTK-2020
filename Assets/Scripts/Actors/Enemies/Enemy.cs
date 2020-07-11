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

        protected const float WAYPOINT_DISTANCE = 3F;
        
        protected Player _player;
        protected Path _path;
        protected Seeker _seeker;
        protected int _currentWayPoint;
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
            HandleAstarPath();

            if (_reachedEndOfThePath || _path == null)
                return;

            _directionToCurrentWaypoint = _path.vectorPath[_currentWayPoint+1]-transform.position;
            
            MakeAIDecision();
        }

        protected void HandleAstarPath()
        {
            if (_path != null && _currentWayPoint == _path.vectorPath.Count)
                _reachedEndOfThePath = true;
            else
                _reachedEndOfThePath = false;

            var distance = Vector3.Distance(_rigidbody.position, _path.vectorPath[_currentWayPoint]);
            if (distance <= WAYPOINT_DISTANCE)
                _currentWayPoint++;
        }

        protected abstract void Attack();
        protected abstract void Move();
        protected abstract void MakeAIDecision();
        
        public override void Die() { Destroy(gameObject); }
    }
}