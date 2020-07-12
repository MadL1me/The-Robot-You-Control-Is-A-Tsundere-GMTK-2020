using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBlinker : MonoBehaviour
{
    private Text _text;
    private float _lastBlink;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        if (Time.time - _lastBlink > 0.5F)
        {
            _lastBlink = Time.time;
            _text.enabled = !_text.enabled;
        }
    }
}
