using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField] private WeaponBearer _weaponBearer;

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        _text.text = $"{_weaponBearer.CurrentWeapon?.CurrentRounds ?? 0}";
    }
}
