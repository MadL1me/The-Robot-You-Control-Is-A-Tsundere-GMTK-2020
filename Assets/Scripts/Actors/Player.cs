using System;
using System.Collections;
using System.Collections.Generic;
using GMTK2020;
using UnityEngine;

namespace GMTK2020
{
    public class Player : Actor
    {
        [SerializeField] private float _invisibleTime;
        
        protected override void Awake()
        {
            base.Awake();
        }

        public override void Die()
        {
            base.Die();
            Debug.Log("PLAYER DEATH");
        }

        public override bool Damage(int amount)
        {
            if (base.Damage(amount))
            {
                HealthStats.SetInvisibleForTime(_invisibleTime);
                return true;
            }

            return false;
        }
    }   
}