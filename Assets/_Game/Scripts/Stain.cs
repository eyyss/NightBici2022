using UnityEngine;

public class Stain : MonoBehaviour
{
    private Renderer stainRenderer;
    public float currentAlpha = 1;
    public AudioData broomSweepAudio;
    private float timer;
    private float cooldown = 0.1f;
    private bool clean = false;
    public ParticleSystem effect;
    void Awake()
    {
        stainRenderer = GetComponent<Renderer>();
    }
    void Update()
    {
        timer += Time.deltaTime;

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Broom broom) && collision.rigidbody.velocity.magnitude > 0.1 && !clean)
        {
            if (timer > cooldown)
            {
                timer = 0;

                Color c = stainRenderer.material.color;

                if (currentAlpha < 0.4)
                {
                    clean = true;
                    c.a = 0;
                    gameObject.SetActive(false);
                }
                else
                {
                    broomSweepAudio.Play2D(this);
                    effect.Play();
                }

                c.a -= 0.1f;
                currentAlpha = c.a;
                stainRenderer.material.color = c;
            }

        }
    }
}
