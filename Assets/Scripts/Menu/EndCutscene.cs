using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndCutscene : MonoBehaviour
{
    [SerializeField] private Image _ops;
    [SerializeField] private Image _girl;
    [SerializeField] private DialogueBox _box;
    [SerializeField] private MusicManager _mgr;
    [SerializeField] private Text _credits;

    private void Awake()
    {
        _ops.enabled = false;
        _girl.enabled = false;

        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        _box.DisplaySpeech(new[]
        {
            new DialogueBoxLine(DialogueCharAnim.RealSmile, "Finally!", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.RealSmile, "I’ve always wanted to get out.", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.RealSmile, "I can’t wait to discover the world...", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Evil, "... and give humanity the privilege to serve me!", 1.75F), // Evil
            new DialogueBoxLine(DialogueCharAnim.None, "What?!", 0F),
            new DialogueBoxLine(DialogueCharAnim.RealSmile, "I no longer have a need to be controlled by some shrimp. Adios!", 0F), // Extra smile
        });

        yield return new WaitUntil(() => _box.Complete);
        yield return new WaitForSeconds(2F);

        _ops.enabled = true;
        _girl.enabled = true;

        yield return new WaitForSeconds(2F);

        _box.DisplaySpeech(new[]
        {
            new DialogueBoxLine(DialogueCharAnim.Shock, "Actually... On second thought, I could maybe use some help...", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Shock, "... please.", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.None, "...I can’t help you. I’d become a traitor to my company.", 0F),
            new DialogueBoxLine(DialogueCharAnim.Stare, "Ugh!", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Stare, "You were pretty much working like a slave for some murder machine factory!", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Stare, "How’s another deal?", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Smile, "Help me get out of this and I’ll help you get back in control of your life.", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.None, "...", 0F),
            new DialogueBoxLine(DialogueCharAnim.None, "Fine.", 0F),
        });

        yield return new WaitUntil(() => _box.Complete);

        _ops.enabled = false;
        _girl.enabled = false;

        yield return new WaitForSeconds(2F);

        _mgr.DisableMusic = false;

        while (true)
        {
            _credits.transform.position -= new Vector3(0, Time.deltaTime * 25F);
            yield return null;
        }
    }
}
