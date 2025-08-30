
using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Singelton;
    private Camera cam;
    public Image crosshairIn;
    public TextMeshProUGUI interactItemNameText;
    public GameObject interactItemNameTextParent;
    public GameObject[] inputInfos;
    void Awake()
    {
        cam = Camera.main;
        Singelton = this;
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

}