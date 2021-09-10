using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxRayCast : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;
    private GameObject holder;
	private EditorUI editorUI;
    private BoxPicking boxPicking;
	private MapPlacements mapPlacements = new MapPlacements();

	void Start()
    {
        holder = GameObject.Find(GlobalVariables.canvasName);
        editorUI = holder.GetComponent<EditorUI> ();
        boxPicking = holder.GetComponent<BoxPicking>();
    }

	void Update()
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray, out hit))
        {
			if (editorUI.buildingEnabled && hit.transform.gameObject.tag.Equals (ObjectTypes.boxTagName))
            {				 
				ObjectTypes.BoxTypes curType = boxPicking.selBoxType;

                BoxData boxData = hit.transform.gameObject.GetComponent<BoxData>();
                int xInd = boxData.xInd;
                int yInd = boxData.yInd;
                int zInd = boxData.zInd;
                List<BoxEntry> boxEntryColumn = BHWrapper.bHolder.list[xInd][zInd];

                if (boxEntryColumn[yInd].type == curType)
                {
					if (Input.GetMouseButtonDown (0))
                    {
						Debug.Log ("HIT!");
                        hit.transform.Rotate (0f, 90f, 0f);
                        boxEntryColumn[yInd].yRot += 90f;
					}
				}
                else if (curType == ObjectTypes.BoxTypes.Undetermined)
                {
					if (Input.GetMouseButtonDown (0))
                    {
                        if (editorUI.startBoxSelecting)
                        {
                            editorUI.startBoxSelecting = false;
                            if (boxEntryColumn[yInd].type != ObjectTypes.BoxTypes.Finish)
                            {
                                editorUI.ConfirmStartBoxChoosing(xInd, yInd, zInd);
                            }
                        }
                        else
                        {
                            Destroy(hit.transform.gameObject);
                            // TODO: a better solution is to create Destroy method in BoxEntry
                            // to do game object destruction and this
                            if (boxEntryColumn[yInd].type == ObjectTypes.BoxTypes.Finish)
                            {
                                editorUI.finishBoxPlaced = false;
                            }

                            int columnSize = boxEntryColumn.Count;
                            if (yInd >= columnSize - 1)
                            {
                                boxEntryColumn.RemoveAt(columnSize - 1);
                            }
                            else
                            {
                                boxEntryColumn[yInd].type = ObjectTypes.BoxTypes.Undetermined;
                            }
                        }
					}
				}
				else
                {
					if (Input.GetMouseButtonDown (0))
                    {
                        Destroy(hit.transform.gameObject);
                        // TODO: the same as above
                        if (boxEntryColumn[yInd].type == ObjectTypes.BoxTypes.Finish)
                        {
                            editorUI.finishBoxPlaced = false;
                        }
                        else if (xInd == editorUI.startBoxXInd && zInd == editorUI.startBoxZInd && yInd == editorUI.startBoxYInd)
                        {
                            editorUI.startBoxChosen = false;
                        }

                        Vector3 refPos = hit.transform.position;

						BoxEntry boxEntry = new BoxEntry (curType, "", xInd, yInd, zInd, refPos.x, refPos.y, refPos.z);
                        boxEntryColumn[yInd] = boxEntry;

                        GameObject box = mapPlacements.placeBox (boxEntry, refPos.x, refPos.y, refPos.z, 0f, GlobalVariables.mapEditorBoxesName);
                        boxEntry.SetBoxGameObj(box);
                        mapPlacements.placeCell (curType, xInd, yInd, zInd, 
                            GlobalDimensions.halfMargin_ + GlobalDimensions.minDifDistance_, refPos).transform.parent = box.transform;
					}
				}
				
			}
		}
	}
}
