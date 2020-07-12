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
        protected Color _originalColor;
        
        [SerializeField] protected float _movingSpeed;
        [SerializeField] protected LevelManager _musicManager;

        public void SetMusicManager(LevelManager manager) => _musicManager = manager;
        
        public bool CanTakeDamage() =>
            !HealthStats.IsInvisible;

        public virtual bool Damage(int amount)
        {
            if (!CanTakeDamage())
                return false;

            HealthStats.Health -= amount;
            _musicManager.Intensify();
            StartCoroutine(RedColoringEffect(0.5F));
            
            return true;
        }
        
        protected virtual void Awake()
        {
            AnimationController = GetComponent<AnimationController>();
            HealthStats = GetComponent<HealthComponent>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _originalColor = _spriteRenderer.color;
            
            SubscribeOnEvents();
        }
        
        protected IEnumerator RedColoringEffect(float speed)
        {
            var wait = new WaitForFixedUpdate();
            var curColor = _spriteRenderer.color;
            
            for (float i = 0; i < 1; i += speed)
            {
                _spriteRenderer.color = Color.Lerp(curColor, Color.red, i);
                yield return wait;
            }
            
            _spriteRenderer.color = Color.red;

            for (float i = 0; i < 1; i+=speed)
            {
                _spriteRenderer.color = Color.Lerp(Color.red, _originalColor, i);
                yield return wait;
            }

            _spriteRenderer.color = _originalColor;
        }

        protected virtual void Update() => PlayActorAnimations();
        protected abstract void PlayActorAnimations();
        protected virtual void SubscribeOnEvents() => HealthStats.OnHealthEnd += Die;

        public virtual void Die() => OnActorDeath?.Invoke();
    }
}