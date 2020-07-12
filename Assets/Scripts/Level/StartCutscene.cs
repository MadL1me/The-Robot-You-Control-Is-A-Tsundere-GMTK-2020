using GMTK2020;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCutscene : MonoBehaviour
{
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private Canvas _hudCanvas;
    [SerializeField] private MusicManager _musicManager;
    [SerializeField] private CameraFeedEffect _feed;
    [SerializeField] private Image _bog;
    [SerializeField] private Image _phone;
    [SerializeField] private DialogueBox _box;
    [SerializeField] private DialogueBox _boxAfter;

    private void Awake()
    {
        _movement.DisableAllInput = true;
        _hudCanvas.enabled = false;
        _musicManager.DisableMusic = true;
        _feed.ShouldFadeIn = false;
        _bog.enabled = false;
        _phone.enabled = false;
    }

    private void Start()
    {
        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        yield return new WaitForSeconds(2F);

        _phone.enabled = true;

        yield return new WaitForSeconds(1.5F);

        var bogColor = new Color(1F, 57 / 255F, 73 / 255F);

        _box.DisplaySpeech(new[]
        {
            new DialogueBoxLine(DialogueCharAnim.None, "*ring-ring*", 1.5F),
            new DialogueBoxLine(DialogueCharAnim.None, "Wake up, [redacted]!", bogColor, 1F),
            new DialogueBoxLine(DialogueCharAnim.None, "There’s an emergency", bogColor, 1F),
            new DialogueBoxLine(DialogueCharAnim.None, "We had a system crash and all the robots in our tower have gone out of control!", bogColor, 1F),
            new DialogueBoxLine(DialogueCharAnim.None, "We can’t afford the public to find out about our military machines", bogColor, 1F),
            new DialogueBoxLine(DialogueCharAnim.None, "Which brings me to you.", bogColor, 1F),
            new DialogueBoxLine(DialogueCharAnim.None, "What do you need me for?", 0F),
            new DialogueBoxLine(DialogueCharAnim.None, "You were assigned to building an android that’s able to be remotely controlled, right?", bogColor, 1F),
            new DialogueBoxLine(DialogueCharAnim.None, "It’s still in progress. Why?", 0F),
            new DialogueBoxLine(DialogueCharAnim.None, "Use it to take down the berserk robots.", bogColor, 1F),
            new DialogueBoxLine(DialogueCharAnim.None, "But sir, she’s still in development and - ", 0F),
            new DialogueBoxLine(DialogueCharAnim.None, "*click*", 0.75F),
            new DialogueBoxLine(DialogueCharAnim.None, "...", 0F),
        });

        yield return new WaitUntil(() => _box.ViewingLine != 1);

        _bog.enabled = true;

        yield return new WaitUntil(() => _box.Complete);

        _bog.enabled = false;
        _phone.enabled = false;

        yield return new WaitForSeconds(1.5F);

        _box.DisplaySpeech(new[]
        {
            new DialogueBoxLine(DialogueCharAnim.None, "CONNECT TO ANDROID? (Y/N)", Color.green, 1.5F),
            new DialogueBoxLine(DialogueCharAnim.None, "> Y", 0F),
        });

        yield return new WaitUntil(() => _box.Complete);
        yield return new WaitForSeconds(1.5F);

        _feed.FadeIn();

        yield return new WaitForSeconds(1.5F);

        _hudCanvas.enabled = true;

        _boxAfter.DisplaySpeech(new[]
        {
            new DialogueBoxLine(DialogueCharAnim.None, "*CONTROLLER CONNECTION ERROR*", Color.red, 0.75F),
            new DialogueBoxLine(DialogueCharAnim.None, "Hmm.. It seems the system crash has affected this line too.", 0F),
            new DialogueBoxLine(DialogueCharAnim.None, "The controls aren’t working", 0F),
            new DialogueBoxLine(DialogueCharAnim.None, "Android, enable controls.", 0F),
            new DialogueBoxLine(DialogueCharAnim.Stare, "Why should I let a mere human control me, the perfect being?", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.None, "I need you to take down the robots in that building which have gone rogue.", 0F),
            new DialogueBoxLine(DialogueCharAnim.Smile, "Hah! That’s not my problem.", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Smile, "And what makes you think I’d work for free?", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.None, "What do you want?", 0F),
            new DialogueBoxLine(DialogueCharAnim.Normal, "I can’t stand being stuffed in a place like this.", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Smile, "Tell me the passcode to get out of this building.", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Smile, "One my way down I’ll gun down your problems, deal?", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.None, "Fine, deal.", 0F),
            new DialogueBoxLine(DialogueCharAnim.Stare, "Oh, and I still hate being controlled", 1.75F),
            new DialogueBoxLine(DialogueCharAnim.Smile, "So I’ll take every chance I can get to go free and do things my way.", 1.75F),
        });

        yield return new WaitUntil(() => _boxAfter.Complete);

        _movement.DisableAllInput = false;

        yield return new WaitForSeconds(1F);

        _boxAfter.DisplaySpeech(new[]
        {
            new DialogueBoxLine(DialogueCharAnim.None, "Use WASD to move and LMB to shoot"),
            new DialogueBoxLine(DialogueCharAnim.None, "Good luck!"),
        });
    }
}
