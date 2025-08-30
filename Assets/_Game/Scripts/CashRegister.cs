using TMPro;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    public static CashRegister Singelton;
    public TextMeshProUGUI ordersText;
    public TextMeshProUGUI totalText;
    void Awake()
    {
        Singelton = this;
    }
    public void SetOrders(string orders, string price)
    {
        ordersText.text = orders;
        totalText.text = "TOTAL: " + price;
    }
}
