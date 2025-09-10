using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string ItemName;
    [NonSerialized] public Rigidbody rb;
    public int[] inputInfos = { 0, 1 };
    public AudioData impactAudio, cutAudio;
    public bool impactDestroyable = false;
    public ParticleSystem cutParticleSystem;
    public bool canThrowGarbage = true;
    void Start()
    {

    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) this.enabled = false;
    }
    void OnDestroy()
    {
        if (cutParticleSystem)
        {
            cutParticleSystem.transform.SetParent(null);
            cutParticleSystem.Play();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (rb == null) return;

        if (rb.useGravity)
        {
            if (impactDestroyable && collision.relativeVelocity.magnitude > 5f && !collision.collider.TryGetComponent(out Item item))
            {
                if (cutAudio)
                    cutAudio.Play3D(this, transform.position);
                Destroy(gameObject);
            }
        }
        if (impactAudio)
            impactAudio.Play3D(this, transform.position);

    }
}