using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Donald : MonoBehaviour, ILeftClickable
{
    public bool headLookPlayer = false;
    private Animator animator;
    private Transform pizzaParent;
    public Item pizza;
    public PlayableDirector donaldEnterDirector, donaldExitDirector;

    void Start()
    {
        animator = GetComponent<Animator>();
        pizzaParent = pizza.transform.parent;
    }
    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;
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
    public void Click()
    {
        headLookPlayer = true;
        DialogController.Singelton.StartDialog(7);
        if (TryGetComponent(out Collider coll))
        {
            coll.enabled = false;
        }
        StartCoroutine(WaitForPizzaTaken());
    }

    public void SetTakeablePizza()
    {
        if (pizza.TryGetComponent(out Collider coll))
        {
            coll.enabled = true;
        }
    }

    private IEnumerator WaitForPizzaTaken()
    {
        yield return new WaitUntil(() => pizza.transform.parent != pizzaParent);
        headLookPlayer = false;
        donaldEnterDirector.gameObject.SetActive(false);
        donaldExitDirector.gameObject.SetActive(true);

        Debug.Log("Oyuncu pizzayı aldı!");
    }


}
