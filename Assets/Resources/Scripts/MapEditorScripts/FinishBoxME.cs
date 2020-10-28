using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBoxME : MonoBehaviour
{
    private EditorUI editorUI;
    private BoxPicking boxPicking;
    void Start()
    {
        editorUI = GameObject.Find("Canvas").GetComponent<EditorUI>();
        boxPicking = GameObject.Find("Canvas").GetComponent<BoxPicking>();
        editorUI.finishBoxPlaced = true;
        GameObject.Find(ObjectTypes.boxTypesToSlotNames[boxPicking.selBoxType]).SetActive(false);
        boxPicking.selBoxType = ObjectTypes.BoxTypes.Undetermined;
    }
}
