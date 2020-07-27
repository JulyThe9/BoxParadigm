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
    
	public BoxHolder bHolder = new BoxHolder();

    private bool gridGenerated = false;

    private GameObject player = null;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void parseAndCreateGrid()
    {
        InputField LBox = GameObject.Find("LBox").GetComponent<InputField>();
        InputField WBox = GameObject.Find("WBox").GetComponent<InputField>();
        bHolder.length = int.Parse(LBox.text);
        bHolder.width = int.Parse(WBox.text);
        createGrid();
    }

    public void createGrid()
    {
        floor = setUpPlane (bHolder.length, bHolder.width, "Floor");
		setUpCells (bHolder.length, bHolder.width, floor);

		fillList(bHolder.length, bHolder.width);
		Debug.Log (bHolder.list.Count);
		Debug.Log (bHolder.list[0].Count);
		Debug.Log (bHolder.list[0][0].Count);

        gridGenerated = true;
    }

	public GameObject setUpPlane(float length, float width, string surfNamePar)
    {
		if (floor == null)
        {
			floor = Instantiate (Resources.Load (surfNamePar)) as GameObject;
			Vector3 refPos = player.transform.position;
			floor.transform.position = new Vector3 (refPos.x - 2.39f, refPos.y - 4.7f, refPos.z + 4f); 
			floor.transform.localScale = new Vector3 (length * 0.25f, 1f, width * 0.25f);
            player.AddComponent<CharCtrl>();
		} 
		else
        {
			floor.transform.localScale = new Vector3 (length * 0.25f, 1f, width * 0.25f);
		}
		CharCtrl charController = player.GetComponent<CharCtrl>();
		charController.speed =  (length + width)/2f * 0.25f * 7f;   //ideally make speed a setting for a user 
		Debug.Log (charController.speed); 
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
		serializer.Serialize (stream, bHolder);
		stream.Close ();
	}

    public void loadMap()
    {
        if (bHolder.list.Count != 0)
        {
            // TODO: warn the user, prompt for a decision
            // TODO: remove all created game objects
        }
        // TODO: path hardcoded, ask the user
        XmlSerializer serializer = new XmlSerializer(typeof(BoxHolder));
        FileStream stream = new FileStream(Application.dataPath + "/Resources/StreamingFiles/XML/boxes.xml", FileMode.Open);
        bHolder = serializer.Deserialize(stream) as BoxHolder;
        stream.Close();

        createGrid();
        foreach (List<List<BoxEntry>> column in bHolder.list) // x
        {
            foreach (List<BoxEntry> pillar in column) // z
            {
                foreach (BoxEntry boxEntry in pillar) // y
                {
                    //BoxEntry boxEntry = new BoxEntry(curTypeToName, "", xInd, yInd + 1, zInd);
                    MapPlacements mapPlacements = new MapPlacements();
                    // START FROM HERE NEXT TIME!
                    //GameObject box = mapPlacements.placeBox(boxEntry, boxEntry.x, refPos.y, refPos.z, 0.5f);
                    //mapPlacements.placeCell(boxEntry.type, xInd, yInd + 1, zInd, 1.01f, refPos).transform.parent = box.transform;
                }
            }
        }
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
            bHolder.list.Add(new List<List<BoxEntry>>());
            for (int j = 0; j < width; j++)
            {
                bHolder.list[i].Add(new List<BoxEntry>());
            }
        }
    }

}

public class BoxHolder
{
    public int length = 0;
    public int width = 0;
	public List<List<List<BoxEntry>>> list = new List<List<List<BoxEntry>>>();
}

