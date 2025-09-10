using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using EasyPeasyFirstPersonController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    public GameObject phonePanel;
    public TextMeshProUGUI numberText;
    private bool canOpen = true;
    public AudioData phoneCallingAudio;
    public AudioData numberButtonAudio;
    private FirstPersonController fpsController;

    [Header("PHOTO")]
    public RenderTexture renderTexture;
    private Camera cam;
    public Image photoDisplay; // UI Image
    private List<string> photoPaths = new List<string>();
    private int currentIndex = 0;
    public AudioData cameraClickAudio;
    public Image whiteImage;

    void Start()
    {
        cam = Camera.main;
        fpsController = FirstPersonController.Singelton;
        phonePanel.SetActive(false);

        LoadPhotoPaths();
    }
    public void NumberButtonOnClick(int index)
    {
        if (numberText.text.Length >= 8) return;
        numberButtonAudio.Play2D(this);
        numberText.text += index;
    }
    public void Call()
    {
        if (numberText.text != "5554213")
        {
            numberText.text = "<color=red>Unknown</color>";
            Invoke(nameof(ResetNumberText), 1f);
            return;
        }
        //EventManager.Singelton.InvokeEvent("PhoneCall" + numberText.text);
        ClosePhone();
        phoneCallingAudio.Play2D(this);
        canOpen = false;
        Invoke(nameof(CookDialogue), 6f);
    }
    private void CookDialogue()
    {
        DialogController.Singelton.StartDialog(2);
    }
    private void ResetNumberText()
    {
        numberText.text = string.Empty;
    }
    public void DeleteLastNumber()
    {
        if (numberText.text.Length <= 0) return;
        var newString = numberText.text.Substring(0, numberText.text.Length - 1);
        numberText.text = newString;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && canOpen)
        {
            if (!phonePanel.activeSelf && !fpsController.blockGlobal)
            {
                OpenPhone();
                return;
            }
            if (phonePanel.activeSelf)
            {
                ClosePhone();
            }
        }
    }
    public void OpenPhone()
    {
        phonePanel.SetActive(true);
        fpsController.SetControl(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ClosePhone()
    {
        phonePanel.SetActive(false);
        fpsController.SetControl(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LoadPhotoPaths()
    {
        photoPaths = new List<string>();

        string folder = Application.persistentDataPath;
        string[] files = Directory.GetFiles(folder, "photo_*.png");
        photoPaths = new List<string>(files);
        photoPaths.Sort();

        ShowPhoto();
    }
    public void PreviousPhoto()
    {
        if (photoPaths.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0) currentIndex = photoPaths.Count - 1;

        ShowPhoto();
    }

    public void NextPhoto()
    {
        if (photoPaths.Count == 0) return;

        currentIndex++;
        if (currentIndex >= photoPaths.Count) currentIndex = 0;

        ShowPhoto();
    }
    void ShowPhoto()
    {
        if (photoPaths.Count == 0)
        {
            photoDisplay.sprite = null;
            return;
        }
        string path = photoPaths[currentIndex];
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        photoDisplay.sprite = sprite;
    }
    // Yeni foto eklendiğinde çağırabilirsin
    public void RefreshGallery()
    {
        LoadPhotoPaths();
        if (photoPaths.Count > 0)
        {
            currentIndex = photoPaths.Count - 1; // son foto
            ShowPhoto();
        }
    }

    public void TakePhoto()
    {
        ClosePhone();
        cameraClickAudio.Play2D(this);

        whiteImage.gameObject.SetActive(true);
        DOVirtual.DelayedCall(0.1f, delegate { whiteImage.gameObject.SetActive(false); });

        cam.Render();
        RenderTexture.active = renderTexture;

        // Telefon dikey oranı (örnek 9:16)
        float phoneAspect = 9f / 16f;
        float camAspect = renderTexture.width / (float)renderTexture.height;

        Rect cropRect;

        if (camAspect > phoneAspect)
        {
            // ekran daha geniş, yataydan kırp
            float newWidth = renderTexture.height * phoneAspect;
            float xOffset = (renderTexture.width - newWidth) / 2f;
            cropRect = new Rect(xOffset, 0, newWidth, renderTexture.height);
        }
        else
        {
            // ekran daha dar veya eşit, üstten/altından kırp
            float newHeight = renderTexture.width / phoneAspect;
            float yOffset = (renderTexture.height - newHeight) / 2f;
            cropRect = new Rect(0, yOffset, renderTexture.width, newHeight);
        }

        // Fotoğrafı cropRect ile al
        Texture2D photo = new Texture2D((int)cropRect.width, (int)cropRect.height, TextureFormat.RGB24, false);
        photo.ReadPixels(cropRect, 0, 0);
        photo.Apply();

        RenderTexture.active = null;

        // Fotoğrafı kaydet
        byte[] bytes = photo.EncodeToPNG();
        string path = Path.Combine(Application.persistentDataPath, "photo_" + System.DateTime.Now.Ticks + ".png");
        File.WriteAllBytes(path, bytes);


        RefreshGallery();
    }

}
