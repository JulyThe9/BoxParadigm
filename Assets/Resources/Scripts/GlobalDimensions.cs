using UnityEngine;

public class GlobalDimensions : MonoBehaviour
{
    public static float margin_ = 1.0f;
    public static float halfMargin_ = 0.5f;
    public static float minDifDistance_ = 0.001f;
    // TODO: add better calculations for ledge grabbing
    public static float marginToLedge_ = 0.5f * margin_ + 0.05f * halfMargin_;
    public static float boxToPlayerHaDist = 1.01f; // TODO: not used

    public static int levelMarginInBoxes_ = 2;
    public static float halfFloorThickness_ = 0.5f;
    public static float playerSize_ = 1.0f;
}
