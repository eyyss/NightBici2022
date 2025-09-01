using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

public class Bowl : MonoBehaviour
{
    private Item thisItem;
    [System.NonSerialized] public Collider coll;
    [System.NonSerialized] public Rigidbody rb;
    public bool iced;
    public bool starched;
    public bool rosed;
    public bool bananad;
    public bool watermelond;

    [NonSerialized] public float roseValue;
    public Material biciMat;
    public GameObject bananaSlicesParent;
    public GameObject watermelonSlicesParent;
    public GameObject stachSlicesParent;
    public GameObject iceFilled;
    public GameObject lid;
    public AudioData placeAudio;
    void Awake()
    {
        thisItem = GetComponent<Item>();
        coll = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (lid.activeSelf) return;

        if (other.gameObject.TryGetComponent(out IceGrater iceGrater) && iceGrater.fill && other.rigidbody.useGravity && !iced)
        {
            iced = true;
            FillIce(iceGrater);
        }
        if (!bananad && iced && other.gameObject.TryGetComponent(out Item item) && item.ItemName == "Banana Slices" && other.rigidbody.useGravity)
        {
            placeAudio.Play2D(this);
            bananad = true;
            bananaSlicesParent.SetActive(true);
            Destroy(other.gameObject);
        }
        if (!watermelond && iced && other.gameObject.TryGetComponent(out item) && item.ItemName == "Watermelon Slices" && other.rigidbody.useGravity)
        {
            placeAudio.Play2D(this);
            watermelond = true;
            watermelonSlicesParent.SetActive(true);
            Destroy(other.gameObject);
        }
        if (!starched && iced && other.gameObject.TryGetComponent(out item) && item.ItemName == "Starch" && other.rigidbody.useGravity)
        {
            placeAudio.Play2D(this);
            starched = true;
            stachSlicesParent.SetActive(true);
            Destroy(other.gameObject);
        }

    }
    public void FillIce(IceGrater iceGrater)
    {
        rb.isKinematic = true;
        coll.enabled = false;
        Collider iceGraterCollider = iceGrater.GetComponent<Collider>();
        Rigidbody iceGraterRb = iceGrater.GetComponent<Rigidbody>();
        iceGraterCollider.enabled = false;
        iceGraterRb.isKinematic = true;

        iceGrater.transform.DOMove(transform.position + Vector3.up * 0.5f, 1f);
        iceGrater.transform.DORotate(new Vector3(90, 0, -180), 1f);
        iceGrater.transform.GetChild(1).transform.DOLocalRotate(new Vector3(-190, 0, 0), 2f);

        DOVirtual.DelayedCall(1f, delegate
        {
            iceFilled.transform.DOScale(Vector3.one, 1f);
            iceGrater.iceFallParticle.Play();
        });

        DOVirtual.DelayedCall(2f, delegate
        {
            iceGrater.transform.GetChild(1).transform.DOLocalRotate(new Vector3(-90, 0, 0), 1f);
            rb.isKinematic = false;
            coll.enabled = true;
            iceGraterCollider.enabled = true;
            iceGraterRb.isKinematic = false;
            int dir = UnityEngine.Random.value > 0.5f ? -1 : 1;
            iceGraterRb.AddForce(Vector2.right * dir * 3, ForceMode.Impulse);
        });
    }
    public void FillRoseWater()
    {
        thisItem.ItemName = "Bici Bici";
        rosed = true;
        iceFilled.GetComponent<Renderer>().material = biciMat;
    }
}
