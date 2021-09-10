using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBoxME : MonoBehaviour
{
    private EditorUI editorUI;
    private BoxPicking boxPicking;
    void Start()
    {
        editorUI = GameObject.Find(GlobalVariables.canvasName).GetComponent<EditorUI>();
        boxPicking = GameObject.Find(GlobalVariables.canvasName).GetComponent<BoxPicking>();

        editorUI.finishBoxPlaced = true;
        BoxData boxData = gameObject.GetComponent<BoxData>();
        editorUI.finishBoxXInd = boxData.xInd;
        editorUI.finishBoxYInd = boxData.yInd;
        editorUI.finishBoxZInd = boxData.zInd;

        if (boxPicking.selBoxType != ObjectTypes.BoxTypes.Undetermined)
        {
            GameObject.Find(ObjectTypes.boxTypesToSlotNames[boxPicking.selBoxType]).SetActive(false);
            boxPicking.selBoxType = ObjectTypes.BoxTypes.Undetermined;
        }
    }
}
