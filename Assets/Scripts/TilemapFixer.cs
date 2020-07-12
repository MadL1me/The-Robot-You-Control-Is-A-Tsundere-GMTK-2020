using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFixer : MonoBehaviour
{
    void Start()
    {
        GetComponent<TilemapRenderer>().receiveShadows = true;

        GetComponent<TilemapRenderer>().material.EnableKeyword("_SPECULARHIGHLIGHTS_OFF");
        GetComponent<TilemapRenderer>().material.SetFloat("_SpecularHighlights", 0f);
    }
}
