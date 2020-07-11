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
        
        public HealthComponent HealthStats { get; private set; }

        protected Rigidbody2D _rigidbody;
        
        [SerializeField] protected float _movingSpeed;
        
        protected virtual void Awake()
        {
            HealthStats = GetComponent<HealthComponent>();
            _rigidbody = GetComponent<Rigidbody2D>();
            
            SubscribeOnEvents();
        }

        protected virtual void SubscribeOnEvents()
        {
            HealthStats.OnHealthEnd += Die;
        }

        public virtual void Die() => OnActorDeath?.Invoke();
    }
}