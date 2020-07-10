using System;
using Pathfinding;
using UnityEngine;

namespace GMTK2020
{
    public abstract class Enemy : Actor, IDamageDealer
    {
        public int GetDamage => _damageFromTouch;
        [SerializeField] protected int _damageFromTouch;

        protected Player _player;
        protected Path _path;
        protected Seeker _seeker;
        protected int _currentWayPoint;
        protected bool _reachedEndOfThePath;

        protected override void Awake()
        {
            FindPlayer();
            
            _seeker = GetComponent<Seeker>();
            _seeker.StartPath(_rigidbody.position, _player.transform.position, OnPathComplete);
            
            base.Awake();
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

        protected virtual void Update()
        {
            HandleAstarPath();
        }

        protected void HandleAstarPath()
        {
            if (_currentWayPoint == _path.vectorPath.Count)
                _reachedEndOfThePath = true;
            else
                _reachedEndOfThePath = false;
        }

        protected abstract void Attack();
        protected abstract void Move();
        protected abstract void MakeAIDecision();
        
        public override void Die() { Destroy(gameObject); }
    }
}