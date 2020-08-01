using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPicking : MonoBehaviour {

	//potentially: move the method to EditorUI class
	//edit: forget about that, EditorUI is growing large
	public ObjectTypes.BoxTypes selBoxType;
	public void pickBox(GameObject slot){
		if (!slot.activeSelf)
        {
            if (selBoxType != ObjectTypes.BoxTypes.Undetermined)
            {
                GameObject.Find(ObjectTypes.boxTypesToSlotNames[selBoxType]).SetActive(false);
            }
			slot.SetActive (true);
            selBoxType = slot.GetComponent<SlotData>().slotBoxType;
		}
        else
        {
			slot.SetActive (false);
            selBoxType = ObjectTypes.BoxTypes.Undetermined;
		}
	}

}
