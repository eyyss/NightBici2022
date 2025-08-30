using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiciMachine : MonoBehaviour
{
    public Transform target;
    private Bowl currentBowl;
    public Animator biciMachineAnimator;
    public AudioData packingAudio;
    public Renderer biciMachineButton;
    public Color buttonNormalColor, buttonActiveColor;

    void OnTriggerEnter(Collider other)
    {
        if (currentBowl != null) return;
        if (other.attachedRigidbody.useGravity && other.TryGetComponent(out Bowl bowl) && !bowl.lid.activeSelf)
        {
            currentBowl = bowl;
            currentBowl.coll.enabled = false;
            currentBowl.rb.useGravity = false;
            currentBowl.rb.isKinematic = true;
            currentBowl.transform.rotation = target.rotation;
            currentBowl.transform.position = target.position;
            Invoke(nameof(Bicile), 0.5f);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Bowl bowl))
        {
            currentBowl = null;
        }
    }
    private void Bicile()
    {
        biciMachineButton.material.color = buttonActiveColor;
        biciMachineButton.material.SetColor("_EmissionColor", buttonActiveColor);
        biciMachineButton.material.EnableKeyword("_EMISSION");


        biciMachineAnimator.SetTrigger("Bicile");
        Invoke(nameof(Packing), 1.8f);
    }
    private void Packing()
    {
        currentBowl.lid.SetActive(true);
        packingAudio.Play2D(this);
        Invoke(nameof(BicilemeyiTamamla), 2f);
    }
    private void BicilemeyiTamamla()
    {
        biciMachineButton.material.color = buttonNormalColor;
        biciMachineButton.material.SetColor("_EmissionColor", buttonNormalColor);
        biciMachineButton.material.EnableKeyword("_EMISSION");

        currentBowl.coll.enabled = true;
        currentBowl.rb.useGravity = true;
        currentBowl.rb.isKinematic = false;
        //currentBowl = null;
    }
}
