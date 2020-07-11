using System;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK2020.HUD
{
    public class PlayerHPView : MonoBehaviour
    {
        private Image _hp;

        private void Start()
        {
            _hp = GetComponentInChildren<Image>();
            _hp.fillAmount = 1;
            FindObjectOfType<Player>().HealthStats.OnHealthChange += SetHP;
        }

        public void SetHP(int hp, int maxHp)
        {
            float value = hp / (float)maxHp;
            _hp.fillAmount = value;
        }
    }
}