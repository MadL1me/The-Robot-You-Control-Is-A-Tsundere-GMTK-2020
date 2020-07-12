using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const float INTENSE_PART_DURATION = 10F;

    private AudioSource[] _sources;

    private float _initialVolume;
    private float _bossStart = int.MinValue;
    private float _intenseStart = int.MinValue;
    private int _currentLayer;

    private void Awake()
    {
        if (FindObjectsOfType<MusicManager>().Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        _sources = GetComponents<AudioSource>();

        _initialVolume = _sources[0].volume;

        for (int i = 1; i < _sources.Length; i++)
            _sources[1].volume = 0F;

        for (int i = 0; i < _sources.Length; i++)
            _sources[i].Play();
    }

    private void Update()
    {
        if (Time.time - _intenseStart > INTENSE_PART_DURATION)
            _currentLayer = 0;
        else
            _currentLayer = 1;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _sources.Length; i++)
        {
            if (_currentLayer == i)
                _sources[i].volume = Mathf.Lerp(_sources[i].volume, _initialVolume, 0.02F);
            else
                _sources[i].volume = Mathf.Lerp(_sources[i].volume, 0F, 0.02F);
        }
    }

    public void Intensify() =>
        _intenseStart = Time.time;

}
