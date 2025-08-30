using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FuseBox : MonoBehaviour, ILeftClickable
{
    public List<Light> lights;
    private List<float> lightIntensities = new List<float>();
    public List<Renderer> lightRenderers;
    public AudioData powerDownAudio;
    public AudioData powerUpAudio;
    private bool canOpen = false;
    public List<Renderer> fuseRenderers;
    public Color openedColor, closedColor;
    public AudioData fuseAudio;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopElectric();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            StartElectric();
        }
    }

    public void StopElectric()
    {
        canOpen = true;
        // Renderer ışık emisyonlarını kapat
        foreach (var item in lightRenderers)
        {
            item.material.DisableKeyword("_EMISSION");
        }

        // Mevcut ışık değerlerini kaydet
        lightIntensities.Clear();
        foreach (var light in lights)
        {
            lightIntensities.Add(light.intensity);
        }

        // Işıkları sıfıra indir
        foreach (var light in lights)
        {
            float startValue = light.intensity; // güvenli başlangıç değeri
            DOVirtual.Float(startValue, 0f, 1f, newValue =>
            {
                light.intensity = newValue;
            });
        }

        foreach (var item in fuseRenderers)
        {
            item.material.color = closedColor;
            item.material.SetColor("_EmissionColor", closedColor);
        }

        // Güç kapatma sesi çal
        powerDownAudio.Play2D(this);
    }

    public void StartElectric()
    {
        fuseAudio.Play2D(this);
        canOpen = false;
        // Renderer ışık emisyonlarını aç
        foreach (var item in lightRenderers)
        {
            item.material.EnableKeyword("_EMISSION");
        }

        // Işıkları kaydedilmiş değerlere geri döndür
        for (int i = 0; i < lights.Count; i++)
        {
            int index = i; // closure hatasını önlemek için lokal kopya
            float startValue = lights[index].intensity;
            float targetValue = (index < lightIntensities.Count) ? lightIntensities[index] : 1f;
            DOVirtual.Float(startValue, targetValue, 1f, newValue =>
            {
                lights[index].intensity = newValue;
            });
        }

        foreach (var item in fuseRenderers)
        {
            item.material.color = openedColor;
            item.material.SetColor("_EmissionColor", openedColor);
        }

        // Güç açma sesi çal
        powerUpAudio.Play2D(this);
    }

    void OnDestroy()
    {
        if (lightIntensities != null)
        {
            lightIntensities.Clear();
        }
    }

    public void Click()
    {
        if (canOpen)
            StartElectric();
    }
}
