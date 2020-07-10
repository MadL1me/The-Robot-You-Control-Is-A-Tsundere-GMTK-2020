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
        
        protected HealthComponent _health;
        [SerializeField] protected float _movingSpeed;
        
        protected virtual void Awake()
        {
            _health = GetComponent<HealthComponent>();
            
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