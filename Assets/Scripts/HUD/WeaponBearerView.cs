using System;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK2020.HUD
{
    public class WeaponBearerView : MonoBehaviour
    {
        [SerializeField] private Image _weaponImage;
        [SerializeField] private Text _weaponName;
        [SerializeField] private WeaponBearer _weaponBearer;

        private void Update()
        {
            _weaponName.text = _weaponBearer?.CurrentWeapon.WeaponConfig.WeaponName;
            _weaponImage.sprite = _weaponBearer?.CurrentWeapon.WeaponConfig.WeaponSprite;
        }
    }
}