using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEntry {

	public ObjectTypes.BoxTypes type = ObjectTypes.BoxTypes.Undetermined;
	private float _xRot = 0f;
	private float _yRot = 0f;

	public float xRot
	{
		get { return _xRot; }
		set { _xRot = value; 
			if (_xRot == 360f)
				_xRot = 0f;
		}
	}

	public float yRot
	{
		get { return _yRot; }
		set { _yRot = value; 
			if (_yRot == 360f)
				_yRot = 0f;
		}
	}
		
	public string caption;
	public int xInd = 0;
	public int yInd = 0;
	public int zInd = 0;

    public float xPos = 0f;
    public float yPos = 0f;
    public float zPos = 0f;

    private GameObject boxGameObj = null;

    // TODO: caption parameter to be implemented
    public BoxEntry(ObjectTypes.BoxTypes typePar, string captionPar, int xIndPar, int yIndPar, int zIndPar,
        float xPosPar, float yPosPar, float zPosPar)
    {
		type = typePar;
		caption = captionPar;
		xInd = xIndPar;
		yInd = yIndPar;
		zInd = zIndPar;
        xPos = xPosPar;
        yPos = yPosPar;
        zPos = zPosPar;
    }

	private BoxEntry(){}

    public GameObject GetBoxGameObj()
    {
        return boxGameObj;
    }

    public void SetBoxGameObj(GameObject boxGameObjPar)
    {
        boxGameObj = boxGameObjPar;
    }

    public BoxEntry ShallowCopy()
    {
        BoxEntry boxEntry = new BoxEntry();
        boxEntry.type = type;
        boxEntry.xRot = xRot;
        boxEntry.yRot = yRot;
        boxEntry.caption = caption;
        boxEntry.boxGameObj = boxGameObj;
        return boxEntry;
    }

    public void Update(BoxEntry otherBoxEntry)
    {
        type = otherBoxEntry.type;
        xRot = otherBoxEntry.xRot;
        yRot = otherBoxEntry.yRot;
        caption = otherBoxEntry.caption;
        boxGameObj = otherBoxEntry.boxGameObj;
    }

    public void Clear()
    {
        type = ObjectTypes.BoxTypes.Undetermined;
        boxGameObj = null;
    }

    public override string ToString()
    {
        return "BoxType: " + ObjectTypes.boxTypesToNames[type] +
            "; xInd: " + xInd +
            "; zInd: " + zInd +
            "; yInd: " + yInd +
            "; boxGameObj: " + (boxGameObj != null ? " EXISTS" : " NULL");
    }
}
