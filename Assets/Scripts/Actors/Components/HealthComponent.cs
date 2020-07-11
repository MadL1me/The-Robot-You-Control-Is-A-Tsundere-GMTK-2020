using System;
using System.Collections;
using UnityEngine;

namespace GMTK2020
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action OnHealthEnd;
        public event Action<int, int> OnHealthChange;

        public bool IsInvisible { get; set; }
        
        private void Awake()
        {
            _health = _maxHealth;
        }

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                
                OnHealthChange?.Invoke(value, _maxHealth);
                
                if (value <= 0)
                    OnHealthEnd?.Invoke();
                
                if (value >= _maxHealth)
                    _health = _maxHealth;
            } 
        }

        public void SetInvisibleForTime(float time) => StartCoroutine(SetInvisibleForTimeCoroutine(time));
        
        private IEnumerator SetInvisibleForTimeCoroutine(float time)
        {
            IsInvisible = true;
            yield return new WaitForSeconds(time);
            IsInvisible = false;
        }

        [SerializeField] private int _maxHealth;
        [SerializeField] private int _health;
    }
}