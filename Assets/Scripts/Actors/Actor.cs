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

        public bool CanTakeDamage() =>
            !HealthStats.IsInvisible;

        public virtual bool Damage(int amount)
        {
            if (!CanTakeDamage())
                return false;

            HealthStats.Health -= amount;

            return true;
        }
        
        protected virtual void Awake()
        {
            HealthStats = GetComponent<HealthComponent>();
            _rigidbody = GetComponent<Rigidbody2D>();
            
            SubscribeOnEvents();
        }

        protected virtual void SubscribeOnEvents()
        {
            OnActorDeath += Die;
            HealthStats.OnHealthEnd += OnActorDeath;
        }

        public abstract void Die();
    }
}