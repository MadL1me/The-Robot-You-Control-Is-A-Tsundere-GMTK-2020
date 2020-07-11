using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject FocusTarget;
    public float Smoothness = 0.1F;
    public float Elasticity = 1F;
    public float WobbleStrength = 0.1F;
    public float WobbleSpeed = 2F;

    private Vector3 _curPos;
    private Camera _camera;
    private float _shakeAmount;
    private float _wobbleCounter;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public void Shake(float strength)
    {
        _shakeAmount = strength;
    }

    private void Update()
    {
        var wobbleSpeed = WobbleSpeed + Mathf.Min(_shakeAmount, 2F) * 4F;

        _wobbleCounter += Time.deltaTime * wobbleSpeed;

        if (_shakeAmount > 0)
            _shakeAmount -= Time.deltaTime * 10;
    }

    private void FixedUpdate()
    {
        var screenSize = new Vector3(_camera.pixelWidth, _camera.pixelHeight);

        var targetPos = FocusTarget.transform.position;
        targetPos.z = -10F;
        targetPos += (Input.mousePosition - screenSize / 2F) / ((screenSize.x + screenSize.y) / 2F) * Elasticity;

        _curPos = Vector3.Lerp(_curPos, targetPos, Smoothness);
    }

    private void LateUpdate()
    {
        var wobbleStrength = WobbleStrength + _shakeAmount * 0.05F;

        transform.position = _curPos +
            new Vector3(Mathf.PerlinNoise(_wobbleCounter, 0) * 2 - 1F, Mathf.PerlinNoise(0, _wobbleCounter) * 2 - 1F) * wobbleStrength;

        transform.rotation = Quaternion.Euler(0, 0, (Mathf.PerlinNoise(_wobbleCounter, _wobbleCounter) * 2 - 1F) * wobbleStrength);
    }
}
