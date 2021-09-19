using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    // Scene names
    public static string startMenu = "StartMenu";
    public static string mapEditorName = "MapEditor";
    public static string playLevelsName = "PlayLevels";

    // Map editor names
    public static string canvasName = "Canvas";

    public static string XIdxBoxInputFieldName = "XIdxBox";
    public static string ZIdxBoxInputFieldName = "ZIdxBox";
    public static string YIdxBoxInputFieldName = "YIdxBox";

    public static string saveLevelNameBoxInputFieldName = "SaveLevelNameBox";
    public static string levelsSubpath = "/Resources/StreamingFiles/Levels/PlayerCreated/";
    public static string levelFileExtension = ".xml";

    public static string levelSavingPanelName = "Bg3"; // TODO: please...

    public static string analysisCountBoxInputFieldName = "AnalysisCountBox";
    public static string spaceWarpBoxInputFieldName = "SpaceWarpCountBox";
    public static string synthesisCountBoxInputFieldName = "SynthesisCountBox";
    public static string levitatorBoxInputFieldName = "LevitatorCountBox";

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
