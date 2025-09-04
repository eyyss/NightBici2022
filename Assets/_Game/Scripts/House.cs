using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public static House Singelton;
    public int collectedClothing;
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
        }
    }
    public void ShowLaundryGoRoomInfo()
    {
        UIController.Singelton.ShowInfo("(!) Çamaşır sepetini al ve odana götür", 5);
    }
}
