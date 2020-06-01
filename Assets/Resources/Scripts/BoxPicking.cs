using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPicking : MonoBehaviour {

	//potentially: move the method to EditorUI class
	//edit: forget about that, EditorUI is growing large
	public string boxType;
	private string activeSlot;
	public void pickBox(GameObject slot){
		if (!slot.activeSelf) {
			if (!string.IsNullOrEmpty (activeSlot))
				GameObject.Find (activeSlot).SetActive (false);
			slot.SetActive (true);
			boxType = slot.name.Split('_')[0]+"_Box";
			activeSlot = slot.name;
		} else {
			slot.SetActive (false);
			boxType = string.Empty;
			activeSlot = string.Empty;
		}
	}

}
