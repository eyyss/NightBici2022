using System.Collections;
using System.Collections.Generic;
using EasyPeasyFirstPersonController;
using UnityEngine;

public class Pizza : MonoBehaviour, ILeftClickable
{
    public bool canEat = false;
    public List<GameObject> pizzaSlices;
    private float eatTimer;
    public float eatTime = 4f;
    private Collider coll;
    public AudioData eatPizzaAudio;
    void Awake()
    {
        coll = GetComponent<Collider>();
    }
    void Update()
    {
        if (canEat)
        {
            eatTimer += Time.deltaTime;
            if (eatTimer >= eatTime)
            {
                coll.enabled = true;
            }
            else
            {
                coll.enabled = false;
            }
        }
    }
    public void SetCanEat(bool state)
    {
        canEat = state;
    }
    public void Click()
    {
        if (FirstPersonController.Singelton.blockGlobal) return;
        if (!canEat) return;
        if (pizzaSlices.Count <= 0) return;

        if (eatTimer >= eatTime)
        {
            eatTimer = 0; GameObject slice = pizzaSlices[pizzaSlices.Count - 1];
            if (slice != null)
            {
                eatPizzaAudio.Play2D(this);
                slice.SetActive(false);
                pizzaSlices.Remove(slice);
                if (pizzaSlices.Count == 3)
                {
                    EventManager.Singelton.InvokeEvent("PizzaWhenThreeSlicesLeft");
                    canEat = false;
                }
                if (pizzaSlices.Count <= 0)
                {
                    EventManager.Singelton.InvokeEvent("PlayerEatPizza");
                }
            }
        }
    }
}