using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float ParallaxStrength = 0F;

    private Vector3 _initialPos;
    private Camera _camera;

    private void Start()
    {
        _initialPos = transform.position;
        _camera = Camera.main;
    }

    private void Update()
    {
        var normalPos = new Vector3(Mathf.Clamp(Input.mousePosition.x / (float)_camera.pixelWidth, 0F, 1F), Mathf.Clamp(Input.mousePosition.y / (float)_camera.pixelHeight, 0F, 1F));

        transform.position = _initialPos + -(normalPos - new Vector3(0.5F, 0.5F)) * ParallaxStrength * 100;
    }
}
