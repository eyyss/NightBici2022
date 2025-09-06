using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketPreview : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Item item) && item.ItemName == "Basket" && item.rb.useGravity)
        {
            EventManager.Singelton.InvokeEvent("BasketPlacedInRoom");
            gameObject.SetActive(false);
            item.gameObject.SetActive(false);
            item.transform.position = transform.position;
            item.transform.rotation = Quaternion.identity;
            item.gameObject.SetActive(true);
            item.rb.velocity = Vector3.zero;
            item.rb.angularVelocity = Vector3.zero;
        }
    }
}
