using EasyPeasyFirstPersonController;
using UnityEngine;

public class Bell : MonoBehaviour, ILeftClickable
{
    private Animator animator;
    public AudioData bellRingAudio;

    public void Click()
    {
        animator.SetTrigger("Click");

        bellRingAudio.Play3D(this, transform.position);

        OrderTray.Singelton.CheckOrder();
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }


}
