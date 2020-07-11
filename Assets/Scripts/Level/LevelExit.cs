using GMTK2020.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private LevelManager _manager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.gameObject;

        if (other.CompareTag("Player") && _manager.IsCompleted)
            _manager.ProceedToNextLevel();
    }
}
