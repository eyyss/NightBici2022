using System.Collections;
using System.Collections.Generic;
using EasyPeasyFirstPersonController;
using UnityEngine;

public class WashingStand : MonoBehaviour, ILeftClickable
{
    public AudioData washingHandAudio;
    private bool canWash = true;
    public void Click()
    {
        if (!canWash) return;

        canWash = false;
        FirstPersonController.Singelton.SetControl(false);
        UIController.Singelton.FadeIn(1f, delegate
        {
            washingHandAudio.Play2D(this);
            Invoke(nameof(HandWashComplete), 2);
        });
    }
    private void HandWashComplete()
    {
        FirstPersonController.Singelton.SetControl(true);
        UIController.Singelton.FadeOut(1f);
        Invoke(nameof(HungryDialogue), 2f);
    }
    private void HungryDialogue()
    {
        DialogController.Singelton.StartDialog(1);
    }
}
