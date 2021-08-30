using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // TODO: see where else can be used
    public static void RemoveFromPillar(int xInd, int zInd, int yIndToRemove)
    {
        bHolder.list[xInd][zInd].RemoveAt(yIndToRemove);
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
