using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EasyPeasyFirstPersonController;
using UnityEngine;

public class PizzaBoxPreview : MonoBehaviour
{
    public Transform sitPoint;
    public Transform kalkisPoint;
    public List<GameObject> previewObjs;
    public ParticleSystem smoke;
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Item item) && item.ItemName == "Pizza Box")
        {
            foreach (var i in previewObjs)
            {
                i.SetActive(false);
            }
            Destroy(item.rb);
            if (TryGetComponent(out Collider thisCollider))
            {
                thisCollider.enabled = false;
            }
            if (item.TryGetComponent(out Pizza pizza))
            {
                pizza.SetCanEat(true);
            }
            item.transform.position = transform.position;
            item.transform.eulerAngles = new Vector3(0, 90, 0);
            item.transform.GetChild(1).DOLocalRotate(new Vector3(-300f, 0f, 0f), 1f).SetDelay(2);
            Destroy(item);
            //gameObject.SetActive(false);
            Invoke(nameof(EatPizzaState), 0.5f);

        }
    }
    private void EatPizzaState()
    {
        FirstPersonController.Singelton.SetMove(false);
        UIController.Singelton.FadeIn(1f, OnPlayerSit);
    }
    private void OnPlayerSit()
    {
        smoke.Play();
        FirstPersonController.Singelton.transform.position = sitPoint.position;
        UIController.Singelton.FadeOut(1f);
    }
    public void PlayerMasadanKalk()
    {
        UIController.Singelton.FadeIn(1f, delegate
        {
            FirstPersonController.Singelton.transform.position = kalkisPoint.position;
            UIController.Singelton.FadeOut(1f, delegate
            {
                FirstPersonController.Singelton.SetMove(true);
            });
        });
    }

}
