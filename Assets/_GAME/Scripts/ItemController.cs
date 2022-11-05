using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public static event Action Reset;

    static Transform itemFolder = null;
    static Sprite[] mirrorSprites = new Sprite[2];
    public static int LayerItem;
    public static Dictionary<string, Vector2> ItemOffsets = new Dictionary<string, Vector2>();
    public static Dictionary<string, ItemController> AllItems = new Dictionary<string, ItemController>();
    public string Item;
    public bool Pickuppable;
    public bool IsHeld = false;
    public bool IsMoving = false;
    [SerializeField] float BottomEdge;

    SpriteRenderer sRenderer;
    Rigidbody2D rb;
    BoxCollider2D col;
    Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        sRenderer = GetComponent<SpriteRenderer>();
        AllItems.Add(Item, this);
        if (itemFolder == null)
            StartStatic();

        originalPos = transform.position;
        Reset += Replace;
    }

    // Summary:
    //     Called one time on account of all ItemController scripts, to initialize 
    //     static variables.
    public static void StartStatic()
    {
        itemFolder = GameObject.Find("Items").transform;
        LayerItem = LayerMask.GetMask("Item");

        ItemOffsets.Add("Mirror01", new Vector2(0.2f, 0.02f));
        ItemOffsets.Add("Anchor", new Vector2(0.2f, 0.02f));

        mirrorSprites[0] = Resources.Load<Sprite>("Sprites/mirror_tilted");
        mirrorSprites[1] = Resources.Load<Sprite>("Sprites/mirror_normal");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving)
            CheckIfOnFloor();
    }

    // Summary:
    //     Called when a Person interacts with a pickuppable object. Converts this item
    //     to its held form.
    // Parameters:
    //   entity:
    //     The Transform of the Person picking up this item.
    public string PickUp(Transform entity)
    {
        Debug.Log(Item + " picked up!");
        IsHeld = true;
        IsMoving = true;
        transform.parent = entity;
        transform.localPosition = ItemOffsets[Item];
        rb.isKinematic = true;
        col.isTrigger = true;

        return Item;
    }

    // Summary:
    //     Called when a Person drops the item they're carrying. Converts this item back
    //     to its object form.
    // Parameters:
    //   entity:
    //     The Transform of the Person picking up this item.
    public void PutDown(Transform entity)
    {
        IsHeld = false;
        transform.parent = itemFolder;
        rb.isKinematic = false;
        col.isTrigger = false;
        Vector2 momentum = new Vector2(entity.eulerAngles.y == 0 ? 5 : -5, 10);
        rb.AddForce(momentum, ForceMode2D.Impulse);

        if (name == "T_MirrorItem")
            FixMirror();
    }

    // Summary:
    //     Checks whether the bottom of this item is within a tiny radius of the ground,
    //     and prevents it from sliding if it is.
    void CheckIfOnFloor()
    {
        Collider2D[] hitColliders = new Collider2D[2];
        int hitNum = Physics2D.OverlapCircleNonAlloc(new Vector2(transform.position.x, transform.position.y + BottomEdge), 0.01f, hitColliders, Person.LayerFloor);
        if (hitNum == 0)
            return;

        // Has reached the floor
        IsMoving = false;
        rb.velocity = Vector2.zero;
    }

    // Summary:
    //     Only for Mirror items. Activates animation for a Mirror item when deflecting a laser. 
    public void Deflect()
    {
        if (!Item.Contains("Mirror"))
            return;

        sRenderer.sprite = mirrorSprites[0];
        transform.Translate(new Vector2(-0.1f, 0f));
        name = "T_MirrorItem";
        LeanTween.delayedCall(0.4f, FixMirror);
    }

    // Summary:
    //     Only for Mirror items. Ends animation for a Mirror item when deflecting a laser.
    void FixMirror()
    {
        if (!Item.Contains("Mirror") || name != "T_MirrorItem")
            return;

        sRenderer.sprite = mirrorSprites[1];
        transform.Translate(new Vector2(0.1f, 0f));
        name = "MirrorItem";
    }

    // Summary:
    //     Resets item to its starting position when the level is reset.
    void Replace()
    {
        transform.position = originalPos;
    }

    // Summary:
    //     FOR LEVEL BUILDING: makes it easier to set the location of the bottom of the item in the Scene window.
    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position + new Vector3(1, BottomEdge, 0), transform.position + new Vector3(-1, BottomEdge, 0), Color.blue);
    }
}
