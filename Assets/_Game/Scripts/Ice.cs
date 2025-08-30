using System.Collections;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public AudioSource source; // Sürtünme sesi
    public AudioClip[] frictionClips;
    private bool isEnter = false;
    private bool isPlayingLoop = false;
    private Rigidbody itemRb;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Item item) && item.TryGetComponent(out IceGrater iceGrater))
        {
            isEnter = true;
            itemRb = item.GetComponent<Rigidbody>();

            if (!isPlayingLoop)
            {
                StartCoroutine(PlayFrictionLoop());
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Item item))
        {
            isEnter = false;
            itemRb = null;
        }
    }
    IEnumerator PlayFrictionLoop()
    {
        isPlayingLoop = true;
        int clipIndex = 0;

        while (isEnter && itemRb != null)
        {
            float speed = itemRb.velocity.magnitude;

            if (speed > 0.1f)
            {
                source.clip = frictionClips[clipIndex];
                source.Play();

                // Ses süresi kadar bekle
                yield return new WaitForSeconds(source.clip.length);

                // Diğer sese geç
                clipIndex = (clipIndex + 1) % frictionClips.Length;
            }
            else
            {
                yield return null;
            }
        }

        isPlayingLoop = false;
    }
}
