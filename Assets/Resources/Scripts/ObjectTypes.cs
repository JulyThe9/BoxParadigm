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

}
