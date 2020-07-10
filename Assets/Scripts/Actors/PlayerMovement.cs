using System;
using UnityEngine;

namespace GMTK2020
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        [SerializeField] private float _movingSpeed;
        
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
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");

            var moveVector = new Vector3(horizontal, vertical).normalized * _movingSpeed * Time.deltaTime;
            
            transform.Translate(moveVector);
        }
    }
}