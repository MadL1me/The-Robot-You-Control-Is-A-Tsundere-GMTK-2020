using GMTK2020;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RageIndicator : MonoBehaviour
{
    [SerializeField] private PlayerMovement _mov;
    [SerializeField] private PlayerRage _rage;

    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        if (_mov.GetCurrentGlitch() != GlitchType.None)
        {
            _text.color = Color.red;
            _text.text = $"{Mathf.CeilToInt(_mov.GetGlitchProgress())}";
        }
        else
        {
            _text.color = Color.white;
            _text.text = $"{Mathf.CeilToInt(_rage.GetRemainingTimeUntilRage())}";
        }
    }
}
