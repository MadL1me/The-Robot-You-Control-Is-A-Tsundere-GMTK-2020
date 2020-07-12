using GMTK2020;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public enum DialogueCharAnim
{
    None = 0,
    Normal = 1,
    Smile = 2
}

public struct DialogueBoxLine
{
    public DialogueCharAnim Animation;
    public string Line;

    public DialogueBoxLine(DialogueCharAnim anim, string line)
    {
        Animation = anim;
        Line = line;
    }
}

public class DialogueBox : MonoBehaviour
{
    private const float BOX_APPEAR_ANIM_DURATION = 0.25F;
    private const float BOX_TARGET_WIDTH = 7F;
    private const float BOX_TYPE_SPEED = 0.03F;

    [SerializeField] private RectTransform _imageMiddle;
    [SerializeField] private RectTransform _imageLeft;
    [SerializeField] private RectTransform _imageRight;
    [SerializeField] private Image _blinker;
    [SerializeField] private Image _character;
    [SerializeField] private Text _text;
    [SerializeField] private AudioSource _source;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private Sprite[] _charSprites;

    public bool IsShown { get; private set; }

    private bool _playingAnim;
    private bool _isTyping;
    private bool _shouldSkip;
    private DialogueBoxLine[] _lines;
    private int _currentLine;
    private float _initialCharPos;
    private DialogueCharAnim _lastAnim;

    private void Start()
    {
        _initialCharPos = _character.rectTransform.position.x;
    }

    public void DisplaySpeech(DialogueBoxLine[] lines)
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

    private IEnumerator AnimateCharSprite(DialogueCharAnim anim, float duration)
    {
        if (_lastAnim == anim)
            yield break;

        if (_lastAnim == DialogueCharAnim.None)
        {
            _character.color = Color.clear;
            _character.transform.position = new Vector3(_initialCharPos - 48F, _character.transform.position.y);
            _character.sprite = _charSprites[(int)anim - 1];

            for (float i = 0F; i < duration && !_shouldSkip; i += Time.deltaTime)
            {
                _character.color = new Color(1F, 1F, 1F, i / duration);
                _character.transform.position = new Vector3(_initialCharPos - 48F + 48F * i / duration, _character.transform.position.y);

                yield return null;
            }

            _character.color = Color.white;
            _character.transform.position = new Vector3(_initialCharPos, _character.transform.position.y);
        }
        else if (anim != DialogueCharAnim.None)
        {
            _character.sprite = _charSprites[(int)anim - 1];
        }
        else
        {
            _character.color = Color.white;
            _character.transform.position = new Vector3(_initialCharPos, _character.transform.position.y);

            for (float i = 0F; i < duration && !_shouldSkip; i += Time.deltaTime)
            {
                _character.color = new Color(1F, 1F, 1F, 1F - i / duration);
                _character.transform.position = new Vector3(_initialCharPos - 48F * i / duration, _character.transform.position.y);

                yield return null;
            }

            _character.color = Color.clear;
            _character.transform.position = new Vector3(_initialCharPos - 48F, _character.transform.position.y);
        }

        _lastAnim = anim;
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

        StartCoroutine(AnimateCharSprite(DialogueCharAnim.None, BOX_APPEAR_ANIM_DURATION * 0.5F));

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

    private IEnumerator TypeLine(DialogueBoxLine line)
    {
        _blinker.enabled = false;
        _shouldSkip = false;
        _isTyping = true;
        _text.text = "";
        _text.rectTransform.sizeDelta = new Vector2(BOX_TARGET_WIDTH * _imageMiddle.rect.width, _imageMiddle.rect.height);

        StartCoroutine(AnimateCharSprite(line.Animation, BOX_APPEAR_ANIM_DURATION * 0.25F));

        for (int i = 0; i < line.Line.Length && !_shouldSkip; i++)
        {
            _source.PlayOneShot(_source.clip);
            _text.text += line.Line[i];

            yield return new WaitForSeconds(BOX_TYPE_SPEED);
        }

        _text.text = line.Line;

        _isTyping = false;
        _currentLine++;
        _blinker.enabled = true;
    }

    private void Update()
    {
        // TODO: Remove
        if (Input.GetKeyDown(KeyCode.F))
            DisplaySpeech(new[] {
                new DialogueBoxLine(DialogueCharAnim.None, "First line"),
                new DialogueBoxLine(DialogueCharAnim.Normal, "Second line"),
                new DialogueBoxLine(DialogueCharAnim.Smile, "Third line"),
            });

        if (IsShown && Input.GetKeyDown(KeyCode.Space))
            Skip();
    }
}
