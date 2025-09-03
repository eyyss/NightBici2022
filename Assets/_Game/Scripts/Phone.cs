using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EasyPeasyFirstPersonController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    public GameObject phonePanel;
    public TextMeshProUGUI numberText;
    private bool canOpen = true;
    public AudioData phoneCallingAudio;
    public AudioData numberButtonAudio;
    private FirstPersonController fpsController;
    void Start()
    {
        fpsController = FirstPersonController.Singelton;
        phonePanel.SetActive(false);
    }
    public void NumberButtonOnClick(int index)
    {
        if (numberText.text.Length >= 8) return;
        numberButtonAudio.Play2D(this);
        numberText.text += index;
    }
    public void Call()
    {
        if (numberText.text != "5554213")
        {
            numberText.text = "<color=red>Unknown</color>";
            Invoke(nameof(ResetNumberText), 1f);
            return;
        }
        //EventManager.Singelton.InvokeEvent("PhoneCall" + numberText.text);
        ClosePhone();
        phoneCallingAudio.Play2D(this);
        canOpen = false;
        Invoke(nameof(CookDialogue), 6f);
    }
    private void CookDialogue()
    {
        DialogController.Singelton.StartDialog(2);
    }
    private void ResetNumberText()
    {
        numberText.text = string.Empty;
    }
    public void DeleteLastNumber()
    {
        if (numberText.text.Length <= 0) return;
        var newString = numberText.text.Substring(0, numberText.text.Length - 1);
        numberText.text = newString;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && canOpen)
        {
            if (!phonePanel.activeSelf && !fpsController.blockGlobal)
            {
                OpenPhone();
                return;
            }
            if (phonePanel.activeSelf)
            {
                ClosePhone();
            }
        }
    }
    public void OpenPhone()
    {
        phonePanel.SetActive(true);
        fpsController.SetControl(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ClosePhone()
    {
        phonePanel.SetActive(false);
        fpsController.SetControl(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
