using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTypes : MonoBehaviour
{
    public enum BoxTypes : uint
    {
        Undetermined,
        Wood,
        Stone,
        Turret,
        Mirror
    }

    static public Dictionary<BoxTypes, string> boxTypesToNames = new Dictionary<BoxTypes, string>
    {
        { BoxTypes.Undetermined, "Undetermined_Box" },
        { BoxTypes.Wood, "Wood_Box" },
        { BoxTypes.Stone, "Stone_Box" },
        { BoxTypes.Turret, "Turret_Box" },
        { BoxTypes.Mirror, "Mirror_Box" }
    };

    static public Dictionary<BoxTypes, string> boxTypesToSlotNames = new Dictionary<BoxTypes, string>
    {
        { BoxTypes.Wood, "Wood_Slot" },
        { BoxTypes.Stone, "Stone_Slot" },
        { BoxTypes.Turret, "Turret_Slot" },
        { BoxTypes.Mirror, "Mirror_Slot" }
    };

    static public Dictionary<BoxTypes, string> boxTypesToMaterialNames = new Dictionary<BoxTypes, string>
    {
        { BoxTypes.Undetermined, "InitCell" },
        { BoxTypes.Wood, "Wood_Box" },
        { BoxTypes.Stone, "Stone_Box" },
        { BoxTypes.Turret, "Turret_Box" },
        { BoxTypes.Mirror, "Mirror_Box" }
    };

    static public Dictionary<BoxTypes, string> boxTypesToSelMaterialNames = new Dictionary<BoxTypes, string>
    {
        { BoxTypes.Undetermined, "InitCell_Sel" },
        { BoxTypes.Wood, "Wood_Box_Sel" },
        { BoxTypes.Stone, "Stone_Box_Sel" },
        { BoxTypes.Turret, "Turret_Box_Sel" },
        { BoxTypes.Mirror, "Mirror_Box_Sel" }
    };

    static public string ledgeToGrabName = "LedgeToGrab";
}
