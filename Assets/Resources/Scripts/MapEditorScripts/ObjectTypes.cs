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

    static public Dictionary<BoxTypes, string> boxTypesToStartMaterialNames = new Dictionary<BoxTypes, string>
    {
        { BoxTypes.Wood, "Wood_Box_Start" },
        { BoxTypes.Stone, "Stone_Box_Start" },
        { BoxTypes.Turret, "Turret_Box_Start" },
        { BoxTypes.Mirror, "Mirror_Box_Start" }
    };

    static public Dictionary<EffectTypes, string> effectTypesToSelMaterialNames = new Dictionary<EffectTypes, string>
    {
        { EffectTypes.SwapSelect, "SwapSelect" },
        { EffectTypes.QuantumSelect, "QuantumSelect" },
        { EffectTypes.QuantumConnect, "QuantumConnect" }
    };

    public enum EffectTypes : uint
    {
        AnalysisAttack,
        TurretAttack,
        SwapSelect, // quasi-type
        Swapping,
        QuantumSelect, // quasi-type
        QuantumConnect,
        GravityArgument
    }

    public enum ToolTypes : uint
    {
        Undetermined,
        Analysis,
        Synthesis,
        SpaceWarp,
        Levitator
    }

    static public Dictionary<EffectTypes, ToolTypes> effectTypesToToolTypes = new Dictionary<EffectTypes, ToolTypes>
    {
        { EffectTypes.AnalysisAttack, ToolTypes.Analysis },
        { EffectTypes.Swapping, ToolTypes.SpaceWarp },
        { EffectTypes.QuantumConnect, ToolTypes.Synthesis },
        { EffectTypes.GravityArgument, ToolTypes.Levitator }
    };

    public enum BoxActions : uint // Kind of relates to EffectTypes
    {
        Irrelevant,
        Destroyed,
        SwappedBySpace,
        Levitating
    }

    static public Dictionary<EffectTypes, BoxActions> effectTypesToBoxActions = new Dictionary<EffectTypes, BoxActions>
    {
        { EffectTypes.AnalysisAttack, BoxActions.Destroyed },
        { EffectTypes.TurretAttack, BoxActions.Destroyed },
        { EffectTypes.SwapSelect, BoxActions.Irrelevant }, // NOTE: maybe !Irrelevant, but then only partial application (say, visual)
        { EffectTypes.Swapping, BoxActions.SwappedBySpace }, // 
        { EffectTypes.QuantumSelect, BoxActions.Irrelevant },
        { EffectTypes.QuantumConnect, BoxActions.Irrelevant },
        { EffectTypes.GravityArgument, BoxActions.Levitating }
    };

    static public string ledgeToGrabName = "LedgeToGrab";
    static public string ledgeGrabber = "LedgeGrabber";
    static public string bottomJoint = "BottomJoint";

    static public string floorTagName = "Floor";
    static public string boxTagName = "Box";
    static public string projectileTagName = "Projectile";
    static public string playerTagName = "Player";
    static public string playerBottomTagName = "PlayerBottom";
    static public string mainCameraTagName = "MainCamera";

    static public string fire1Name = "Fire1";

    static public string simpleEmergenceName = "SimpleEmergence";

    static public string baseWallName = "BaseWall";
    static public string topWallName = "TopWall";
    static public string leftWallName = "LeftWall";
    static public string rightWallName = "RightWall";
    static public string backWallName = "BackWall";
    static public string frontWallName = "FrontWall";
}
