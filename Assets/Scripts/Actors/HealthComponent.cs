using System;
using UnityEngine;

namespace GMTK2020
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action OnHealthEnd;

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
                
                if (value <= 0)
                    OnHealthEnd?.Invoke();
                
                if (value >= _maxHealth)
                    _health = _maxHealth;
            } 
        }

        [SerializeField] private int _maxHealth;
        [SerializeField] private int _health;
    }
}