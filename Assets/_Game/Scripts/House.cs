using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(StartDialgue), 1f);
    }
    public void StartDialgue()
    {
        DialogController.Singelton.StartDialog(0);
    }
    public void ShowPhoneInfo()
    {
        UIController.Singelton.ShowInfo("(!) Telefonla arama yapmak için P tuşunu kullanın", 5);
    }
}
