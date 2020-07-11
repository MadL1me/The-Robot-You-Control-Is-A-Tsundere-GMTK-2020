using System;
using UnityEngine;
using UnityEngine.UI;

namespace GMTK2020.HUD
{
    public class PlayerHPView : MonoBehaviour
    {
        [SerializeField] private Image[] _hp;

        [SerializeField] private Sprite _filledHpSprite;
        [SerializeField] private Sprite _emptyHpSprite;

        [SerializeField] private GameObject _hpBarPrefab;
        [SerializeField] private Transform _barsParent;

        private Player _player;
        
        private void Start()
        {
            var player = FindObjectOfType<Player>();
            player.HealthStats.OnHealthChange += SetHP;
            CreateHpBars(player.HealthStats.MaxHealth);
            SetHP(player.HealthStats.Health, player.HealthStats.MaxHealth);
            _player = player;
        }

        private void CreateHpBars(int maxHp)
        {
            _hp = new Image[maxHp];

            for (int i = 0; i < maxHp; i++)
            {
                var bar = Instantiate(_hpBarPrefab, _barsParent);
                var img = bar.GetComponent<Image>();
                _hp[i] = img;
            }
        }

        public void SetHP(int hp, int maxHp)
        {
            for (int i = 0; i < _hp.Length; i++)
            {
                if (hp > i)
                    _hp[i].sprite = _filledHpSprite;
                else
                    _hp[i].sprite = _emptyHpSprite;
            }
        }
    }
}