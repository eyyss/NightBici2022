using DG.Tweening;
using UnityEngine;

public class RoseWater : MonoBehaviour
{
    private Rigidbody rb;
    private AudioSource audioSource;
    public ParticleSystem liquidParticle, liquidCircleParticle;
    private Tween rotateTween;
    public Material biciMat;
    public Transform rayPoint;
    public Vector3 targetRot;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        liquidParticle.Stop();
        liquidCircleParticle.transform.SetParent(null);
    }
    private void Update()
    {
        if (rb.useGravity)
        {
            if (liquidParticle.isPlaying)
            {
                audioSource.Stop();
                liquidParticle.Stop();
            }
            return;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            if (rotateTween != null) rotateTween.Complete();
            rotateTween = transform.DOLocalRotate(targetRot, 0.2f).OnComplete(delegate
            {
                audioSource.Play();
                liquidParticle.Play();
                liquidCircleParticle.Play();
            });
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (rotateTween != null) rotateTween.Complete();
            rotateTween = transform.DOLocalRotate(new Vector3(-90, 0, 0), 0.2f);
            liquidParticle.Clear();
            liquidParticle.Stop();
            liquidCircleParticle.Stop();
            audioSource.Stop();
        }


        if (Input.GetKey(KeyCode.E))
        {
            if (Physics.Raycast(rayPoint.position, rayPoint.forward, out RaycastHit hit, 1.5f))
            {
                Debug.DrawLine(transform.position, hit.point);
                ApplyLiquidCircle(hit);

                Debug.Log(hit.normal);


                if (hit.collider.TryGetComponent(out Bowl bowl) && bowl.iced)
                {
                    if (bowl.roseValue < 1)
                        bowl.roseValue += Time.deltaTime;
                    if (bowl.roseValue >= 1 && !bowl.rosed)
                        bowl.FillRoseWater();
                }
            }
        }

    }

    private void ApplyLiquidCircle(RaycastHit hit)
    {
        liquidCircleParticle.transform.position = hit.point + hit.normal * 0.01f;
        Quaternion rot = Quaternion.LookRotation(-hit.normal);
        Vector3 euler = rot.eulerAngles * Mathf.Deg2Rad;
        var main = liquidCircleParticle.main;
        main.startRotation3D = true;   // 3D rotasyonu aktif et
        main.startRotationX = euler.x;
        main.startRotationY = euler.y;
        main.startRotationZ = euler.z;
    }
}
