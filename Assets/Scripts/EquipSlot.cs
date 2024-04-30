using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EquipSlot : MonoBehaviour
{
    
    public SlotPosition slot;
    public OilDrink currentItem;

    [SerializeField] private bool updatePositionEachFrame = false;
    private Transform itemLastParent;

    private void FixedUpdate()
    {
        if (updatePositionEachFrame && currentItem != null) 
        {
            currentItem.transform.localPosition = currentItem.equipPosition.localPosition;
            currentItem.transform.localRotation = currentItem.equipPosition.localRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO: Replace Oil Drink with class heirarchy
        if (currentItem == null) // If nothing already equipped
        {
            OilDrink item = other.GetComponentInParent<OilDrink>();
            if (item != null)
            {
                if (item.equippableSlots == slot)
                {
                    Equip(item);
                }

            }
        }
    }

    public void Equip(OilDrink item) 
    {
        item.GetComponent<Rigidbody>().isKinematic = true;
        XRGrabInteractable interactable = item.GetComponent<XRGrabInteractable>();
        
        if (interactable != null) 
        {
            interactable.enabled = false;
        }

        itemLastParent = item.transform.parent;
        item.transform.parent = this.transform;
        item.transform.localPosition = item.equipPosition.localPosition;
        item.transform.localRotation = item.equipPosition.localRotation;
        
        currentItem = item;
        item.currentlyEquippedSlot = this;

        item.OnEquip();
    } 

    public void UnEquip()
    {
        if (currentItem != null) 
        {
            currentItem.transform.parent = itemLastParent;

            Rigidbody currentItemRB = currentItem.GetComponent<Rigidbody>();
            currentItemRB.isKinematic = false;
            currentItemRB.velocity = Vector3.zero;
            XRGrabInteractable interactable = currentItem.GetComponent<XRGrabInteractable>();

            if (interactable != null)
            {
                interactable.enabled = true;
            }

            currentItem = null;
        }
    }

    public enum SlotPosition 
    {
        None = 0,
        Head = 1,
        Shoulder = 2,
        Knee = 3,
        Toes = 4
    }
}
