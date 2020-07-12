using GMTK2020;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEventHandler : MonoBehaviour
{
    [SerializeField] private GameObject _closingTilemap;
    [SerializeField] private MusicManager _music;
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private Boss _boss;
    [SerializeField] private DialogueBox _box;
    private bool _isDoorClosed;

    private void Start()
    {
        StartCoroutine(EndDialog());
    }

    private IEnumerator EndDialog()
    {
        yield return new WaitUntil(() => _boss.IsDead);

        _box.DisplaySpeech(new[]
        {
            new DialogueBoxLine(DialogueCharAnim.RealSmile, "Yay!!! We did it!", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Stare, "I.. I mean I did it!", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Stare, "That's what I was trying to say.", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Stare, "And you are still an idiot.", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Stare, "...", 1.75F),
        });
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!_isDoorClosed && other.gameObject.CompareTag("Player"))
            HandlePlayerComeToBoss();
    }

    private void HandlePlayerComeToBoss()
    {
        _camera.Shake(8F);
        _music.Restart();
        _music.DisableMusic = false;
        _closingTilemap.SetActive(true);
        _isDoorClosed = true;
    }
}