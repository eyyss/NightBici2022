
using UnityEngine;
using UnityEngine.Rendering;

public class Knife : MonoBehaviour
{
    private Rigidbody rb;
    public Transform raycastTransform;
    public Vector3 targetRot;
    public AudioClip knifeHitClip, knifeSliceClip;
    public AudioData knifeHitAudio, knifeSliceAudio;
    public GameObject bananaSlicesPrefab, watermelonSlicesPrefab;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (!rb.useGravity)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetRot), Time.deltaTime * 5);
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        bool bananaOrWatermelon = false;
        if (rb.velocity.magnitude > 1 && rb.useGravity && collision.gameObject.TryGetComponent(out Item item))
        {
            if (item.ItemName == "Banana")
            {
                bananaOrWatermelon = true;
                SpawnSlices(bananaSlicesPrefab, collision.gameObject);
            }
            else if (item.ItemName == "Watermelon")
            {
                bananaOrWatermelon = true;
                SpawnSlices(watermelonSlicesPrefab, collision.gameObject);
            }
        }

        if (rb.velocity.magnitude > 1 && rb.useGravity)
        {
            if (rb.velocity.magnitude > 3f)
                knifeHitAudio.Play3D(this, transform.position);
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            transform.up = -transform.up * 0.25f - collision.GetContact(0).normal;
            if (!bananaOrWatermelon)
                transform.SetParent(collision.transform);
        }
    }

    public void SpawnSlices(GameObject prefab, GameObject other)
    {
        Destroy(other);
        Instantiate(prefab, transform.position, prefab.transform.rotation);
        knifeSliceAudio.Play3D(this, transform.position);
    }

}
