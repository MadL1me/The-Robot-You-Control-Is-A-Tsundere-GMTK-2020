using System.Collections;
using Unity.Collections;
using UnityEngine;

namespace GMTK2020
{
    public class RobotDasher : Enemy
    {
        [SerializeField][ReadOnly] private bool _isDashing;

        [SerializeField] private float _prepareTime;
        [SerializeField] private float _beforeDashDistance;
        [SerializeField] private float _dashingSpeed;
        [SerializeField] private float _dashingLength;
        [SerializeField] private float _afterDashTimeout;
        
        protected override void Attack() => StartCoroutine(Charge());
        
        protected IEnumerator Charge()
        {
            _isDashing = true;
            
            var distance = _dashingLength * _dashingSpeed;
            var wait = new WaitForFixedUpdate();
            
            yield return new WaitForSeconds(_prepareTime);
            
            var direction = (_player.transform.position - transform.position).normalized;
            
            for (float i = 0; i < distance; i += _dashingSpeed)
            {
                _rigidbody.velocity = direction * _dashingSpeed * Time.fixedDeltaTime;
                yield return wait;
            }

            yield return new WaitForSeconds(_afterDashTimeout);
            
            _isDashing = false;
        }
        

        protected override void MakeAIDecision()
        {
            if (_isDashing)
                return;
            
            if (Vector3.Distance(transform.position, _player.transform.position) <= _beforeDashDistance)
                Attack();
            else 
                Move();
        }
    }
}