using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private WeaponBearer _bearer;

    private Transform[] _pieces;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        _pieces = Enumerable.Range(0, transform.childCount)
            .Select(x => transform.GetChild(x))
            .ToArray();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    private void LateUpdate()
    {
        var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0F;

        transform.position = worldPos;
        transform.rotation = Quaternion.Euler(0, 0, Time.time * 45F);

        for (int i = 0; i < _pieces.Length; i++)
        {
            var factor = i / (float)_pieces.Length * Mathf.PI * 2;

            var spread = 0.2F;

            if (_bearer.CurrentWeapon != null)
                spread = _bearer.CurrentWeapon.WeaponConfig.Spread * 0.03F + 0.2F;

            _pieces[i].localPosition = new Vector3(Mathf.Cos(factor), Mathf.Sin(factor)) * spread;
        }
    }
}
