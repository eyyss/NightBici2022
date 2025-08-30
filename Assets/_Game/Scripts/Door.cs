using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour, ILeftClickable
{
    public Vector3 openRot;
    private Vector3 startRot;
    public float doorSpeed = 6f;
    public float returnDoorSpeed = 6f;
    private bool open;
    public AudioClip doorOpenClip;
    public AudioClip doorCloseClip;
    private AudioSource source;


    private void Awake()
    {
        source = GetComponent<AudioSource>();
        startRot = transform.localRotation.eulerAngles;
    }
    private void Update()
    {
        if (open)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(openRot), Time.deltaTime * doorSpeed);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(startRot), Time.deltaTime * returnDoorSpeed);
        }
    }


    public void Click()
    {

        open = !open;

        if (!doorOpenClip || !doorCloseClip) return;
        if (source)
        {
            source.clip = open ? doorOpenClip : doorCloseClip;
            source.Play();
        }
    }
}
