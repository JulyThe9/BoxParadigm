using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// TODO: might want to add range changes for all methods
public static class BHWrapper
{
    public static BoxHolder bHolder = new BoxHolder();

    // TODO: find out how refs work
    public static BoxEntry GetBoxEntry(int xInd, int zInd, int yInd)
    {
        return bHolder.list[xInd][zInd][yInd];
    }

    // TODO: see where else this can be used
    public static bool BoxExists(int xInd, int zInd, int yInd)
    {
        if (bHolder.list[xInd][zInd].Count - 1 < yInd || bHolder.list[xInd][zInd][yInd].type == ObjectTypes.BoxTypes.Undetermined)
        {
            return false;
        }
        return true;
    }

    public static void ClearBoxEntry(int xInd, int zInd, int yInd)
    {
        bHolder.list[xInd][zInd][yInd].Clear();
    }

    public static void UpdateBoxEntry(int xInd, int zInd, int yInd, BoxEntry boxEntry)
    {
        // NOTE: temporary for debugging
        if (xInd > bHolder.list.Count - 1)
        {
            return;
        }
        if (zInd > bHolder.list[xInd].Count - 1)
        {
            return; 
        }
        if (yInd > bHolder.list[xInd][zInd].Count - 1)
        {
            return;
        }
        bHolder.list[xInd][zInd][yInd].Update(boxEntry);
    }

    // TODO: see where else can be used; ALWAYS USE THIS TO REMOVE to preserve the logic with never having an empty pillar
    public static void RemoveFromPillar(int xInd, int zInd, int yIndToRemove)
    {
        if (yIndToRemove > 0)
        {
            bHolder.list[xInd][zInd].RemoveAt(yIndToRemove);
        }
        else
        {
            bHolder.list[xInd][zInd][yIndToRemove].Clear();
        }
    }

    public static void FillWithEmpty(int xInd, int zInd, int yIndFinal) // yIndFinal is inclusive
    {
        Vector3 refPos = new Vector3(0, 0, 0);
        List<BoxEntry> pillar = bHolder.list[xInd][zInd];

        Debug.Assert(pillar.Count > 0);
        if (pillar.Count > 0)
        {
            BoxEntry topBox = pillar.Last();
            refPos.x = topBox.xPos;
            refPos.y = topBox.yPos;
            refPos.z = topBox.zPos;
        }

        while (pillar.Count <= yIndFinal)
        {
            BoxEntry boxEntry = new BoxEntry(ObjectTypes.BoxTypes.Undetermined, "", xInd, pillar.Count, zInd, 
                refPos.x, refPos.y + GlobalDimensions.margin_, refPos.z);
            pillar.Add(boxEntry);
            refPos.y = boxEntry.yPos;
        }
    }

    public static bool CheckIntegrity()
    {
        bool res = true;
        for (int i = 0; i < bHolder.list.Count; ++i) // x
        {
            for (int j = 0; j < bHolder.list[i].Count; ++j) // z
            {
                for (int k = 0; k < bHolder.list[i][j].Count; ++k) // y
                {
                    BoxEntry boxEntry = bHolder.list[i][j][k];
                    if (boxEntry.xInd != i || boxEntry.zInd != j || boxEntry.yInd != k)
                    {
                        Debug.Log("INTEGRITY CHECK FAILED: BoxEntry index mismatch: " + boxEntry.xInd + i + " " + boxEntry.zInd + j + " " + boxEntry.yInd + k);
                        res = false;
                    }
                    GameObject boxGameObject = boxEntry.GetBoxGameObj();
                    if (boxGameObject != null)
                    {
                        BoxData boxData = boxGameObject.GetComponent<BoxData>();
                        if (boxData.xInd != i || boxData.zInd != j || boxData.yInd != k)
                        {
                            Debug.Log("INTEGRITY CHECK FAILED: BoxData index mismatch: " + boxData.xInd + i + " " + boxData.zInd + j + " " + boxData.yInd + k);
                            res = false;
                        }
                    }
                    // TODO: boxEntry from list compare to boxEntry from BoxBehavior of boxGameObject
                    // TODO: boxData as boxGameObject.GetComponent<BoxData>() compare to boxData from BoxBehavior of boxGameObject 
                }
            }
        }
        Debug.Log("INTEGRITY CHECK SUCCEEDED");
        return res;
    }
}

public class BoxHolder
{
    public int length = 0;
    public int width = 0;

    public int startXInd = 0;
    public int startYInd = 0;
    public int startZInd = 0;
    public bool startGiven = false;

    public int finishXInd = 0;
    public int finishYInd = 0;
    public int finishZInd = 0;
    public bool finishGiven = false;

    public List<List<List<BoxEntry>>> list = new List<List<List<BoxEntry>>>();
}
