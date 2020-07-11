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

        protected override void Awake()
        {
            _bearer = GetComponent<WeaponBearer>();
            base.Awake();
        }

        public override void Die()
        {
            Debug.Log("PLAYER DEATH");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                HealthStats.Health -= 100;

            if (Input.GetKeyDown(KeyCode.R) && _bearer.CanReload())
                _bearer.Reload();

            if (Input.GetKeyDown(KeyCode.Alpha1))
                _bearer.TrySetWeapon(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                _bearer.TrySetWeapon(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                _bearer.TrySetWeapon(2);

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