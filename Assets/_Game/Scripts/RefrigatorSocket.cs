using UnityEngine;

public class RefrigatorSocket : MonoBehaviour, ILeftClickable
{
    public Animator cableAnimator;
    public void Click()
    {
        cableAnimator.SetBool("isOpen", !cableAnimator.GetBool("isOpen"));
    }

}
