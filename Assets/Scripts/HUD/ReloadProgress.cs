using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadProgress : MonoBehaviour
{
    [SerializeField] private WeaponBearer _weaponBearer;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (_weaponBearer.IsReloading)
        {
            _image.enabled = true;
            _image.fillAmount = _weaponBearer.GetReloadProgress();
        }
        else
        {
            _image.enabled = false;
        }
    }
}
