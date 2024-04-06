using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EquipSlot : MonoBehaviour
{
    public SlotPosition slot;
    public OilDrink currentItem;
    private Transform itemLastParent;

    private void OnTriggerEnter(Collider other)
    {
        // TODO: Replace Oil Drink with class heirarchy
        OilDrink item = other.GetComponentInParent<OilDrink>();
        if (item != null)
        {
            if (item.equippableSlots == slot)
            {
                Equip(item);
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
        item.transform.parent = this.transform.parent;
        item.transform.localPosition = item.equipPositionOffset;
        item.transform.localPosition = item.equipPositionOffset;
        item.transform.rotation = Quaternion.Euler(item.equipRotationOffset);
        
        currentItem = item;
        item.currentlyEquippedSlot = this;

        item.OnEquip();
    } 

    public void UnEquip()
    {
        if (currentItem != null) 
        {
            currentItem.GetComponent<Rigidbody>().isKinematic = false;
            XRGrabInteractable interactable = currentItem.GetComponent<XRGrabInteractable>();

            if (interactable != null)
            {
                interactable.enabled = true;
            }


            currentItem.transform.parent = itemLastParent;
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
