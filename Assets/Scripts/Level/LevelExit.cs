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
    [SerializeField] private Sprite[] _frames;

    private SpriteRenderer _rdr;

    public bool IsOpen => _manager.IsCompleted;

    private bool _isAnimating;
    private bool _wasOpen;

    private void Start()
    {
        _rdr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (IsOpen && !_wasOpen)
            StartCoroutine(PlayOpenAnim());

        _wasOpen = IsOpen;
    }

    private IEnumerator PlayOpenAnim()
    {
        var main = Camera.main;

        for (int i = 0; i < _frames.Length; i++)
        {
            if (i != 0)
                main.GetComponent<CameraFollow>().Shake(4F);

            _rdr.sprite = _frames[i];

            yield return new WaitForSeconds(0.3F);
        }
    }

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
