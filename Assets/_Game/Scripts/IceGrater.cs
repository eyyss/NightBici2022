using System.Collections.Generic;
using UnityEngine;

public class IceGrater : MonoBehaviour
{
    public ParticleSystem sicramaParticle;
    public ParticleSystem iceFallParticle;
    public Vector3 targetRot;
    private Rigidbody rb;
    [System.NonSerialized] public bool fill;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (!rb.useGravity)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetRot), Time.deltaTime * 5);
    }

    private void OnCollisionStay(Collision other)
    {
        float relativeVelocityMagnitude = other.relativeVelocity.magnitude;
        //Debug.Log(relativeVelocityMagnitude);


        if (other.gameObject.TryGetComponent(out Ice ice) && relativeVelocityMagnitude > 0.2f)
        {
            Vector3 point = other.GetContact(0).point;
            sicramaParticle.transform.position = point;
            sicramaParticle.Play();
            Vector3 scale = ice.transform.localScale;
            scale.y -= 1f * Time.deltaTime;
            ice.transform.localScale = scale;
            if (ice.transform.localScale.y < 0.1f)
            {
                fill = true;
                Destroy(ice.gameObject);
            }
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Ice ice))
        {
            if (sicramaParticle.isPlaying)
            {
                sicramaParticle.Stop();
            }
        }
    }

}
