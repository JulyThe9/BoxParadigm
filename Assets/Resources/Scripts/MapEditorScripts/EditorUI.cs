using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class EditorUI : MonoBehaviour
{
    public GameObject floor;
    public bool buildingEnabled;

    private bool gridGenerated = false;

    private GameObject player = null;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.AddComponent<CharCtrl>();
    }

    public void parseAndCreateGrid()
    {
        InputField LBox = GameObject.Find("LBox").GetComponent<InputField>();
        InputField WBox = GameObject.Find("WBox").GetComponent<InputField>();
        BoxHolderWrapper.bHolder.length = int.Parse(LBox.text);
        BoxHolderWrapper.bHolder.width = int.Parse(WBox.text);
        createGrid();
    }

    public void createGrid(bool fillBoxList = true)
    {
        floor = setUpPlane (BoxHolderWrapper.bHolder.length, BoxHolderWrapper.bHolder.width, "Floor");
		setUpCells (BoxHolderWrapper.bHolder.length, BoxHolderWrapper.bHolder.width, floor);

        if (fillBoxList)
        {
            fillList(BoxHolderWrapper.bHolder.length, BoxHolderWrapper.bHolder.width);
        }

        gridGenerated = true;

        Debug.Log (BoxHolderWrapper.bHolder.list.Count);
		Debug.Log (BoxHolderWrapper.bHolder.list[0].Count);
		Debug.Log (BoxHolderWrapper.bHolder.list[0][0].Count);
    }

	public GameObject setUpPlane(float length, float width, string surfNamePar)
    {
		if (floor == null)
        {
			floor = Instantiate (Resources.Load (surfNamePar)) as GameObject;
			//Vector3 refPos = player.transform.position;
			//floor.transform.position = new Vector3 (refPos.x - 2.39f, refPos.y - 4.7f, refPos.z + 4f);
            floor.transform.position = new Vector3(0f, 0f, 0f);
            floor.transform.localScale = new Vector3 (length * 0.25f, 1f, width * 0.25f);
		} 
		else
        {
            // TODO: resizes, but boxes remain, prompt / clear
            floor.transform.localScale = new Vector3 (length * 0.25f, 1f, width * 0.25f);
		}
        float speed = (length + width) / 2f * 0.25f * 7f;
        player.GetComponent<CharCtrl>().speed = speed;   //ideally make speed a setting for a user 
		Debug.Log (speed); 
		return floor;
	}

	public void setUpCells(float length, float width, GameObject floor)
    {
		
		if (GameObject.FindGameObjectWithTag ("Cell") != null) {
			GameObject[] cells = GameObject.FindGameObjectsWithTag ("Cell");
			for (int i = 0; i < cells.Length; i++)
				Destroy (cells [i]);
		}
			
		// no need to scale because a box's scale is 1 
		float orix = floor.transform.position.x - (length-1); 
		float oriz = floor.transform.position.z - (width-1);
		float tempOriz = oriz;
		for (int i = 0; i < length; i++) {
			for (int j = 0; j < width; j++) {
				GameObject cell = Instantiate (Resources.Load ("InitCell")) as GameObject;
                cell.transform.parent = floor.transform;
                cell.transform.position = new Vector3 (orix, floor.transform.position.y + 0.01f, oriz);
				oriz += 2f;
				cell.tag = "Cell";

				InitPosBeh initPosBeh = cell.GetComponent<InitPosBeh> ();
				initPosBeh.xInd = i;
				initPosBeh.zInd = j;
				initPosBeh.yInd = -1;

			}
			oriz = tempOriz;
			orix += 2f;
		}
	}

	public void disableBuild()
    {
		buildingEnabled = false;
	}

	public void enableBuild()
    {
		buildingEnabled = true;
	}

	public void saveBoxes()
    {
		XmlSerializer serializer = new XmlSerializer(typeof(BoxHolder));
		FileStream stream = new FileStream (Application.dataPath + "/Resources/StreamingFiles/XML/boxes.xml", FileMode.Create);
		serializer.Serialize (stream, BoxHolderWrapper.bHolder);
		stream.Close ();
	}

    public void loadMap()
    {
        if (BoxHolderWrapper.bHolder.list.Count != 0)
        {
            // TODO: warn the user, prompt for a decision
            clearUserCreatedObjects();
        }
        // TODO: path hardcoded, ask the user
        XmlSerializer serializer = new XmlSerializer(typeof(BoxHolder));
        FileStream stream = new FileStream(Application.dataPath + "/Resources/StreamingFiles/XML/boxes.xml", FileMode.Open);
        BoxHolderWrapper.bHolder = serializer.Deserialize(stream) as BoxHolder;
        stream.Close();

        createGrid(false);

        foreach (List<List<BoxEntry>> column in BoxHolderWrapper.bHolder.list) // x
        {
            foreach (List<BoxEntry> pillar in column) // z
            {
                foreach (BoxEntry boxEntry in pillar) // y
                {
                    if (boxEntry.type != ObjectTypes.BoxTypes.Undetermined)
                    {
                        MapPlacements mapPlacements = new MapPlacements();
                        GameObject box = mapPlacements.placeBox(boxEntry, boxEntry.xPos, boxEntry.yPos, boxEntry.zPos, 0f); // TODO: unify margins
                        boxEntry.SetBoxGameObj(box);
                        mapPlacements.placeCell(boxEntry.type, boxEntry.xInd, boxEntry.yInd, boxEntry.zInd, 0.51f, box.transform.position).transform.parent
                            = box.transform;
                    }
                }
            }
        }
    }

    private void clearUserCreatedObjects()
    {
        // removing the all boxes
        foreach (List<List<BoxEntry>> column in BoxHolderWrapper.bHolder.list)
        {
            foreach (List<BoxEntry> pillar in column)
            {
                foreach (BoxEntry boxEntry in pillar) 
                {
                    if (boxEntry != null)
                    {
                        Destroy(boxEntry.GetBoxGameObj());
                    }
                }
            }
        }

        Destroy(floor);
        floor = null;
    }

	public void preAction(GameObject panel)
    {
        player.GetComponent<CharCtrl>().enabled = false;
		panel.SetActive (true);
	}

	public void cancelAction(GameObject panel)
    {
        player.GetComponent<CharCtrl>().enabled = true;
        panel.SetActive (false);
	}

    public bool GetGridGenerated()
    {
        return gridGenerated;
    }

    private void fillList(int length, int width)
    {
        for (int i = 0; i < length; i++)
        {
            BoxHolderWrapper.bHolder.list.Add(new List<List<BoxEntry>>());
            for (int j = 0; j < width; j++)
            {
                BoxHolderWrapper.bHolder.list[i].Add(new List<BoxEntry>());
            }
        }
    }
}

