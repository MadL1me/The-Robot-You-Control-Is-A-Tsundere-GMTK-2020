using GMTK2020;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadIndicator : MonoBehaviour
{
    [SerializeField] private WeaponBearer _weaponBearer;

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        if (_weaponBearer.RequiresReload())
            _text.enabled = true;
        else
            _text.enabled = false;
    }
}
