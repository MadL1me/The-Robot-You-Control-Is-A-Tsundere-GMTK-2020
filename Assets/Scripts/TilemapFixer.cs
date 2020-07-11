using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapFixer : MonoBehaviour
{
    void Start()
    {
        GetComponent<TilemapRenderer>().receiveShadows = true;
    }
}
