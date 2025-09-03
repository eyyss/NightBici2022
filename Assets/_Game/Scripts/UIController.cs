
using System;
using System.Collections;
using DG.Tweening;
using EasyPeasyFirstPersonController;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Singelton;
    private Camera cam;
    public Image crosshairIn;
    public TextMeshProUGUI interactItemNameText;
    public GameObject interactItemNameTextParent;
    public GameObject[] inputInfos;
    public Image fadeImage;
    public TextMeshProUGUI currentDayText;
    public TextMeshProUGUI infoText;

    void Awake()
    {
        cam = Camera.main;
        Singelton = this;
    }
    void Start()
    {
        ResetInfo();
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            FirstPersonController.Singelton.SetControl(false);
            currentDayText.text = "Day 1";
            currentDayText.DOColor(Color.clear, 2f).SetDelay(2f);
            FadeOut(2f, delegate
            {
                FirstPersonController.Singelton.SetControl(true);
            });
        }
    }
    void Update()
    {
        bool active = false;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out RaycastHit hit, InteractionController.PickupRange))
        {
            if (hit.collider.TryGetComponent(out Rigidbody rb)
            || hit.collider.TryGetComponent(out ILeftClickable leftClickable))
            {
                active = true;
            }

            if (hit.collider.TryGetComponent(out Item item))
            {
                if (item.rb != null && !item.rb.useGravity)
                {
                    interactItemNameText.text = string.Empty;
                    interactItemNameTextParent.SetActive(false);
                }
                else
                {
                    interactItemNameText.text = item.ItemName;
                    interactItemNameTextParent.SetActive(true);
                }
            }
            else
            {
                interactItemNameText.text = string.Empty;
                interactItemNameTextParent.SetActive(false);
            }
        }
        else
        {
            interactItemNameText.text = string.Empty;
            interactItemNameTextParent.SetActive(false);
            active = false;
        }

        if (active)
        {
            crosshairIn.transform.localScale = Vector3.Lerp(crosshairIn.transform.localScale, Vector3.one * 0.6f, Time.deltaTime * 15f);
        }
        else
        {
            crosshairIn.transform.localScale = Vector3.Lerp(crosshairIn.transform.localScale, Vector3.zero, Time.deltaTime * 15f);
        }
    }
    public void OpenInputPanel(int[] infos)
    {
        foreach (var item in infos)
        {
            inputInfos[item].SetActive(true);
        }
    }
    public void CloseInputPanel(int[] infos)
    {
        foreach (var item in infos)
        {
            inputInfos[item].SetActive(false);
        }
    }

    public void FadeIn(float duration, Action onComplete = null)
    {
        if (fadeImage == null) return;
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(0f, 1f, duration, onComplete));
    }

    // Fade Out
    public void FadeOut(float duration, Action onComplete = null)
    {
        if (fadeImage == null) return;
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeRoutine(1f, 0f, duration, () =>
        {
            fadeImage.gameObject.SetActive(false);
            onComplete?.Invoke();
        }));
    }
    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration, Action onComplete)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        // Ensure final alpha
        color.a = endAlpha;
        fadeImage.color = color;

        onComplete?.Invoke();
    }
    public void ShowInfo(string info, float duration = 3f)
    {
        infoText.text = info;
        Invoke(nameof(ResetInfo), duration);
    }
    private void ResetInfo()
    {
        infoText.text = string.Empty;
    }

}