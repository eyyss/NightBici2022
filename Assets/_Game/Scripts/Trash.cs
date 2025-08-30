using UnityEngine;

public class Trash : MonoBehaviour
{
    public AudioData garbageDisposalAudio;
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out Item item) && item.canThrowGarbage && other.rigidbody != null && other.rigidbody.useGravity)
        {
            garbageDisposalAudio.Play3D(this, transform.position);
            Destroy(other.gameObject);
        }
    }
}
