using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected static Transform itemFolder = null;
    public static Dictionary<string, Vector2> Offsets = new Dictionary<string, Vector2>();
    public static Dictionary<string, Item> AllItems = new Dictionary<string, Item>();
    public string ItemId;
    protected Vector3 originalPos;

    public static int LayerCarry;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        AllItems.Add(ItemId, this);
        if (itemFolder == null)
            StartStatic();

        originalPos = transform.position;
        EventManager.ResetLevel += Replace;
    }

    // Summary:
    //     Called one time on account of all Item scripts, to initialize 
    //     static variables.
    public static void StartStatic()
    {
        itemFolder = GameObject.Find("Items").transform;
        LayerCarry = LayerMask.GetMask("Carryable");

        Offsets.Add("Mirror01", new Vector2(0.2f, 0.02f));
        Offsets.Add("Anchor", new Vector2(0.2f, 0.02f));

        MirrorItem.SetupSprites();
    }

    // Summary:
    //     Resets item to its starting position when the level is reset.
    protected virtual void Replace()
    {
        transform.position = originalPos;
    }



    // Summary:
    //     Only for Mirror CarryableItems. Activates animation for a Mirror item when deflecting a laser. 
    public virtual void Deflect() { }

    // Summary:
    //     Only for CarryableItems.
    //     Called when a Person interacts with a pickuppable object. Converts this item
    //     to its held form.
    // Parameters:
    //   entity:
    //     The Transform of the Person picking up this item.
    public virtual string PickUp(Transform entity) { return ItemId; }

    // Summary:
    //     Only for CarryableItems.
    //     Called when a Person drops the item they're carrying. Converts this item back
    //     to its object form.
    // Parameters:
    //   entity:
    //     The Transform of the Person picking up this item.
    public virtual void PutDown(Transform entity) { }
}
