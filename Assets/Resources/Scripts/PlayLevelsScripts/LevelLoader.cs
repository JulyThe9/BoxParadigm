﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class LevelLoader : MonoBehaviour
{
    private GameObject player = null;
    private ToolControl toolControl = null;

    void Start()
    {
        loadWalls();
        loadMap();
        loadPlayer();
        loadToolCounts();
    }

    private void loadWalls()
    {
        // TODO: temp
        if (BHWrapper.GetBHolders().Count == 0)
        {
            return;
        }

        // base wall
        GameObject baseWall = GameObject.Instantiate(Resources.Load(GlobalVariables.environmentObjPath + "/" + ObjectTypes.baseWallName)) as GameObject;
        baseWall.name = ObjectTypes.baseWallName;
        Vector3 baseWallLS = baseWall.transform.localScale;
        baseWallLS.x = (BHWrapper.BHolder().length * 2 - 1) + GlobalDimensions.levelMarginInBoxes_;
        baseWallLS.y = (BHWrapper.BHolder().width * 2 - 1) + GlobalDimensions.levelMarginInBoxes_;
        baseWall.transform.localScale = baseWallLS;

        GlobalDimensions.halfFloorThickness_ = baseWallLS.z / 2f;
        baseWall.transform.position = new Vector3(0f, 0f - GlobalDimensions.halfFloorThickness_, 0f);

        // top wall
        GameObject topWall = GameObject.Instantiate(Resources.Load(GlobalVariables.environmentObjPath + "/" + ObjectTypes.topWallName)) as GameObject;
        topWall.name = ObjectTypes.topWallName;
        topWall.transform.localScale = baseWallLS;

        int height = findMaxPillarHeight();
        topWall.transform.position = baseWall.transform.position;
        float topWallDist = height * GlobalDimensions.margin_ + 2 * GlobalDimensions.playerSize_ + 2 * GlobalDimensions.halfFloorThickness_;
        Vector3 topWallIncr = new Vector3(0f, topWallDist, 0f);
        topWall.transform.position += topWallIncr;


        // left wall
        GameObject leftWall = GameObject.Instantiate(Resources.Load(GlobalVariables.environmentObjPath + "/" + ObjectTypes.leftWallName)) as GameObject;
        leftWall.name = ObjectTypes.leftWallName;
        leftWall.transform.localScale = baseWallLS;
        leftWall.transform.Rotate(-90f, 0f, 0f);
        leftWall.transform.Rotate(0f, 90f, 0f);
        leftWall.transform.Rotate(0f, 0f, -90f);

        Vector3 leftWallLS = leftWall.transform.localScale;
        leftWallLS.x = topWallDist - 2 * GlobalDimensions.halfFloorThickness_;
        leftWall.transform.localScale = leftWallLS;

        leftWall.transform.position = baseWall.transform.position;
        Vector3 leftWallIncr = new Vector3(topWall.transform.localScale.x / 2 - GlobalDimensions.halfFloorThickness_, -topWallDist / 2, 0f);
        leftWall.transform.position -= leftWallIncr;

        // right wall
        GameObject rightWall = GameObject.Instantiate(Resources.Load(GlobalVariables.environmentObjPath + "/" + ObjectTypes.rightWallName)) as GameObject;
        rightWall.name = ObjectTypes.rightWallName;
        rightWall.transform.localScale = leftWallLS;
        rightWall.transform.Rotate(-90f, 0f, 0f);
        rightWall.transform.Rotate(0f, 90f, 0f);
        rightWall.transform.Rotate(0f, 0f, -90f);

        rightWall.transform.position = leftWall.transform.position;
        Vector3 rightWallIncr = new Vector3(topWall.transform.localScale.x - 2 * GlobalDimensions.halfFloorThickness_, 0f, 0f);
        rightWall.transform.position += rightWallIncr;


        // back wall
        GameObject backWall = GameObject.Instantiate(Resources.Load(GlobalVariables.environmentObjPath + "/" + ObjectTypes.backWallName)) as GameObject;
        backWall.name = ObjectTypes.backWallName;
        backWall.transform.localScale = baseWallLS;
        backWall.transform.Rotate(-90f, 0f, 0f);

        Vector3 backWallLS = backWall.transform.localScale;
        backWallLS.x -= 4 * GlobalDimensions.halfFloorThickness_;
        backWallLS.y = topWallDist - 2 * GlobalDimensions.halfFloorThickness_;
        backWall.transform.localScale = backWallLS;

        backWall.transform.position = baseWall.transform.position;
        Vector3 backWallIncr = new Vector3(0f, topWallDist / 2, topWall.transform.localScale.y / 2 - GlobalDimensions.halfFloorThickness_);
        backWall.transform.position += backWallIncr;

        // front wall
        GameObject frontWall = GameObject.Instantiate(Resources.Load(GlobalVariables.environmentObjPath + "/" + ObjectTypes.frontWallName)) as GameObject;
        frontWall.name = ObjectTypes.frontWallName;
        frontWall.transform.localScale = backWallLS;
        frontWall.transform.Rotate(-90f, 0f, 0f);

        frontWall.transform.position = backWall.transform.position;
        Vector3 frontWallIncr = new Vector3(0f, 0f, topWall.transform.localScale.y - 2 * GlobalDimensions.halfFloorThickness_);
        frontWall.transform.position -= frontWallIncr;
    }

    private void loadMap()
    {
        if (BHWrapper.GetBHolders().Count == 0)
        {
            // TODO: temp
            BHWrapper.AddEmptyLevel();

            XmlSerializer serializer = new XmlSerializer(typeof(BoxHolder));
            FileStream stream = new FileStream(Application.dataPath + GlobalVariables.premadeLevelsSubpath + "2boxes.xml", FileMode.Open);
            BHWrapper.BHolderSet(serializer.Deserialize(stream) as BoxHolder);
            stream.Close();

            loadWalls();
        }

        foreach (List<List<BoxEntry>> column in BHWrapper.BHolder().list) // x
        {
            foreach (List<BoxEntry> pillar in column) // z
            {
                //  foreach (BoxEntry boxEntry in pillar) // y
                for (int i = 0; i < pillar.Count; ++i)
                {
                    BoxEntry boxEntry = pillar[i];
                    if (boxEntry.type != ObjectTypes.BoxTypes.Undetermined)
                    {
                        MapPlacements mapPlacements = new MapPlacements();
                        GameObject box = mapPlacements.placeBox(boxEntry, boxEntry.xPos, boxEntry.yPos, boxEntry.zPos, 0f, GlobalVariables.playLevelsBoxesName);
                        box.layer = LayerMask.NameToLayer("Ground");
                        boxEntry.SetBoxGameObj(box);

                        // TODO: check how efficient it is in larger levels
                        GameObject ledgeToGrab = GameObject.Instantiate(Resources.Load(ObjectTypes.ledgeToGrabName)) as GameObject;
                        ledgeToGrab.transform.position = new Vector3(boxEntry.xPos, boxEntry.yPos + GlobalDimensions.marginToLedge_, boxEntry.zPos); 
                        ledgeToGrab.transform.Rotate(boxEntry.xRot, boxEntry.yRot, 0f);
                        ledgeToGrab.transform.parent = box.transform;
                        ledgeToGrab.name = ObjectTypes.ledgeToGrabName;
                    }
                }
            }
        }
    }

    private void loadPlayer()
    {
        if (!BHWrapper.BHolder().startGiven)
        {
            findHighestBox(out BHWrapper.BHolder().startXInd, out BHWrapper.BHolder().startZInd, out BHWrapper.BHolder().startYInd);
        }
        GameObject startBox =
            BHWrapper.BHolder().list[BHWrapper.BHolder().startXInd][BHWrapper.BHolder().startZInd][BHWrapper.BHolder().startYInd].GetBoxGameObj();

        player = GameObject.FindGameObjectWithTag(ObjectTypes.playerTagName);
        player.transform.position = new Vector3(startBox.transform.position.x, 
            startBox.transform.position.y + GlobalDimensions.margin_ + GlobalDimensions.minDifDistance_, startBox.transform.position.z);

        toolControl = player.GetComponent<ToolControl>();
    }

    private void loadToolCounts()
    {
        Debug.Assert(player != null);
        ToolControl toolControl = player.GetComponent<ToolControl>();

        toolControl.toolCounts[ObjectTypes.ToolTypes.Analysis] = BHWrapper.BHolder().analysisCount;
        toolControl.toolCounts[ObjectTypes.ToolTypes.SpaceWarp] = BHWrapper.BHolder().spaceWarpCount;
        toolControl.toolCounts[ObjectTypes.ToolTypes.Synthesis] = BHWrapper.BHolder().synthesisCount;
        toolControl.toolCounts[ObjectTypes.ToolTypes.Levitator] = BHWrapper.BHolder().levitatorCount;

        foreach (KeyValuePair<ObjectTypes.ToolTypes, int> toolCount in toolControl.toolCounts)
        {
            if (toolCount.Value == 0)
            {
                toolControl.TriggerToolDepletion(toolControl.GetToolIdxByType(toolCount.Key), true);
            }
        }
    }

    private void findHighestBox(out int xInd, out int zInd, out int yInd)
    {
        xInd = 0;
        zInd = 0;
        int maxPillarHeight = 0;
        foreach (List<List<BoxEntry>> column in BHWrapper.BHolder().list) // x
        {
            foreach (List<BoxEntry> pillar in column) // z
            {
                if (pillar.Count > maxPillarHeight)
                {
                    maxPillarHeight = pillar.Count;
                    xInd = pillar[0].xInd;
                    zInd = pillar[0].zInd;
                }
                pillar[pillar.Count - 1].topInPillar = true;
            }
        }
        yInd = maxPillarHeight - 1;
    }

    private int findMaxPillarHeight()
    {
        int maxPillarHeight = 0;
        foreach (List<List<BoxEntry>> column in BHWrapper.BHolder().list) // x
        {
            foreach (List<BoxEntry> pillar in column) // z
            {
                if (pillar.Count > maxPillarHeight)
                {
                    maxPillarHeight = pillar.Count;
                }
            }
        }
        return maxPillarHeight;
    }
}
