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

	public void createGrid() {
		InputField LBox = GameObject.Find ("LBox").GetComponent<InputField>();
		InputField WBox = GameObject.Find ("WBox").GetComponent<InputField>();
		float length = float.Parse(LBox.text);
		float width = float.Parse(WBox.text);

		//surfName = surfNamePar+"(Clone)";
		floor = setUpPlane (length, width, "Floor");
		setUpCells (length, width, floor);

		fillList((int) length, (int) width);
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
			Vector3 refPos = GameObject.FindGameObjectWithTag ("Player").transform.position;
			floor.transform.position = new Vector3 (refPos.x - 2.39f, refPos.y - 4.7f, refPos.z + 4f); 
			floor.transform.localScale = new Vector3 (length * 0.25f, 1f, width * 0.25f);
			GameObject.FindGameObjectWithTag ("Player").AddComponent<CharCtrl>();
		} 
		else
        {
			floor.transform.localScale = new Vector3 (length * 0.25f, 1f, width * 0.25f);
		}
		CharCtrl charController = GameObject.FindWithTag ("Player").GetComponent<CharCtrl>();
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

	public void preAction(GameObject panel)
    {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<CharCtrl>().enabled = false;
		panel.SetActive (true);
	}

	public void cancelAction(GameObject panel)
    {
		GameObject.FindGameObjectWithTag ("Player").GetComponent<CharCtrl>().enabled = true;
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
	public List<List<List<BoxEntry>>> list = new List<List<List<BoxEntry>>>();
}

