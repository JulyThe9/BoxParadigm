using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class LevelLoader : MonoBehaviour
{

    private GameObject player = null;

    void Start()
    {
        loadMap();
        loadPlayer();
    }

    private void loadMap()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(BoxHolder));
        FileStream stream = new FileStream(Application.dataPath + "/Resources/StreamingFiles/XML/boxes.xml", FileMode.Open);
        BHWrapper.bHolder = serializer.Deserialize(stream) as BoxHolder;
        stream.Close();

        foreach (List<List<BoxEntry>> column in BHWrapper.bHolder.list) // x
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
                        GameObject box = mapPlacements.placeBox(boxEntry, boxEntry.xPos, boxEntry.yPos, boxEntry.zPos, 0f);
                        box.layer = LayerMask.NameToLayer("Ground");
                        boxEntry.SetBoxGameObj(box);

                        if (i + 1 >= pillar.Count || pillar[i+1].type == ObjectTypes.BoxTypes.Undetermined)
                        {
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
    }

    private void loadPlayer()
    {
        if (!BHWrapper.bHolder.startGiven)
        {
            findHighestBox();
        }
        GameObject startBox =
            BHWrapper.bHolder.list[BHWrapper.bHolder.startXInd][BHWrapper.bHolder.startZInd][BHWrapper.bHolder.startYInd].GetBoxGameObj();

        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(startBox.transform.position.x, 
            startBox.transform.position.y + GlobalDimensions.margin_ + GlobalDimensions.minDifDistance_, startBox.transform.position.z);
    }
    private void findHighestBox()
    {
        int xInd = 0;
        int zInd = 0;
        int maxPillarHeight = 0;
        foreach (List<List<BoxEntry>> column in BHWrapper.bHolder.list) // x
        {
            foreach (List<BoxEntry> pillar in column) // z
            {
                if (pillar.Count > maxPillarHeight)
                {
                    maxPillarHeight = pillar.Count;
                    xInd = pillar[0].xInd;
                    zInd = pillar[0].zInd;
                }
            }
        }
        BHWrapper.bHolder.startXInd = xInd;
        BHWrapper.bHolder.startZInd = zInd;
        BHWrapper.bHolder.startYInd = maxPillarHeight - 1;
    }
}
