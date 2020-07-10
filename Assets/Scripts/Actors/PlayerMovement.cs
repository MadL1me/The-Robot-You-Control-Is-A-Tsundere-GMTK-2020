using System;
using UnityEngine;

namespace GMTK2020
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
    }
}