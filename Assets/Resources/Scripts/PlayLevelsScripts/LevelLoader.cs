using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class LevelLoader : MonoBehaviour
{
    void Start()
    {
        loadMap();
    }

    private void loadMap()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(BoxHolder));
        FileStream stream = new FileStream(Application.dataPath + "/Resources/StreamingFiles/XML/boxes.xml", FileMode.Open);
        BoxHolderWrapper.bHolder = serializer.Deserialize(stream) as BoxHolder;
        stream.Close();

        foreach (List<List<BoxEntry>> column in BoxHolderWrapper.bHolder.list) // x
        {
            foreach (List<BoxEntry> pillar in column) // z
            {
                foreach (BoxEntry boxEntry in pillar) // y
                {
                    if (boxEntry.type != ObjectTypes.BoxTypes.Undetermined)
                    {
                        MapPlacements mapPlacements = new MapPlacements();
                        GameObject box = mapPlacements.placeBox(boxEntry, boxEntry.xPos, boxEntry.yPos, boxEntry.zPos, 0f);
                        boxEntry.SetBoxGameObj(box);
                    }
                }
            }
        }
    }
}
