using UnityEngine;
using System.Runtime.CompilerServices;

public class ToolControl : MonoBehaviour
{
    public Tool[] loadout_;
    public Transform toolParent_;

    private ObjectTypes.ToolTypes curToolType_;
    private GameObject currentTool_;

    public void Equip(int toolIdx)
    {
        if (curToolType_ != ObjectTypes.ToolTypes.Undetermined)
        {
            if (curToolType_ != loadout_[toolIdx].type_)
            {
                Destroy(currentTool_); // TODO: more efficient with enabling/disabling?

                curToolType_ = loadout_[toolIdx].type_;
                InstToolPrefab(toolIdx);
            }
        }
        else
        {
            curToolType_ = loadout_[toolIdx].type_;
            InstToolPrefab(toolIdx);
        }
    }

    public void Unquip()
    {
        if (curToolType_ != ObjectTypes.ToolTypes.Undetermined)
        {
            curToolType_ = ObjectTypes.ToolTypes.Undetermined;
            Destroy(currentTool_);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void InstToolPrefab(int toolIdx)
    {
        GameObject newTool = Instantiate(loadout_[toolIdx].prefab_, toolParent_.position, toolParent_.rotation, toolParent_) as GameObject;
        newTool.transform.localPosition = Vector3.zero;
        newTool.transform.localEulerAngles = Vector3.zero;
        currentTool_ = newTool;
    }
}
