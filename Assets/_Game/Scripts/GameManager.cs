
using DG.Tweening;
using EasyPeasyFirstPersonController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool startFadeIn = true;
    public Image transitionImage;
    public TextMeshProUGUI currentDayText;
    void Start()
    {
        if (startFadeIn)
        {
            FirstPersonController.Singelton.SetControl(false);
            currentDayText.text = "Day 1";
            transitionImage.color = Color.black;
            currentDayText.DOColor(Color.clear, 2f).SetDelay(2f);
            transitionImage.DOColor(Color.clear, 2f).SetDelay(5f).OnComplete(delegate
            {
                FirstPersonController.Singelton.SetControl(true);
            });
        }
    }
}
