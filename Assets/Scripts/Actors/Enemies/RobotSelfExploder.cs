using System;
using System.Collections;
using UnityEngine;

namespace GMTK2020
{
    public class RobotSelfExploder : Enemy
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private float _explosionTime;
        [SerializeField] private int _bulletsAmount;
        [SerializeField] private float _distanceToPlayer;
        [SerializeField] private float _radius;
        
        private bool _isExploding;
        
        protected override void Attack() => StartCoroutine(Explode());

        protected IEnumerator Explode()
        {
            _isExploding = true;
            
            yield return new WaitForSecondsRealtime(_explosionTime);

            var bulletPerAngle = 360 / _bulletsAmount;
            var angle = 0;
            
            for (int i = 0; i < _bulletsAmount; i++)
            {
                var bullet = Instantiate(_bulletPrefab).GetComponent<Bullet>();
                bullet.Side = ProjectileSide.Opponent;
                bullet.transform.position = transform.position + new Vector3(_radius*Mathf.Sin(angle), _radius*Mathf.Cos(angle));
                bullet.transform.rotation = Quaternion.LookRotation(bullet.transform.position-transform.position, transform.forward);
                angle += bulletPerAngle;
            }
            
            _isExploding = false;
            Destroy(gameObject);
        }

        protected override void MakeAIDecision()
        {
            if (_isExploding)
                return;

            if (Vector3.Distance(transform.position, _player.transform.position) <= _distanceToPlayer)
                Explode();
            else
                Attack();
        }
    }
} 