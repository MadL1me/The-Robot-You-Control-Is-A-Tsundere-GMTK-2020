using System;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK2020.HUD
{
    public class WeaponBearerView : MonoBehaviour
    {
        [SerializeField] private Text _weaponName;
        [SerializeField] private WeaponBearer _weaponBearer;

        private void Update()
        {
            _weaponName.text = _weaponBearer?.CurrentWeapon.WeaponConfig.WeaponName;
        }
    }
}