using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPosBeh : MonoBehaviour {

	public ObjectTypes.BoxTypes origMatType;
	private EditorUI editorUI;
	private Vector3 refPos;
	private GameObject holder;
	private BoxPicking boxPicking;
	private MapPlacements mapPlacements= new MapPlacements();

	// FINALLY START USING PROPERTIES WITH GET AND SET
	public int xInd;
	public int yInd;
	public int zInd; 

	void Start()
    {
		gameObject.GetComponent<Renderer>().material =  Resources.Load("Materials/" + ObjectTypes.boxTypesToMaterialNames[origMatType], 
            typeof(Material)) as Material;
		editorUI = GameObject.Find ("Canvas").GetComponent<EditorUI> ();

		refPos = transform.position;
		holder = GameObject.Find ("Canvas");
		boxPicking = holder.GetComponent<BoxPicking> ();
		gameObject.GetComponent<Renderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

		transform.name = "cell_" + xInd + ":" + zInd + ":" + yInd;
	}
	void OnMouseEnter()
	{
		Material selectedMat = Resources.Load("Materials/" + ObjectTypes.boxTypesToSelMaterialNames[origMatType], typeof(Material)) as Material;
		gameObject.GetComponent<Renderer>().material = selectedMat;
	}
	void OnMouseExit()
	{
		Material origMat = Resources.Load("Materials/"+ ObjectTypes.boxTypesToMaterialNames[origMatType], typeof(Material)) as Material;
		gameObject.GetComponent<Renderer>().material = origMat;
	}
	void OnMouseUp()
	{
		//Material origMat = Resources.Load("Materials/InitCell", typeof(Material)) as Material;
		//gameObject.GetComponent<Renderer>().material = origMat;
		Debug.Log (editorUI.buildingEnabled);
		if(editorUI.buildingEnabled)
        {		
			ObjectTypes.BoxTypes curType = boxPicking.selBoxType;
			if (curType != ObjectTypes.BoxTypes.Undetermined)
            {
				BoxEntry boxEntry = new BoxEntry (curType, "", xInd, yInd + 1, zInd, refPos.x, refPos.y + GlobalDimensions.halfMargin_, refPos.z);
                if (yInd + 1 < BoxHolderWrapper.bHolder.list[xInd][zInd].Count)
                {
                    BoxHolderWrapper.bHolder.list[xInd][zInd][yInd + 1] = boxEntry;
                }
                else
                {
                    BoxHolderWrapper.bHolder.list[xInd][zInd].Add(boxEntry);
                }
				
				GameObject box = mapPlacements.placeBox (boxEntry, refPos.x, refPos.y, refPos.z, GlobalDimensions.halfMargin_);
                boxEntry.SetBoxGameObj(box);
                mapPlacements.placeCell (curType, xInd, yInd+1, zInd, GlobalDimensions.margin_ + GlobalDimensions.minDifDistance_, refPos).transform.parent = box.transform;
			}
		}

	}
}
