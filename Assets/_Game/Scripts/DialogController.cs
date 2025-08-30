using System;
using System.Collections;
using System.Collections.Generic;
using EasyPeasyFirstPersonController;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class DialogController : MonoBehaviour
{
    [Serializable]
    public class DialogDoneEvent : UnityEvent<int, int, bool> { }

    [Serializable]
    public class CharacterInfo
    {
        public string characterName;
        public Color characterTextColor;
    }

    [Serializable]
    public class DialogObject
    {
        public bool playOnRandom;
        private bool dialogShown;
        public List<DialogTextObject> dialogsChildTexts = new List<DialogTextObject>();

        public void SetDialogShown(bool value) => dialogShown = value;
        public bool GetDialogShown() => dialogShown;
    }

    [Serializable]
    public class DialogTextObject
    {
        public bool mainText;
        public LocalizedString localizedDialogText;
        public int characterID;
    }

    public static DialogController Singelton;

    public AudioData dialogPopAudio;
    public static bool blockSkipDialog;

    public TextMeshProUGUI dialogTextNormal;
    public float letterShowSpeed = 0.002f;

    public DialogDoneEvent dialogDoneEvent;
    public CharacterInfo[] charactersInfo;
    public List<DialogObject> allDialogs = new List<DialogObject>();

    private Coroutine tempCor;
    private bool dialogActive;
    private bool skipFirstTime;
    private int childTextCounter;
    private int currentDialog = -1;

    private void Awake()
    {
        Singelton = this;
    }

    public void StartDialog(int index, bool onlyLastText = false, bool setSkipFirstTimeTrue = true)
    {
        if (FirstPersonController.Singelton != null)
        {
            FirstPersonController.Singelton.SetControl(false);
            FirstPersonController.Singelton.Focus(true);
        }

        childTextCounter = onlyLastText ? allDialogs[index].dialogsChildTexts.Count - 1 : 0;
        currentDialog = index;

        if (allDialogs[index].GetDialogShown() && !allDialogs[index].dialogsChildTexts[childTextCounter].mainText)
        {
            childTextCounter = GetCurrentDialogFirstMainText(index);
        }

        skipFirstTime = setSkipFirstTimeTrue;
        dialogDoneEvent.Invoke(currentDialog, childTextCounter, false);
        ShowDialog(index);
    }

    private int GetCurrentDialogFirstMainText(int index)
    {
        for (int i = 0; i < allDialogs[index].dialogsChildTexts.Count; i++)
        {
            if (allDialogs[index].dialogsChildTexts[i].mainText)
                return i;
        }
        return 0;
    }

    private void ShowDialog(int index)
    {
        if (allDialogs.Count > index)
        {
            if (tempCor != null)
                StopCoroutine(tempCor);

            tempCor = StartCoroutine(LoadLocalizedAndShow(index));
        }
    }

    private IEnumerator LoadLocalizedAndShow(int index)
    {
        LocalizedString locString;

        if (allDialogs[index].playOnRandom)
        {
            var randomDialog = allDialogs[index].dialogsChildTexts[UnityEngine.Random.Range(0, allDialogs[index].dialogsChildTexts.Count)];
            locString = randomDialog.localizedDialogText;
        }
        else
        {
            locString = allDialogs[index].dialogsChildTexts[childTextCounter].localizedDialogText;
        }

        var localizedOp = locString.GetLocalizedStringAsync();
        yield return localizedOp;

        string result = localizedOp.Result;
        tempCor = StartCoroutine(showDialogLettersOneByOne(result, index));
    }

    private IEnumerator showDialogLettersOneByOne(string textToShow, int index)
    {
        yield return new WaitForEndOfFrame();
        dialogActive = true;

        string originalText = "";
        string hiddenText = textToShow;
        char[] characters = textToShow.ToCharArray();

        Color textColor = charactersInfo[Mathf.Clamp(allDialogs[index].dialogsChildTexts[childTextCounter].characterID, 0, charactersInfo.Length - 1)].characterTextColor;

        for (int i = 0; i < characters.Length; i++)
        {

            dialogPopAudio.Play2D(this);

            originalText += characters[i];
            if (hiddenText.Length > 0)
                hiddenText = hiddenText.Remove(0, 1);

            dialogTextNormal.text = $"<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{originalText}</color><color=#00000000>{hiddenText}</color>";
            dialogTextNormal.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(letterShowSpeed);
        }

        tempCor = null;
    }

    private bool isLastChildText(int index)
    {
        return allDialogs[index].playOnRandom || allDialogs[index].dialogsChildTexts.Count - 1 == childTextCounter;
    }

    private void StopDialog()
    {
        if (!dialogActive) return;

        if (!isLastChildText(currentDialog))
        {
            childTextCounter++;
            if (allDialogs[currentDialog].GetDialogShown() && !allDialogs[currentDialog].dialogsChildTexts[childTextCounter].mainText)
            {
                StopDialog();
                return;
            }

            ShowDialog(currentDialog);
            dialogDoneEvent.Invoke(currentDialog, childTextCounter, false);
            return;
        }

        allDialogs[currentDialog].SetDialogShown(true);
        dialogTextNormal.gameObject.SetActive(false);

        if (tempCor != null)
            StopCoroutine(tempCor);

        StartCoroutine(waitForFrameAndUnblock());
        dialogDoneEvent.Invoke(currentDialog, childTextCounter, true);
        dialogActive = false;
    }

    private IEnumerator waitForFrameAndUnblock()
    {
        yield return new WaitForEndOfFrame();
        if (FirstPersonController.Singelton != null)
        {
            FirstPersonController.Singelton.Focus(false);
            FirstPersonController.Singelton.SetControl(true);
        }
    }

    private void Update()
    {
        if (!blockSkipDialog && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape)))
        {
            if (skipFirstTime)
                skipFirstTime = false;
            else
                StopDialog();
        }
    }
}
