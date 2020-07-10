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

        private bool _isDashing;
       
        private float _horizontalMove;
        private float _verticalMove;
        private Vector3 _moveVector;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
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
            
            transform.Translate(_moveVector * _movingSpeed * Time.deltaTime);
        }

        private IEnumerator Dash()
        {
            _isDashing = true;

            var curPosition = transform.position;
            var nextPosition = _moveVector * _dashingLength * _dashingSpeed;
            var wait = new WaitForFixedUpdate();
            
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                transform.position = Vector3.Lerp(curPosition, nextPosition, i);
                yield return wait;
            }

            _isDashing = false;
        }
    }
}