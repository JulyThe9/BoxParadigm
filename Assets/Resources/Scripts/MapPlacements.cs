using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlacements /*: MonoBehaviour*/{

	private BoxPicking boxPicking;

	public MapPlacements(){}

    //make pos dep on indices
    public GameObject placeBox(BoxEntry boxEntry, float xPos, float yPos, float zPos, float yMargin)
    {

		GameObject box = GameObject.Instantiate (Resources.Load (boxEntry.name)) as GameObject;
		box.transform.position = new Vector3 (xPos, yPos + yMargin, zPos);       //0.5f for yMargin
		box.transform.Rotate(boxEntry.xRot, boxEntry.yRot, 0f);

        BoxData boxData = box.GetComponent<BoxData>();
        boxData.xInd = boxEntry.xInd;
        boxData.yInd = boxEntry.yInd;
        boxData.zInd = boxEntry.zInd;

        box.name = boxEntry.name;
		box.tag = "Box";
		return box;
	}

	public GameObject placeCell(ObjectTypes.BoxTypes type, int xInd, int yInd, int zInd, float yMargin, Vector3 refPos)
    {
		GameObject nextCell = GameObject.Instantiate (Resources.Load ("InitCell")) as GameObject;
		InitPosBeh initPosBeh = nextCell.GetComponent<InitPosBeh> ();
		initPosBeh.origMatType = type;

		initPosBeh.xInd = xInd;
		initPosBeh.yInd = yInd;
		initPosBeh.zInd = zInd;

		nextCell.transform.position = new Vector3 (refPos.x, refPos.y + yMargin, refPos.z); 
		return nextCell;
	}
}
