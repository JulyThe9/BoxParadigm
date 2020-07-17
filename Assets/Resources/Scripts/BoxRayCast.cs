using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRayCast : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;
	private EditorUI editorUI;
	private MapPlacements mapPlacements = new MapPlacements();

	void Start(){
		editorUI = GameObject.Find ("Canvas").GetComponent<EditorUI> ();
	}

	void Update()
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
        {
			if (editorUI.buildingEnabled && hit.transform.gameObject.tag.Equals ("Box"))
            {				
				GameObject holder = GameObject.Find ("Canvas");
				BoxPicking boxPicking = holder.GetComponent<BoxPicking> ();
				ObjectTypes.BoxTypes curType = boxPicking.boxType;

				if (hit.transform.gameObject.name.Equals (ObjectTypes.boxTypesToNames[curType])) // TODO: simplify
                {
					if (Input.GetMouseButtonDown (0))
                    {
						Debug.Log ("HIT!");
						hit.transform.Rotate (0f, 90f, 0f);
					
						int[] indices = getBoxEntryIndices (hit.transform.gameObject);
						editorUI.bHolder.list [indices [0]] [indices [1]] [indices [2]].yRot += 90f;

					}
				}
                else if (curType == ObjectTypes.BoxTypes.Undetermined)
                {
					if (Input.GetMouseButtonDown (0))
                    {
						Destroy (hit.transform.gameObject);
						int[] indices = getBoxEntryIndices (hit.transform.gameObject);
						int listSize = editorUI.bHolder.list [indices [0]] [indices [1]].Count;
						if (indices [2] >= listSize-1)
                        {
							editorUI.bHolder.list [indices [0]] [indices [1]].RemoveAt (listSize - 1);
						}
						else editorUI.bHolder.list [indices [0]] [indices [1]] [indices [2]] = null;
					}
				}
				else
                {
					if (Input.GetMouseButtonDown (0))
                    {
						Vector3 refPos = hit.transform.position;
                        string curTypeToName = ObjectTypes.boxTypesToNames[curType];

                        int[] indices = getBoxEntryIndices (hit.transform.gameObject);
						BoxEntry boxEntry = new BoxEntry (curTypeToName, "", indices [0], indices [2], indices [1]);
						editorUI.bHolder.list [indices [0]] [indices [1]] [indices [2]] = boxEntry;

						Destroy (hit.transform.gameObject);

						GameObject box = mapPlacements.placeBox (boxEntry, refPos.x, refPos.y, refPos.z, 0f);
						mapPlacements.placeCell (curTypeToName, indices [0], indices [2], indices [1], 0.51f, refPos).transform.parent = box.transform;
					}
				}
				
			}
		}
	}

	private int[] getBoxEntryIndices(GameObject box){
		string indsStr = box.transform.GetChild (box.transform.childCount-1).name.Split ('_') [1];
		int xInd = int.Parse (indsStr.Split (':') [0]);
		int zInd = int.Parse (indsStr.Split (':') [1]);
		int yInd = int.Parse (indsStr.Split (':') [2]);
		return new int[] { xInd, zInd, yInd };
	}
}
