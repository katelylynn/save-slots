using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorItem : Item
{
    static Sprite[] mirrorSprites = new Sprite[2];
    bool tilted = false;

    public static void SetupSprites()
    {
        mirrorSprites[0] = Resources.Load<Sprite>("Sprites/mirror_tilted");
        mirrorSprites[1] = Resources.Load<Sprite>("Sprites/mirror_normal");
    }

    // Summary:
    //     Called when a Person drops the item they're carrying. Converts this item back
    //     to its object form.
    // Parameters:
    //   entity:
    //     The Transform of the Person picking up this item.
    public override void PutDown(Transform entity)
    {
        base.PutDown(entity);
        FixMirror();
    }

    // Summary:
    //     Only for Mirror items. Activates animation for a Mirror item when deflecting a laser. 
    public void Deflect()
    {
        sRenderer.sprite = mirrorSprites[0];
        transform.Translate(new Vector2(-0.1f, 0f));
        tilted = true;
        LeanTween.delayedCall(0.4f, FixMirror);
    }

    // Summary:
    //     Only for Mirror items. Ends animation for a Mirror item when deflecting a laser.
    void FixMirror()
    {
        if (!tilted)
            return;

        sRenderer.sprite = mirrorSprites[1];
        transform.Translate(new Vector2(0.1f, 0f));
        tilted = false;
    }
}
