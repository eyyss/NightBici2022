using System.Collections.Generic;
using UnityEngine;

public class RefrigatorSocket : MonoBehaviour, ILeftClickable
{
    public Animator cableAnimator;
    public List<Light> refrigatorLights;
    public void Click()
    {
        bool isOpen = cableAnimator.GetBool("isOpen");
        cableAnimator.SetBool("isOpen", !isOpen);
        if (isOpen)
        {
            Invoke(nameof(RefrigatorLightClose), 0.4f);
        }
        else
        {
            Invoke(nameof(RefrigatorLightOpen), 0.4f);
        }
    }
    private void RefrigatorLightOpen()
    {
        foreach (var item in refrigatorLights)
        {
            item.gameObject.SetActive(true);
        }
    }
    private void RefrigatorLightClose()
    {
        foreach (var item in refrigatorLights)
        {
            item.gameObject.SetActive(false);
        }
    }


}
