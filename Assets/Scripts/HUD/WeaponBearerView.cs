using System;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK2020.HUD
{
    public class WeaponBearerView : MonoBehaviour
    {
        [SerializeField] private Image _weaponImage;
        [SerializeField] private Text _weaponName;
        
        public void SwitchWeapon(WeaponConfig weaponConfig)
        {
            _weaponName.text = weaponConfig.WeaponName;
            _weaponImage.sprite = weaponConfig.WeaponSprite;
        }
    }
}