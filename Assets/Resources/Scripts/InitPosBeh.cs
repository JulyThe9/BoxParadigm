using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPosBeh : MonoBehaviour {

	public string origMatName;
	private EditorUI editorUI;
	private Vector3 refPos;
	private GameObject holder;
	private BoxPicking boxPicking;
	private MapPlacements mapPlacements= new MapPlacements();

	// FINALLY START USING PROPERTIES WITH GET AND SET
	public int xInd;
	public int yInd;
	public int zInd; 

	void Start(){
		gameObject.GetComponent<Renderer>().material =  Resources.Load("Materials/"+origMatName, typeof(Material)) as Material;
		editorUI = GameObject.Find ("Canvas").GetComponent<EditorUI> ();

		refPos = transform.position;
		holder = GameObject.Find ("Canvas");
		boxPicking = holder.GetComponent<BoxPicking> ();
		gameObject.GetComponent<Renderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

		transform.name = "cell_" + xInd + ":" + zInd + ":" + yInd;
	}
	void OnMouseEnter()
	{
		Material selectedMat = Resources.Load("Materials/"+origMatName+"_Sel", typeof(Material)) as Material;
		gameObject.GetComponent<Renderer>().material = selectedMat;
	}
	void OnMouseExit()
	{
		Material origMat = Resources.Load("Materials/"+origMatName, typeof(Material)) as Material;
		gameObject.GetComponent<Renderer>().material = origMat;
	}
	void OnMouseUp()
	{
		//Material origMat = Resources.Load("Materials/InitCell", typeof(Material)) as Material;
		//gameObject.GetComponent<Renderer>().material = origMat;
		Debug.Log (editorUI.buildingEnabled);
		if(editorUI.buildingEnabled){
			
			string curType = boxPicking.boxType;
			if (!string.IsNullOrEmpty(curType)) {

				//caption parameter to be implemented

				BoxEntry boxEntry = new BoxEntry (curType, "", xInd, yInd + 1, zInd);
				if (yInd + 1 < editorUI.bHolder.list [xInd] [zInd].Count) {
					editorUI.bHolder.list [xInd] [zInd] [yInd + 1] = boxEntry;
				}
				else 
					editorUI.bHolder.list [xInd] [zInd].Add (boxEntry); 
				
				GameObject box = mapPlacements.placeBox (boxEntry, refPos.x, refPos.y, refPos.z, 0.5f);
				mapPlacements.placeCell (curType,xInd,yInd+1,zInd,1.01f,refPos).transform.parent = box.transform;
			}
		}

	}
}
