using System;
using System.Collections;
using System.Collections.Generic;
using GMTK2020;
using UnityEngine;

namespace GMTK2020
{
    public class Player : Actor
    {
        [SerializeField] private Camera _camera;

        private WeaponBearer _bearer;

        public void Awake()
        {
            _bearer = GetComponent<WeaponBearer>();
        }

        public override void Die()
        {
            Debug.Log("PLAYER DEATH");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                _health.Health -= 200;

            if (Input.GetKeyDown(KeyCode.R) && _bearer.CanReload())
                _bearer.Reload();

            if (_bearer.CurrentWeapon?.WeaponType.IsAutomatic == true ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0))
            {
                var vecDiff = Input.mousePosition - new Vector3(_camera.pixelWidth, _camera.pixelHeight) / 2F;

                var direction = (Mathf.Atan2(vecDiff.y, vecDiff.x) * Mathf.Rad2Deg + 360) % 360;

                Debug.Log(direction);

                _bearer.Shoot(direction);
            }
        }
    }   
}