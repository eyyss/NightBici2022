using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public Transform holdArea;
    public GameObject heldObj;
    public Rigidbody heldObjRb;
    public static float PickupRange = 4;
    public float pickupForce = 150;
    public float throwForce = 20;

    public Light spotLight;
    public AudioData lightOpenAudio;
    void Update()
    {
        //DrawOutline();

        if (Input.GetKeyDown(KeyCode.F))
        {
            spotLight.enabled = !spotLight.enabled;
            lightOpenAudio.Play2D(this);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, PickupRange))
                {
                    if (hit.collider != null)
                    {
                        PickupObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                DropObject();
            }
        }

        if (heldObj != null)
        {
            MoveObject();
            if (Input.GetMouseButtonDown(1))
            {
                ThrowObject();
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit h, PickupRange))
        {
            if (h.collider.TryGetComponent(out ILeftClickable leftClickable) && Input.GetMouseButtonDown(0))
            {
                leftClickable.Click();
            }
        }
    }
    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.transform.position) > 0.01f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRb.AddForce(moveDirection * pickupForce);
        }
    }
    void ThrowObject()
    {
        Rigidbody tempRb = heldObjRb;
        DropObject();
        float throwForceM = 1;
        if (tempRb.TryGetComponent(out Item item) && item.ItemName == "Knife")
        {
            tempRb.transform.up = transform.forward;
            throwForceM = 5;
        }
        tempRb.AddForce(transform.TransformDirection(Vector3.forward) * throwForce * throwForceM, ForceMode.Impulse);
    }
    public void PickupObject(GameObject pickObj)
    {
        if (pickObj.TryGetComponent(out Rigidbody rb))
        {
            heldObj = pickObj;
            heldObjRb = rb;
            heldObjRb.isKinematic = false;
            heldObjRb.useGravity = false;
            heldObjRb.drag = 10;
            heldObjRb.constraints = RigidbodyConstraints.FreezeRotation;
            heldObj.transform.parent = holdArea;
            Item item = heldObj.GetComponent<Item>();
            UIController.Singelton.OpenInputPanel(item.inputInfos);
        }

    }
    void DropObject()
    {
        heldObjRb.useGravity = true;
        heldObjRb.drag = 1;
        heldObjRb.constraints = RigidbodyConstraints.None;
        heldObj.transform.parent = null;
        heldObjRb = null;
        Item item = heldObj.GetComponent<Item>();
        UIController.Singelton.CloseInputPanel(item.inputInfos);
        heldObj = null;


    }

    private GameObject hitObj;
    private void DrawOutline()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, PickupRange))
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out Rigidbody rb) && rb.useGravity)
            {
                if (hitObj != null) ChangeGameObjectsLayer(hitObj, "Items");
                hitObj = hit.collider.gameObject;
                ChangeGameObjectsLayer(hitObj, "Outline");
            }
            else
            {
                if (hitObj != null)
                    ChangeGameObjectsLayer(hitObj, "Items");
            }
        }
        else
        {
            if (hitObj != null)
                ChangeGameObjectsLayer(hitObj, "Items");
        }
    }
    private void ChangeGameObjectsLayer(GameObject g, string layerName)
    {
        var layer = LayerMask.NameToLayer(layerName); ;
        g.layer = layer;
        var children = g.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}
