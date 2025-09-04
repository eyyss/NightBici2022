using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;
using UnityEngine.UI;

public class Clothings : MonoBehaviour, ILeftClickable
{
    public float clickCount;
    public Image clickProgress;
    public GameObject emptyClothings;
    private MeshRenderer meshRenderer;
    public AudioData clothesAudio;
    public Transform basket;
    private bool ilkClick;
    private Item item;
    void Awake()
    {
        item = GetComponent<Item>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public void Click()
    {
        if (!ilkClick)
        {
            ilkClick = true;

            Vector3 pos = transform.position;
            pos.y = 0.48f;
            basket.transform.position = pos;
            basket.transform.eulerAngles = Vector3.zero;
        }

        if (clickCount % 0.5f == 0)
            clothesAudio.Play3D(this, transform.position);


        clickCount += 0.1f;
        clickProgress.fillAmount = clickCount;
        if (clickCount >= 1 && clickProgress.gameObject.activeSelf)
        {
            House.Singelton.CollectClothing();

            clickProgress.gameObject.SetActive(false);
            meshRenderer.enabled = false;
            emptyClothings.SetActive(true);

            Vector3 basketFillScale = basket.GetChild(0).localScale;
            basketFillScale.y += 0.8f;
            basket.GetChild(0).localScale = basketFillScale;
            Destroy(item);
            Destroy(this);
        }
    }


}
