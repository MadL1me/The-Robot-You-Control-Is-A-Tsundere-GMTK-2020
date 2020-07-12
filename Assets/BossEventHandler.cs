using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEventHandler : MonoBehaviour
{
    [SerializeField] private GameObject _closingTilemap;
    private bool _isDoorClosed;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!_isDoorClosed && other.gameObject.CompareTag("Player"))
            HandlePlayerComeToBoss();
    }

    private void HandlePlayerComeToBoss()
    {
        _closingTilemap.SetActive(true);
        _isDoorClosed = false;
    }
}