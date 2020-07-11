using GMTK2020.Level;
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
        public AnimationController AnimationController { get; private set; }
        
        protected Rigidbody2D _rigidbody;
        protected SpriteRenderer _spriteRenderer;
        
        [SerializeField] protected float _movingSpeed;
        [SerializeField] protected LevelManager _musicManager;
        
        public bool CanTakeDamage() =>
            !HealthStats.IsInvisible;

        public virtual bool Damage(int amount)
        {
            if (!CanTakeDamage())
                return false;

            HealthStats.Health -= amount;
            _musicManager.Intensify();

            return true;
        }
        
        protected virtual void Awake()
        {
            AnimationController = GetComponent<AnimationController>();
            HealthStats = GetComponent<HealthComponent>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            SubscribeOnEvents();
        }
        
        protected IEnumerator RedColoringEffect(float speed)
        {
            //var wait = new WaitForSeconds();
            var colorBefore = _spriteRenderer.color;
            
            for (float i = 0; i < 1; i += Time.deltaTime)
            {
                colorBefore = _spriteRenderer.color;
                _spriteRenderer.color = Color.clear;
                //yield return wait;
                _spriteRenderer.color = colorBefore;
                yield return null;
            }
        }

        protected virtual void Update() => PlayActorAnimations();
        protected abstract void PlayActorAnimations();
        protected virtual void SubscribeOnEvents() => HealthStats.OnHealthEnd += Die;

        public virtual void Die() => OnActorDeath?.Invoke();
    }
}