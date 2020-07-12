using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBoxBlinkerAnim : MonoBehaviour
{
    private float _startPoint;

    private void Start()
    {
        _startPoint = transform.localPosition.y;
    }

    private void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, _startPoint - Mathf.Abs(Mathf.Sin(Time.time * 10F) * 8));
    }
}
