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
        Mirror,
        Finish
    }

    static public Dictionary<BoxTypes, string> boxTypesToNames = new Dictionary<BoxTypes, string>
    {
        { BoxTypes.Undetermined, "Undetermined_Box" },
        { BoxTypes.Wood, "Wood_Box" },
        { BoxTypes.Stone, "Stone_Box" },
        { BoxTypes.Turret, "Turret_Box" },
        { BoxTypes.Mirror, "Mirror_Box" },
        { BoxTypes.Finish, "Finish_Box" }
    };

    static public Dictionary<BoxTypes, string> boxTypesToSlotNames = new Dictionary<BoxTypes, string>
    {
        { BoxTypes.Wood, "Wood_Slot" },
        { BoxTypes.Stone, "Stone_Slot" },
        { BoxTypes.Turret, "Turret_Slot" },
        { BoxTypes.Mirror, "Mirror_Slot" },
        { BoxTypes.Finish, "Finish_Slot" }
    };

    static public Dictionary<BoxTypes, string> boxTypesToMaterialNames = new Dictionary<BoxTypes, string>
    {
        { BoxTypes.Undetermined, "InitCell" },
        { BoxTypes.Wood, "Wood_Box" },
        { BoxTypes.Stone, "Stone_Box" },
        { BoxTypes.Turret, "Turret_Box" },
        { BoxTypes.Mirror, "Mirror_Box" },
        { BoxTypes.Finish, "Finish_Box" }
    };

    static public Dictionary<BoxTypes, string> boxTypesToSelMaterialNames = new Dictionary<BoxTypes, string>
    {
        { BoxTypes.Undetermined, "InitCell_Sel" },
        { BoxTypes.Wood, "Wood_Box_Sel" },
        { BoxTypes.Stone, "Stone_Box_Sel" },
        { BoxTypes.Turret, "Turret_Box_Sel" },
        { BoxTypes.Mirror, "Mirror_Box_Sel" },
        { BoxTypes.Finish, "Finish_Box_Sel" }
    };

    public enum AttackTypes : uint
    {
        AnalysisAttack,
        TurretAttack
    }

    static public string ledgeToGrabName = "LedgeToGrab";
    static public string ledgeGrabber = "LedgeGrabber";
    static public string bottomJoint = "BottomJoint";

    static public string floorTagName = "Floor";
    static public string boxTagName = "Box";
}
