using UnityEngine;

public class Broom : MonoBehaviour
{
    private Rigidbody rb;
    public Vector3 targetRot;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (!rb.useGravity)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(targetRot), Time.deltaTime * 3);
        }
    }
}
