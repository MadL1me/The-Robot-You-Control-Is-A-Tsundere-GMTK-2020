using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueBoxLine[] Lines;

    [SerializeField] private DialogueBox _box;

    private bool _used;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_used && collision.gameObject.CompareTag("Player"))
        {
            _used = true;

            _box.DisplaySpeech(Lines);
        }
    }
}
