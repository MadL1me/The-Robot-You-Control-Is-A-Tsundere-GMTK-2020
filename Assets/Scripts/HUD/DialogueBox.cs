using GMTK2020;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    private const float BOX_APPEAR_ANIM_DURATION = 0.25F;
    private const float BOX_TARGET_WIDTH = 5F;
    private const float BOX_TYPE_SPEED = 0.03F;

    [SerializeField] private RectTransform _imageMiddle;
    [SerializeField] private RectTransform _imageLeft;
    [SerializeField] private RectTransform _imageRight;
    [SerializeField] private Image _blinker;
    [SerializeField] private Text _text;
    [SerializeField] private PlayerMovement _movement;

    public bool IsShown { get; private set; }

    private bool _playingAnim;
    private bool _isTyping;
    private bool _shouldSkip;
    private string[] _lines;
    private int _currentLine;

    public void DisplaySpeech(string[] lines)
    {
        if (IsShown || _playingAnim)
            return;

        _currentLine = 0;
        _lines = lines;

        StartCoroutine(PlayAppearAnim());
    }

    public void Skip()
    {
        if (!_playingAnim)
        {
            if (_isTyping)
                _shouldSkip = true;
            else if (_currentLine < _lines.Length)
                StartCoroutine(TypeLine(_lines[_currentLine]));
            else
                StartCoroutine(PlayDisappearAnim());
        }
    }

    private IEnumerator PlayAppearAnim()
    {
        _movement.DisableAllInput = true;

        _playingAnim = true;

        _blinker.enabled = false;

        transform.localScale = new Vector3(1F, 0F, 1F);
        _imageMiddle.transform.localScale = new Vector3(0F, 1F, 1F);
        _imageLeft.transform.localPosition = new Vector3(0F, 0F, 0F);
        _imageRight.transform.localPosition = new Vector3(0F, 0F, 0F);
        _text.text = "";

        for (float i = 0F; i < BOX_APPEAR_ANIM_DURATION * 0.3F; i += Time.deltaTime)
        {
            float prog = i / (BOX_APPEAR_ANIM_DURATION * 0.3F);

            transform.localScale = new Vector3(1F, prog, 1F);

            yield return null;
        }

        for (float i = 0F; i < BOX_APPEAR_ANIM_DURATION * 0.7F; i += Time.deltaTime)
        {
            float prog = i / (BOX_APPEAR_ANIM_DURATION * 0.7F);

            _imageMiddle.transform.localScale = new Vector3(prog * BOX_TARGET_WIDTH, 1F, 1F);
            _imageLeft.transform.localPosition = new Vector3(-prog * BOX_TARGET_WIDTH * _imageMiddle.rect.width * 0.5F, 0F, 0F);
            _imageRight.transform.localPosition = new Vector3(prog * BOX_TARGET_WIDTH * _imageMiddle.rect.width * 0.5F, 0F, 0F);

            yield return null;
        }

        transform.localScale = new Vector3(1F, 1F, 1F);
        _imageMiddle.transform.localScale = new Vector3(BOX_TARGET_WIDTH, 1F, 1F);
        _imageLeft.transform.localPosition = new Vector3(-BOX_TARGET_WIDTH * _imageMiddle.rect.width * 0.5F, 0F, 0F);
        _imageRight.transform.localPosition = new Vector3(BOX_TARGET_WIDTH * _imageMiddle.rect.width * 0.5F, 0F, 0F);

        _playingAnim = false;
        IsShown = true;

        if (_lines.Length != 0)
            StartCoroutine(TypeLine(_lines[0]));
    }

    private IEnumerator PlayDisappearAnim()
    {
        _playingAnim = true;

        _text.text = "";
        _blinker.enabled = false;

        transform.localScale = new Vector3(1F, 1F, 1F);
        _imageMiddle.transform.localScale = new Vector3(BOX_TARGET_WIDTH, 1F, 1F);
        _imageLeft.transform.localPosition = new Vector3(-BOX_TARGET_WIDTH * _imageMiddle.rect.width * 0.5F, 0F, 0F);
        _imageRight.transform.localPosition = new Vector3(BOX_TARGET_WIDTH * _imageMiddle.rect.width * 0.5F, 0F, 0F);

        for (float i = 0F; i < BOX_APPEAR_ANIM_DURATION * 0.7F; i += Time.deltaTime)
        {
            float prog = 1F - i / (BOX_APPEAR_ANIM_DURATION * 0.7F);

            _imageMiddle.transform.localScale = new Vector3(prog * BOX_TARGET_WIDTH, 1F, 1F);
            _imageLeft.transform.localPosition = new Vector3(-prog * BOX_TARGET_WIDTH * _imageMiddle.rect.width * 0.5F, 0F, 0F);
            _imageRight.transform.localPosition = new Vector3(prog * BOX_TARGET_WIDTH * _imageMiddle.rect.width * 0.5F, 0F, 0F);

            yield return null;
        }

        for (float i = 0F; i < BOX_APPEAR_ANIM_DURATION * 0.3F; i += Time.deltaTime)
        {
            float prog = 1F - i / (BOX_APPEAR_ANIM_DURATION * 0.3F);

            transform.localScale = new Vector3(1F, prog, 1F);

            yield return null;
        }

        transform.localScale = new Vector3(1F, 0F, 1F);
        _imageMiddle.transform.localScale = new Vector3(0F, 1F, 1F);
        _imageLeft.transform.localPosition = new Vector3(0F, 0F, 0F);
        _imageRight.transform.localPosition = new Vector3(0F, 0F, 0F);

        _playingAnim = false;
        IsShown = false;

        _movement.DisableAllInput = false;
    }

    private IEnumerator TypeLine(string line)
    {
        _blinker.enabled = false;
        _shouldSkip = false;
        _isTyping = true;
        _text.text = "";
        _text.rectTransform.sizeDelta = new Vector2(BOX_TARGET_WIDTH * _imageMiddle.rect.width, _imageMiddle.rect.height);

        for (int i = 0; i < line.Length && !_shouldSkip; i++)
        {
            _text.text += line[i];

            yield return new WaitForSeconds(BOX_TYPE_SPEED);
        }

        _text.text = line;

        _isTyping = false;
        _currentLine++;
        _blinker.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            DisplaySpeech(new[] { "Hello, this is first line", "Hello, this is second line", "Hello, this is third line" });

        if (IsShown && Input.GetKeyDown(KeyCode.Space))
            Skip();
    }
}
