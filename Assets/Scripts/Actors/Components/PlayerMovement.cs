using System;
using System.Collections;
using UnityEngine;

namespace GMTK2020
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        
        [SerializeField] private float _movingSpeed;
        [SerializeField] private float _dashingSpeed;
        [SerializeField] private float _dashingLength;
        [SerializeField] private float _dashTimeout;
        
        private bool _isDashing;
        private float _horizontalMove;
        private float _verticalMove;
        private Vector3 _moveVector;

        private HealthComponent _healthComponent;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _healthComponent = GetComponent<HealthComponent>();
        }

        private void Update()
        {
            HandleInputs();
        }

        private void HandleInputs()
        {
            if (_isDashing)
                return;
            
            _horizontalMove = Input.GetAxisRaw("Horizontal");
            _verticalMove = Input.GetAxisRaw("Vertical");

            _moveVector = new Vector3(_horizontalMove, _verticalMove).normalized;
            
            if (Input.GetKeyDown(KeyCode.Space))
                StartCoroutine(Dash());
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = (_moveVector * _movingSpeed * Time.fixedDeltaTime);
        }

        private IEnumerator Dash()
        {
            _isDashing = true;
            _healthComponent.IsInvisible = true;
            
            var curPosition = transform.position;
            var nextPosition = _moveVector * _dashingLength * _dashingSpeed;
            var difference = (nextPosition - curPosition).normalized;
            var wait = new WaitForFixedUpdate();
            
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                _rigidbody.velocity = difference * _dashingSpeed;
                yield return wait;
            }

            _healthComponent.IsInvisible = false;
            yield return new WaitForSeconds(_dashTimeout);
            _isDashing = false;
        }
    }
}