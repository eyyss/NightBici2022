using System.Collections.Generic;
using DG.Tweening;
using EasyPeasyFirstPersonController;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour, ILeftClickable
{
    public CustomerIdentitiy customerIdentitiy;
    public List<GameObject> characterVisuals;
    private NavMeshAgent agent;
    private Animator animator;
    private CapsuleCollider capsuleCollider;
    public Transform destination;
    public CustomerDialogues[] dialogues;
    private bool firstDialogueTrigged;
    public List<Order> dayOrders; // farklı günlerdeki siparişler

    public List<Item> ownedItems;

    private bool ordersReady;

    private bool headLookPlayer = true;


    private void OrderSendCashRegister()
    {
        Order currentOrder = dayOrders[CustomerManager.Singelton.currentDay];
        if (currentOrder == null) return;

        string orderText = "";

        // Bici
        if (currentOrder.bici.count > 0)
        {
            orderText += $"x{currentOrder.bici.count} Bici\n";

            if (currentOrder.bici.bananaCount > 0)
                orderText += $"   x{currentOrder.bici.bananaCount} Banana\n";

            if (currentOrder.bici.watermelonCount > 0)
                orderText += $"   x{currentOrder.bici.watermelonCount} Watermelon\n";
        }

        // Diğerleri
        if (currentOrder.cokeCount > 0)
            orderText += $"x{currentOrder.cokeCount} Coke\n";

        if (currentOrder.orangeCokeCount > 0)
            orderText += $"x{currentOrder.orangeCokeCount} Orange Coke\n";

        if (currentOrder.sodaCount > 0)
            orderText += $"x{currentOrder.sodaCount} Soda\n";


        CashRegister.Singelton.SetOrders(orderText, currentOrder.price);
    }


    void OnAnimatorIK(int layerIndex)
    {
        var cam = Camera.main;
        if (headLookPlayer)
        {
            animator.SetLookAtWeight(1f);
            animator.SetLookAtPosition(cam.transform.position);
        }
        else
        {
            animator.SetLookAtWeight(0f);
        }
    }
    private void GetCurrentIdentity()
    {
        int current = (int)customerIdentitiy;
        foreach (var item in characterVisuals)
        {
            item.SetActive(false);
        }
        characterVisuals[current].SetActive(true);
    }
    void Start()
    {
        GetCurrentIdentity();
        DialogController.Singelton.dialogDoneEvent.AddListener(DialogueDoneEvent);
    }
    private void DialogueDoneEvent(int doneDialogueIndex, int doneChildDialogueIndex, bool lastText)
    {
        //Debug.Log("doneDialogueIndex: " + doneDialogueIndex);
        //Debug.Log("doneChildDialogueIndex: " + doneChildDialogueIndex);
        //Debug.Log("lastText: " + lastText);

        if (lastText)
        {
            if (dialogues[CustomerManager.Singelton.currentDay].dialogueIndexs[1] == doneDialogueIndex)
            {
                headLookPlayer = false;
                ChangeDestination(CustomerManager.Singelton.customerExitTransform);
                DOVirtual.DelayedCall(6, delegate { gameObject.SetActive(false); });
                CustomerManager.Singelton.SpawnNextCustomer(6f);
                CashRegister.Singelton.SetOrders(null, null);
            }
            string eventName = "dialog" + doneDialogueIndex;
            EventManager.Singelton.InvokeEvent(eventName);
        }
    }




    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    void Update()
    {
        if (destination != null)
        {
            agent.SetDestination(destination.position);
            if (Vector3.Distance(transform.position, destination.position) < 0.1f)
            {
                var targetRot = Quaternion.LookRotation(destination.forward);
                targetRot.x = 0;
                targetRot.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 3);
            }
        }

        animator.SetFloat("Velocity", agent.velocity.magnitude);

        if (Vector3.Distance(transform.position, CustomerManager.Singelton.customerOrderTransform.position) < 1)
            capsuleCollider.enabled = true;
        else capsuleCollider.enabled = false;


    }
    public void ChangeDestination(Transform newDestination)
    {
        destination = newDestination;
    }
    public void Click()
    {
        if (!firstDialogueTrigged)
        {
            firstDialogueTrigged = true;
            OrderSendCashRegister();
            DialogController.Singelton.StartDialog(dialogues[CustomerManager.Singelton.currentDay].dialogueIndexs[0]);
        }
    }


    public void CheckOrder(Collider other)
    {
        if (!other.TryGetComponent(out Item item) || !firstDialogueTrigged) return;

        //other.gameObject.SetActive(false);


        ownedItems.Add(item);

        Order currentOrder = dayOrders[CustomerManager.Singelton.currentDay];

        bool orderRequirements = CheckOrderRequirements(currentOrder);
        Debug.Log("Sipariş Durumu: " + orderRequirements);

        if (orderRequirements)
        {

            foreach (var itm in ownedItems)
            {
                if (itm == null) continue;

                var rb = itm.GetComponent<Rigidbody>();
                var col = itm.GetComponent<Collider>();

                if (rb != null) rb.isKinematic = false;
                if (col != null) col.enabled = false;

                itm.transform.DOMove(transform.position + Vector3.up * 1.5f, 0.5f).SetLink(gameObject);
                itm.transform.DOScale(Vector3.zero, 0.5f).SetLink(gameObject)
                    .OnComplete(() => itm.gameObject.SetActive(false));
            }



            DialogController.Singelton.StartDialog(dialogues[CustomerManager.Singelton.currentDay].dialogueIndexs[1]);
            FirstPersonController.Singelton.Look(transform.position + Vector3.up * 1.5f, 0.4f);
            EventManager.Singelton.InvokeEvent("CustomerPayout");
            CustomerManager.Singelton.currentCustomer = null;
        }


    }
    private bool CheckOrderRequirements(Order order)
    {
        int validBiciCount = 0;
        int validBananaBiciCount = 0;
        int validWatermelonBiciCount = 0;
        int validCokeCount = 0;
        int validOrangeCokeCount = 0;
        int validSodaCount = 0;

        foreach (var item in ownedItems)
        {
            if (item.ItemName == "Bowl")
            {
                Bowl bowl = item.GetComponent<Bowl>();
                if (bowl != null && bowl.iced && bowl.rosed)
                {
                    validBiciCount++;
                    if (bowl.bananad)
                    {
                        validBananaBiciCount++;
                    }
                    if (bowl.watermelond)
                    {
                        validWatermelonBiciCount++;
                    }
                }
            }
            else if (item.ItemName == "Coke")
            {
                validCokeCount++;
            }
            else if (item.ItemName == "Orange Coke")
            {
                validOrangeCokeCount++;
            }
            else if (item.ItemName == "Soda")
            {
                validSodaCount++;
            }
        }

        // Koşullar:
        // 1. Bici için: count, bananaCount, watermelonCount
        bool isBiciRequirementMet = order.bici.count == 0 || validBiciCount >= order.bici.count;
        bool isBananaRequirementMet = order.bici.bananaCount == 0 || validBananaBiciCount >= order.bici.bananaCount;
        bool isWatermelonRequirementMet = order.bici.watermelonCount == 0 || validWatermelonBiciCount >= order.bici.watermelonCount;

        // 2. Coke, Orange Coke, Soda için
        bool isCokeRequirementMet = order.cokeCount == 0 || validCokeCount >= order.cokeCount;
        bool isOrangeCokeRequirementMet = order.orangeCokeCount == 0 || validOrangeCokeCount >= order.orangeCokeCount;
        bool isSodaRequirementMet = order.sodaCount == 0 || validSodaCount >= order.sodaCount;

        return isBiciRequirementMet && isBananaRequirementMet && isWatermelonRequirementMet &&
               isCokeRequirementMet && isOrangeCokeRequirementMet && isSodaRequirementMet;
    }

    public void OpenMoney()
    {
        Order currentOrder = dayOrders[CustomerManager.Singelton.currentDay];
        GameObject money = System.Array.Find(CustomerManager.Singelton.moneys, p => p.name == currentOrder.price);
        money.SetActive(true);
    }

}