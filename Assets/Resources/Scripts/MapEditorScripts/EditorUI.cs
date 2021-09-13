using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;
using System.IO;


public class EditorUI : MonoBehaviour
{
    private class XZCoords
    {
        public float XPos_ { get; } = 0.0f;
        public float ZPos_ { get; } = 0.0f;


        public XZCoords(float xPos, float zPos)
        {
            XPos_ = xPos;
            ZPos_ = zPos;
        }
    }

    public GameObject floor;
    public bool buildingEnabled;

    public bool finishBoxPlaced;
    public int finishBoxXInd;
    public int finishBoxYInd;
    public int finishBoxZInd;

    public bool startBoxSelecting;
    public bool startBoxChosen;
    public int startBoxXInd;
    public int startBoxYInd;
    public int startBoxZInd;

    private bool gridGenerated = false;

    private GameObject player = null;
    private BoxPicking boxPicking = null;

    private List<List<XZCoords>> cellXZCoordsByIndices = new List<List<XZCoords>>();

    private InputField XIdxBox_;
    private InputField ZIdxBox_;
    private InputField YIdxBox_;

    public InputField saveLevelNameBox_;
    public InputField loadLevelNameBox_;

    public GameObject levelSavingPanel_;
    private string tempSaveLevelName_;
    private string curSaveLevelName_;

    private InputField analysisCountBox_;
    private InputField spaceWarpCountBox_;
    private InputField synthesisCountBox_;
    private InputField levitatorCountBox_;

    void Start()
    {
        // TODO: initialize everything else
        startBoxSelecting = false;
        tempSaveLevelName_ = "";

        player = GameObject.FindGameObjectWithTag("Player");
        player.AddComponent<CharCtrl>();

        boxPicking = GameObject.Find(GlobalVariables.canvasName).GetComponent<BoxPicking>();

        XIdxBox_ = GameObject.Find(GlobalVariables.XIdxBoxInputFieldName).GetComponent<InputField>();
        ZIdxBox_ = GameObject.Find(GlobalVariables.ZIdxBoxInputFieldName).GetComponent<InputField>();
        YIdxBox_ = GameObject.Find(GlobalVariables.YIdxBoxInputFieldName).GetComponent<InputField>();

        analysisCountBox_ = GameObject.Find(GlobalVariables.analysisCountBoxInputFieldName).GetComponent<InputField>();
        spaceWarpCountBox_ = GameObject.Find(GlobalVariables.spaceWarpBoxInputFieldName).GetComponent<InputField>();
        synthesisCountBox_ = GameObject.Find(GlobalVariables.synthesisCountBoxInputFieldName).GetComponent<InputField>();
        levitatorCountBox_ = GameObject.Find(GlobalVariables.levitatorBoxInputFieldName).GetComponent<InputField>();
    }

    public void parseAndCreateGrid()
    {
        InputField LBox = GameObject.Find("LBox").GetComponent<InputField>();
        InputField WBox = GameObject.Find("WBox").GetComponent<InputField>();
        BHWrapper.bHolder.length = int.Parse(LBox.text); // TODO: input format check/sanitization needed
        BHWrapper.bHolder.width = int.Parse(WBox.text);
        createGrid();
    }

    public void createGrid(bool fillBoxList = true)
    {
        floor = setUpPlane (BHWrapper.bHolder.length, BHWrapper.bHolder.width, "Floor");
		setUpCells (BHWrapper.bHolder.length, BHWrapper.bHolder.width, floor);

        if (fillBoxList)
        {
            fillList(BHWrapper.bHolder.length, BHWrapper.bHolder.width);
        }

        gridGenerated = true;

        Debug.Log (BHWrapper.bHolder.list.Count);
		Debug.Log (BHWrapper.bHolder.list[0].Count);
		Debug.Log (BHWrapper.bHolder.list[0][0].Count);
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
            cellXZCoordsByIndices.Add(new List<XZCoords>());
            for (int j = 0; j < width; j++) {
				GameObject cell = Instantiate (Resources.Load ("InitCell")) as GameObject;
                cell.transform.parent = floor.transform;
                cell.transform.position = new Vector3 (orix, floor.transform.position.y + GlobalDimensions.minDifDistance_, oriz);
				oriz += 2f;
				cell.tag = "Cell";

				InitPosBeh initPosBeh = cell.GetComponent<InitPosBeh> ();
				initPosBeh.xInd = i;
				initPosBeh.zInd = j;
				initPosBeh.yInd = -1;

                cellXZCoordsByIndices[i].Add(new XZCoords(cell.transform.position.x, cell.transform.position.z));
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

    public void confirmBoxLoading(GameObject panel)
    {
        string curLoadLevelName = loadLevelNameBox_.text;
        string fullPath = Application.dataPath + GlobalVariables.levelsSubpath + curLoadLevelName + GlobalVariables.levelFileExtension;
        if (File.Exists(fullPath))
        {
            loadMap(curLoadLevelName);
            cancelAction(panel);
        }
    }

    public void confirmBoxSaving(GameObject overwritePanel)
    {
        tempSaveLevelName_ = saveLevelNameBox_.text;
        foreach (char c in tempSaveLevelName_)
        {
            if (!Char.IsLetterOrDigit(c) && c != '!' && c !='?' && c !='_' && c != '-')
            {
                return;
            }
        }
        string fullPath = Application.dataPath + GlobalVariables.levelsSubpath + tempSaveLevelName_ + GlobalVariables.levelFileExtension;
        if (File.Exists(fullPath))
        {
            preAction(overwritePanel);
        }
        else
        {
            cancelAction(levelSavingPanel_);
            saveBoxes(tempSaveLevelName_);
            curSaveLevelName_ = tempSaveLevelName_;
        }
    }

    public void saveCurOpenLevel()
    {
        if (!String.IsNullOrEmpty(curSaveLevelName_))
        {
            saveBoxes(curSaveLevelName_);
        }
    }

    // TOOD: seems like a very specific method, rename
    public void saveBoxesFromVar(GameObject panel)
    {
        saveBoxes(tempSaveLevelName_);
        curSaveLevelName_ = tempSaveLevelName_;

        cancelAction(panel);
        cancelAction(levelSavingPanel_);
    }

	public void saveBoxes(string saveLevelName)
    {
        AddFundamentToEmptyPillars();

        ValidateAndTransferLevelData();

		XmlSerializer serializer = new XmlSerializer(typeof(BoxHolder));
		FileStream stream = new FileStream (Application.dataPath + GlobalVariables.levelsSubpath + saveLevelName + GlobalVariables.levelFileExtension, FileMode.Create);
		serializer.Serialize (stream, BHWrapper.bHolder);
		stream.Close ();
	}

    private void AddFundamentToEmptyPillars()
    {
        for (int i = 0; i < BHWrapper.bHolder.length; ++i) // x
        {
            for (int j = 0; j < BHWrapper.bHolder.width; ++j) // z
            {
                if (BHWrapper.bHolder.list[i][j].Count == 0)
                {
                    // placing an empty box
                    BoxEntry emptyBoxEntry = new BoxEntry(ObjectTypes.BoxTypes.Undetermined, "", i, 0, j,
                        cellXZCoordsByIndices[i][j].XPos_, floor.transform.position.y + GlobalDimensions.halfMargin_, cellXZCoordsByIndices[i][j].ZPos_);
                    BHWrapper.bHolder.list[i][j].Add(emptyBoxEntry);
                }
            }
        }
    }

    private void ValidateAndTransferLevelData()
    {
        BHWrapper.bHolder.finishGiven = finishBoxPlaced;
        BHWrapper.bHolder.finishXInd = finishBoxXInd;
        BHWrapper.bHolder.finishYInd = finishBoxYInd;
        BHWrapper.bHolder.finishZInd = finishBoxZInd;

        if (startBoxChosen)
        {
            List<BoxEntry> pillar = BHWrapper.bHolder.list[startBoxXInd][startBoxZInd];
            // NOTE: BoxBehavior's IsTopInPillar and GetUpperBoxEntry might come in handy here
            // TODO: need to change the first condition if the concept of ceiling is introduced
            // this check is for making sure there is space for player to be placed on the box marked as start
            if (startBoxYInd < pillar.Count - 1 && pillar[startBoxYInd + 1].type != ObjectTypes.BoxTypes.Undetermined)
            {
                startBoxChosen = false;
            }
        }
        BHWrapper.bHolder.startGiven = startBoxChosen;
        BHWrapper.bHolder.startXInd = startBoxXInd;
        BHWrapper.bHolder.startYInd = startBoxYInd;
        BHWrapper.bHolder.startZInd = startBoxZInd;

        if (!String.IsNullOrEmpty(analysisCountBox_.text))
        {
            BHWrapper.bHolder.analysisCount = int.Parse(analysisCountBox_.text);
        }
        if (!String.IsNullOrEmpty(spaceWarpCountBox_.text))
        {
            BHWrapper.bHolder.spaceWarpCount = int.Parse(spaceWarpCountBox_.text);
        }
        if (!String.IsNullOrEmpty(synthesisCountBox_.text))
        {
            BHWrapper.bHolder.synthesisCount = int.Parse(synthesisCountBox_.text);
        }
        if (!String.IsNullOrEmpty(levitatorCountBox_.text))
        {
            BHWrapper.bHolder.levitatorCount = int.Parse(levitatorCountBox_.text);
        }
    }

    private void TransferLevelDataBack()
    {
        finishBoxPlaced = BHWrapper.bHolder.finishGiven;
        if (finishBoxPlaced)
        {
            finishBoxXInd = BHWrapper.bHolder.finishXInd;
            finishBoxYInd = BHWrapper.bHolder.finishYInd;
            finishBoxZInd = BHWrapper.bHolder.finishZInd;
        }

        startBoxChosen = BHWrapper.bHolder.startGiven;
        if (startBoxChosen)
        {
            startBoxXInd = BHWrapper.bHolder.startXInd;
            startBoxYInd = BHWrapper.bHolder.startYInd;
            startBoxZInd = BHWrapper.bHolder.startZInd;

            BoxEntry curStartBoxEntry = BHWrapper.bHolder.list[startBoxXInd][startBoxZInd][startBoxYInd];
            curStartBoxEntry.GetBoxGameObj().GetComponent<Renderer>().material =
                Resources.Load("Materials/" + ObjectTypes.boxTypesToStartMaterialNames[curStartBoxEntry.type], typeof(Material)) as Material;
        }

        analysisCountBox_.text = BHWrapper.bHolder.analysisCount.ToString();
        spaceWarpCountBox_.text = BHWrapper.bHolder.spaceWarpCount.ToString();
        synthesisCountBox_.text = BHWrapper.bHolder.synthesisCount.ToString();
        levitatorCountBox_.text = BHWrapper.bHolder.levitatorCount.ToString();
    }

    public void loadMap(string loadLevelName)
    {
        if (BHWrapper.bHolder.list.Count != 0)
        {
            // TODO: warn the user, prompt for a decision
            clearUserCreatedObjects();
        }
        // TODO: path hardcoded, ask the user
        XmlSerializer serializer = new XmlSerializer(typeof(BoxHolder));
        FileStream stream = new FileStream(Application.dataPath + GlobalVariables.levelsSubpath + loadLevelName + GlobalVariables.levelFileExtension, FileMode.Open);
        BHWrapper.bHolder = serializer.Deserialize(stream) as BoxHolder;
        stream.Close();

        createGrid(false);

        foreach (List<List<BoxEntry>> column in BHWrapper.bHolder.list) // x
        {
            foreach (List<BoxEntry> pillar in column) // z
            {
                foreach (BoxEntry boxEntry in pillar) // y
                {
                    if (boxEntry.type != ObjectTypes.BoxTypes.Undetermined)
                    {
                        MapPlacements mapPlacements = new MapPlacements();
                        GameObject box = mapPlacements.placeBox(boxEntry, boxEntry.xPos, boxEntry.yPos, boxEntry.zPos, 0f, GlobalVariables.mapEditorBoxesName);
                        boxEntry.SetBoxGameObj(box);
                        mapPlacements.placeCell(boxEntry.type, boxEntry.xInd, boxEntry.yInd, boxEntry.zInd,
                           GlobalDimensions.halfMargin_ + GlobalDimensions.minDifDistance_, box.transform.position).transform.parent = box.transform;
                    }
                }
            }
        }

        curSaveLevelName_ = loadLevelName;
        TransferLevelDataBack();
    }

    private void clearUserCreatedObjects()
    {
        // removing the all boxes
        foreach (List<List<BoxEntry>> column in BHWrapper.bHolder.list)
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
        finishBoxPlaced = false;
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

    private void SetStartBox()
    {
        startBoxSelecting = false;

        int startBoxXIndBuff = int.Parse(XIdxBox_.text);
        int startBoxZIndBuff = int.Parse(ZIdxBox_.text);
        int startBoxYIndBuff = int.Parse(YIdxBox_.text);

        bool startBoxApproved = false;

        if  (startBoxXIndBuff <= BHWrapper.bHolder.list.Count - 1)
        {
            if (startBoxZIndBuff <= BHWrapper.bHolder.list[startBoxXIndBuff].Count - 1)
            {
                if (startBoxYIndBuff <= BHWrapper.bHolder.list[startBoxXIndBuff][startBoxZIndBuff].Count - 1)
                {
                    if (startBoxXIndBuff != finishBoxXInd || startBoxZIndBuff != finishBoxZInd || startBoxYIndBuff != finishBoxYInd)
                    {
                        startBoxApproved = true;
                    }
                }
            }
        }

        if (startBoxApproved)
        {
            ConfirmStartBoxChoosing(startBoxXIndBuff, startBoxYIndBuff, startBoxZIndBuff);
        }
    }

    private void SelectStartBox()
    {
        startBoxSelecting = true;
        boxPicking.DeactivateCurSlot();  
    }

    public void ConfirmStartBoxChoosing(int startBoxXIndPar, int startBoxYIndPar, int startBoxZIndPar)
    {
        if (startBoxChosen)
        {
            BoxEntry curStartBoxEntry = BHWrapper.bHolder.list[startBoxXInd][startBoxZInd][startBoxYInd];
            curStartBoxEntry.GetBoxGameObj().GetComponent<Renderer>().material =
                Resources.Load("Materials/" + ObjectTypes.boxTypesToMaterialNames[curStartBoxEntry.type], typeof(Material)) as Material;
        }

        startBoxXInd = startBoxXIndPar;
        startBoxYInd = startBoxYIndPar;
        startBoxZInd = startBoxZIndPar;

        BoxEntry startBoxEntry = BHWrapper.bHolder.list[startBoxXInd][startBoxZInd][startBoxYInd];
        startBoxEntry.GetBoxGameObj().GetComponent<Renderer>().material =
            Resources.Load("Materials/" + ObjectTypes.boxTypesToStartMaterialNames[startBoxEntry.type], typeof(Material)) as Material;

        startBoxChosen = true;
    }

    public bool GetGridGenerated()
    {
        return gridGenerated;
    }

    private void fillList(int length, int width)
    {
        for (int i = 0; i < length; i++)
        {
            BHWrapper.bHolder.list.Add(new List<List<BoxEntry>>());
            for (int j = 0; j < width; j++)
            {
                BHWrapper.bHolder.list[i].Add(new List<BoxEntry>());
            }
        }
    }
}

