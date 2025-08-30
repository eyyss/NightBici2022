using UnityEngine;

public class ItemBox : MonoBehaviour, ILeftClickable
{
    public GameObject itemPrefab;
    private InteractionController interactionController;
    void Awake()
    {
        interactionController = Camera.main.GetComponent<InteractionController>();
    }

    public void Click()
    {
        var item = Instantiate(itemPrefab, interactionController.holdArea.position, Quaternion.identity);
        interactionController.PickupObject(item);
    }
}
