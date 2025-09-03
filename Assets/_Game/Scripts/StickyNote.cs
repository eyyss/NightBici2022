using System.Collections;
using System.Collections.Generic;
using EasyPeasyFirstPersonController;
using UnityEngine;

public class StickyNote : MonoBehaviour, ILeftClickable
{
    public int dialogueIndex = 0;
    public AudioData noteAudio;

    public void Click()
    {
        noteAudio.Play2D(this);
        DialogController.Singelton.StartDialog(dialogueIndex);
    }
}
