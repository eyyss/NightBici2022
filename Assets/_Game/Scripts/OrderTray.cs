using System.Collections.Generic;
using UnityEngine;

public class OrderTray : MonoBehaviour
{
    public static OrderTray Singelton;

    public List<Collider> items;
    void Awake()
    {
        Singelton = this;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Bowl bowl) && !bowl.lid.activeSelf) return;// kase ise ve kapatilmamissa sayma

        if (!items.Contains(other))
            items.Add(other);
    }
    void OnTriggerExit(Collider other)
    {
        if (items.Contains(other))
            items.Remove(other);
    }
    public void CheckOrder()
    {
        if (CustomerManager.Singelton == null) return;
        foreach (var item in items)
        {
            if (CustomerManager.Singelton.currentCustomer != null && item != null)
                CustomerManager.Singelton.currentCustomer.CheckOrder(item);
        }
        items.Clear();
    }
}
