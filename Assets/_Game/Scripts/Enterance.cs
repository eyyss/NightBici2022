using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enterance : MonoBehaviour
{
    public Image image;
    void Start()
    {
        image.color = Color.black;
        image.DOColor(Color.clear, 2f).SetDelay(2f).OnComplete(delegate
        {
            DialogController.Singelton.StartDialog(0);
            DialogController.Singelton.dialogDoneEvent.AddListener(DialogDone);
        });
    }
    private void DialogDone(int dialogueIndex, int b, bool lastDialogue)
    {
        if (lastDialogue)
        {
            Debug.Log("diyalog bitti");
            image.DOColor(Color.black, 2f).SetDelay(1f).OnComplete(delegate
            {
                // siradaki sahneye geç
                SceneManager.LoadScene("Gameplay");
            });
        }
    }

}
