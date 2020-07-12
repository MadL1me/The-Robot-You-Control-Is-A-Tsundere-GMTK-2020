using GMTK2020;
using GMTK2020.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFeedEffect : MonoBehaviour
{
    private const float LEVEL_FADEIN_ANIM_DURATION = 0.5F;

    [SerializeField] private Material _shaderMat;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private LevelManager _levelManager;

    private PlayerMovement _mov;

    public float FadeOutAmount { get; set; } = 1F;
    public bool ShouldFadeIn { get; set; } = true;

    private float _lastFactor;
    private float _randomFactor;
    private float _interference;

    public bool IsGlitchingOut => _playerMovement.GetCurrentGlitch() != GlitchType.None || _levelManager.IsCompleted;

    private void Start()
    {
        _mov = _playerMovement.GetComponent<PlayerMovement>();

        if (ShouldFadeIn)
            FadeIn();
    }

    public void FadeIn()
    {
        StartCoroutine(PlayFadeInAnim());
    }

    private IEnumerator PlayFadeInAnim()
    {
        for (float i = 0F; i < LEVEL_FADEIN_ANIM_DURATION; i += Time.deltaTime)
        {
            FadeOutAmount = 1F - i / LEVEL_FADEIN_ANIM_DURATION;
            yield return null;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (Time.time > _lastFactor + 0.25F)
        {
            _randomFactor = Random.Range(0.4F, 1.7F);
            _lastFactor = Time.time;
        }    

        var screenCutOff = 1F - (Mathf.Sin(Time.time * _randomFactor) + 1.25F);

        if (!IsGlitchingOut)
            screenCutOff = 1F;

        _shaderMat.SetFloat("_GlitchY", screenCutOff);
        _shaderMat.SetFloat("_FadeOutAmount", FadeOutAmount);

        if (_mov.GetCurrentGlitch() == GlitchType.Interference)
            _interference = Mathf.Lerp(_interference, 1F, 0.03F);
        else
            _interference = Mathf.Lerp(_interference, 0F, 0.03F);

        _shaderMat.SetFloat("_NoiseAmount", 0.2F + _interference * 0.4F);
        _shaderMat.SetFloat("_Interference", _interference);

        Graphics.Blit(source, destination, _shaderMat);
    }
}
