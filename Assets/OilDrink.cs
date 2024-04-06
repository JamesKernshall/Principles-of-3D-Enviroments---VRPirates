using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilDrink : MonoBehaviour
{
    [SerializeField] private float oilLevel = 1;
    [SerializeField] private float timeToDrink = 3;
    [SerializeField] private Transform oilMesh;

    [Header("Equippable Stats")]
    public EquipSlot.SlotPosition equippableSlots;
    public Vector3 equipPositionOffset;
    public Vector3 equipRotationOffset;
    [SerializeField] private Collider equipCollider;

    [HideInInspector] public EquipSlot currentlyEquippedSlot;

    private float drinkingTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateOilVisual();
    }

    void UpdateOilVisual() 
    {
        oilMesh.localScale = new Vector3(oilMesh.localScale.x, oilLevel, oilMesh.localScale.z);
    }
    
    public void OnEquip()
    {
        StartCoroutine(OilDrain());
    }

    IEnumerator OilDrain() 
    {
        while (oilLevel > 0f)
        {
            oilLevel = Mathf.Lerp(1, -0.1f, drinkingTime / timeToDrink);

            yield return new WaitForEndOfFrame();
            drinkingTime += Time.deltaTime;
        }
        yield return new WaitForSeconds(0.15f);

        equipCollider.enabled = false;

        currentlyEquippedSlot.UnEquip();
    }


}
