using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // Map editor names
    public static string XIdxBoxInputFieldName = "XIdxBox";
    public static string ZIdxBoxInputFieldName = "ZIdxBox";
    public static string YIdxBoxInputFieldName = "YIdxBox";

    // Play levels names
    public static string mapEditorBoxesName = "MapEditorBoxes";
    public static string playLevelsBoxesName = "PlayLevelsBoxes";

    public static string groundLayerName = "Ground";

    public static string leftHaloPath = "_Tools and projectiles/LeftHalo";
    public static string rightHaloPath = "_Tools and projectiles/RightHalo";

    // TODO: maybe a separate utility script
    public static float prjctlSpeed = 2f;
    public static float maxPrjctlSpeedPercent = 10f;
    public static float prjctlSpeedChangePercent = 0.1f;
}
