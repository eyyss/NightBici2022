using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public static House Singelton;
    public int collectedClothing;
    public GameObject previewBasket;
    public AudioSource inComingCallAudioSource;
    void Awake()
    {
        Singelton = this;
    }
    void Start()
    {
        Invoke(nameof(StartDialgue), 1f);
    }
    private void StartDialgue()
    {
        DialogController.Singelton.StartDialog(0);
    }
    public void ShowPhoneInfo()
    {
        UIController.Singelton.ShowInfo("(!) Telefonla arama yapmak için P tuşunu kullanın", 5);
    }
    public void StartLaundryHangingDialogue()
    {
        DialogController.Singelton.StartDialog(6);
    }
    public void ShowLaundryHangingInfo()
    {
        UIController.Singelton.ShowInfo("(!) Çamaşırlar arka bahçede, toplamak için arka bahçeye git", 5);
    }
    public void CollectClothing()
    {
        collectedClothing++;
        if (collectedClothing >= 3)
        {
            EventManager.Singelton.InvokeEvent("ClothingsCollected");
            previewBasket.SetActive(true);
        }
    }
    public void ShowLaundryGoRoomInfo()
    {
        UIController.Singelton.ShowInfo("(!) Çamaşır sepetini al ve odana götür", 5);
    }
    public void InComingCall()
    {
        inComingCallAudioSource.Play();
        Invoke(nameof(ShowInComingCallOpenInfo), 1f);
    }
    public void ShowInComingCallOpenInfo()
    {
        UIController.Singelton.ShowInfo("(!) Aramayı cevaplamak için Space tuşuna bas", 5);
        StartCoroutine(WaitFKey());
    }
    private IEnumerator WaitFKey()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        inComingCallAudioSource.Stop();
        DialogController.Singelton.StartDialog(8);
    }

}
