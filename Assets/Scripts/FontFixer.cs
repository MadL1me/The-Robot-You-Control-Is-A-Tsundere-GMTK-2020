using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontFixer : MonoBehaviour
{
    private void Start()
    {
        var text = GetComponent<Text>();

        text.font.material.mainTexture.filterMode = FilterMode.Point;
    }
}
