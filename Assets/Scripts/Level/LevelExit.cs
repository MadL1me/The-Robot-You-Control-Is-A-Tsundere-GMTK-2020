using GMTK2020;
using GMTK2020.Level;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private const float LEVEL_FADEOUT_ANIM_DURATION = 1F;

    [SerializeField] private LevelManager _manager;

    public bool IsOpen => _manager.IsCompleted;

    private bool _isAnimating;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var other = collision.gameObject;

        if (other.CompareTag("Player") && IsOpen && !_isAnimating)
        {
            _isAnimating = true;
            StartCoroutine(PlayFadeOutAnim(other.GetComponent<PlayerMovement>()));
        }
    }

    private IEnumerator PlayFadeOutAnim(PlayerMovement player)
    {
        var camera = Camera.main;
        var cameraEffect = camera.GetComponent<CameraFeedEffect>();
        var initialSize = camera.orthographicSize;

        player.DisableAllInput = true;

        for (float i = 0F; i < LEVEL_FADEOUT_ANIM_DURATION; i += Time.deltaTime)
        {
            cameraEffect.FadeOutAmount = i / LEVEL_FADEOUT_ANIM_DURATION;
            camera.orthographicSize = Mathf.Lerp(initialSize, initialSize * 0.25F, i / LEVEL_FADEOUT_ANIM_DURATION);

            yield return null;
        }

        _manager.ProceedToNextLevel();
    }
}
