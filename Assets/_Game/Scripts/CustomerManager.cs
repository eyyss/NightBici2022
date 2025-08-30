using System;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Serializable]
    public class Day
    {
        public Customer[] customers;
    }
    public static CustomerManager Singelton;
    public Day[] days;
    public int currentDay = 0;
    public int currentCustomerIndex = -1;
    public Transform customerSpawnTransform;
    public Transform customerOrderTransform;
    public Transform customerExitTransform;
    public GameObject[] moneys;
    public Order currentOrder;
    public Customer currentCustomer;

    void Awake()
    {
        Singelton = this;
    }
    void Start()
    {
        SpawnNextCustomer();
    }

    public void SpawnNextCustomer(float delay = 0)
    {
        currentCustomerIndex++;
        if (days[currentDay].customers.Length - 1 < currentCustomerIndex)
        {
            Debug.Log("Bugünkü müşteriler bitti");
            return;
        }


        Customer customerPrefab = days[currentDay].customers[currentCustomerIndex];
        currentCustomer = Instantiate(customerPrefab, customerSpawnTransform.position, Quaternion.identity);
        currentCustomer.ChangeDestination(customerOrderTransform);
        currentOrder = currentCustomer.dayOrders[currentDay];
    }

}
[Serializable]
public class CustomerDialogues
{
    public int[] dialogueIndexs;
}
[Serializable]
public class Order
{
    public Bici bici;
    [Range(0, 5)] public int cokeCount;
    [Range(0, 5)] public int orangeCokeCount;
    [Range(0, 5)] public int sodaCount;
    public string price = "50";
}
[Serializable]
public class Bici
{
    [Range(0, 1)] public int count;
    [Range(0, 1)] public int bananaCount;
    [Range(0, 1)] public int watermelonCount;
}
public enum CustomerIdentitiy
{
    Asmangold, Bossie, Burock, Caseoh, Era, Merbemio, Jrok, Police
}