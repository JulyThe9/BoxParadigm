using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPicking : MonoBehaviour {

	//potentially: move the method to EditorUI class
	//edit: forget about that, EditorUI is growing large
	public ObjectTypes.BoxTypes selBoxType;

    private EditorUI editorUI;

    void Start()
    {
        editorUI = GameObject.Find(GlobalVariables.canvasName).GetComponent<EditorUI>();
    }

    public void pickBox(GameObject slot)
    {
        editorUI.startBoxSelecting = false;
        if (!slot.activeSelf)
        {
            DeactivateCurSlot();
            selBoxType = slot.GetComponent<SlotData>().slotBoxType;
            if (selBoxType != ObjectTypes.BoxTypes.Finish || !editorUI.finishBoxPlaced)
            {
                slot.SetActive(true);
            }
            else
            {
                selBoxType = ObjectTypes.BoxTypes.Undetermined;
            }
        }
        else
        {
			slot.SetActive (false);
            selBoxType = ObjectTypes.BoxTypes.Undetermined;
		}
	}

    public void DeactivateCurSlot()
    {
        if (selBoxType != ObjectTypes.BoxTypes.Undetermined)
        {
            GameObject.Find(ObjectTypes.boxTypesToSlotNames[selBoxType]).SetActive(false);
            selBoxType = ObjectTypes.BoxTypes.Undetermined;
        }
    }
}
