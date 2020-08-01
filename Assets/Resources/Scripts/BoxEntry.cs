﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEntry {

	public ObjectTypes.BoxTypes type;
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
	public int xInd;
	public int yInd;
	public int zInd;

    public float xPos;
    public float yPos;
    public float zPos;

    private GameObject boxGameObj;

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

	public bool validate(){
		if (xRot % 90f != 0 || yRot % 90f != 0) return false;
		return true;
	}

    public GameObject GetBoxGameObj()
    {
        return boxGameObj;
    }

    public void SetBoxGameObj(GameObject boxGameObjPar)
    {
        boxGameObj = boxGameObjPar;
    }
}
