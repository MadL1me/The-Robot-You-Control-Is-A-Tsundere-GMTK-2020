using System;
using UnityEngine;

namespace GMTK2020
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action OnHealthEnd;
        public event Action<int, int> OnHealthChange;
        
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

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("DamageDealer"))
            {
                var damage = other.gameObject.GetComponent<IDamageDealer>().GetDamage;
                Debug.Log("Damage get");
                Health -= damage;
            }
        }

        [SerializeField] private int _maxHealth;
        [SerializeField] private int _health;
    }
}