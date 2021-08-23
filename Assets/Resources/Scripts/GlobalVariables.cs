using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static string mapEditorBoxesName = "MapEditorBoxes";
    public static string playLevelsBoxesName = "PlayLevelsBoxes";

    public static string groundLayerName = "Ground";

    // TODO: maybe a separate utility script
    public static float prjctlSpeed = 1f;
    public static float maxPrjctlSpeedPercent = 10f;
    public static float prjctlSpeedChangePercent = 0.1f;
}
