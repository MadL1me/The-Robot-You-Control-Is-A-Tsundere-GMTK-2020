using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMTK2020
{
    [RequireComponent(typeof(HealthComponent))]
    public abstract class Actor : MonoBehaviour
    {
        public event Action OnActorDeath;

        public ProjectileSide Side;
        
        protected HealthComponent _health;
        protected Rigidbody2D _rigidbody;
        
        [SerializeField] protected float _movingSpeed;
        
        protected virtual void Awake()
        {
            _health = GetComponent<HealthComponent>();
            _rigidbody = GetComponent<Rigidbody2D>();
            
            SubscribeOnEvents();
        }

        protected virtual void SubscribeOnEvents()
        {
            OnActorDeath += Die;
            _health.OnHealthEnd += OnActorDeath;
        }

        public abstract void Die();
    }
}