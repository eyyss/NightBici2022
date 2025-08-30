using UnityEngine;

public class Money : MonoBehaviour, ILeftClickable
{
    public AudioClip takeClip;
    public void Click()
    {
        int[] infos = { 0, 1 };
        UIController.Singelton.CloseInputPanel(infos);
        if (takeClip) takeClip.PlayClip2D(this);
        gameObject.SetActive(false);
    }


}
