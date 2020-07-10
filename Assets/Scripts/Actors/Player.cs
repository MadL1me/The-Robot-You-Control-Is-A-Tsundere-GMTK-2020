using System;
using System.Collections;
using System.Collections.Generic;
using GMTK2020;
using UnityEngine;

namespace GMTK2020
{
    public class Player : Actor
    {
        public override void OnActorDeath()
        {
            Debug.Log("PLAYER DEATH");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                _health.Health -= 200;
        }
    }   
}