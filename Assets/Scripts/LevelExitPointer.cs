using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExitPointer : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LevelExit _exit;

    private SpriteRenderer _rdr;

    private void Start()
    {
        _rdr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_exit.IsOpen)
        {
            _rdr.forceRenderingOff = false;

            var pos = _camera.transform.position;
            pos.z = 0F;

            var vecDiff = _exit.transform.position - pos;

            var dist = vecDiff.magnitude;

            var direction = (Mathf.Atan2(vecDiff.y, vecDiff.x) * Mathf.Rad2Deg + 360) % 360;

            var rad = Mathf.Min(2F, dist * 0.5F) + Mathf.Sin(Time.time * 5F) * 0.5F;

            _rdr.color = new Color(1F, 1F, 1F, Mathf.Min(1F, (dist - 2F) * 0.1F));

            transform.position = pos + new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad) * rad, Mathf.Sin(direction * Mathf.Deg2Rad) * rad);
            transform.rotation = Quaternion.Euler(0F, 0F, direction);
        }
        else
        {
            _rdr.forceRenderingOff = true;
        }
    }
}
