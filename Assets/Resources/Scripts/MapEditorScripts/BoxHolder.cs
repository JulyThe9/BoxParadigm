using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// TODO: might want to add range changes for all methods
public static class BHWrapper
{
    private static int curLevelIdx_ = 0;
    private static List<BoxHolder> bHolders_ = new List<BoxHolder>();
    private static BoxHolder bHolder;

    public static List<BoxHolder> GetBHolders()
    {
        return bHolders_;
    }

    public static BoxHolder BHolder()
    {
        Debug.Assert(curLevelIdx_ < bHolders_.Count);
        return bHolders_[curLevelIdx_];
    }

    public static void BHoldersAdd(BoxHolder bHolder)
    {
        bHolders_.Add(bHolder);
    }

    public static void BHoldersRemoveLast()
    {
        if (bHolders_.Count > 0)
        {
            bHolders_.RemoveAt(bHolders_.Count - 1);
        }
    }

    public static void BHoldersClear()
    {
        bHolders_.Clear();
    }

    public static void BHolderSet(BoxHolder bHolder)
    {
        bHolders_[curLevelIdx_] = bHolder; // TODO: copy?
    }

    public static void AddEmptyLevel()
    {
        bHolders_.Add(new BoxHolder());
    }

    // TODO: find out how refs work
    public static BoxEntry GetBoxEntry(int xInd, int zInd, int yInd)
    {
        return BHolder().list[xInd][zInd][yInd];
    }

    public static void IncreaseLevel()
    {
        ++curLevelIdx_;
    }

    public static void ResetCurLevel()
    {
        curLevelIdx_ = 0;
    }

    // TODO: see where else this can be used
    public static bool BoxExists(int xInd, int zInd, int yInd)
    {
        if (BHolder().list[xInd][zInd].Count - 1 < yInd || BHolder().list[xInd][zInd][yInd].type == ObjectTypes.BoxTypes.Undetermined)
        {
            return false;
        }
        return true;
    }

    public static void ClearBoxEntry(int xInd, int zInd, int yInd)
    {
        BHolder().list[xInd][zInd][yInd].Clear();
    }

    public static void UpdateBoxEntry(int xInd, int zInd, int yInd, BoxEntry boxEntry)
    {
        // NOTE: temporary for debugging
        if (xInd > BHolder().list.Count - 1)
        {
            return;
        }
        if (zInd > BHolder().list[xInd].Count - 1)
        {
            return; 
        }
        if (yInd > BHolder().list[xInd][zInd].Count - 1)
        {
            return;
        }
        BHolder().list[xInd][zInd][yInd].Update(boxEntry);
    }

    // TODO: see where else can be used; ALWAYS USE THIS TO REMOVE to preserve the logic with never having an empty pillar
    public static void RemoveFromPillar(int xInd, int zInd, int yIndToRemove)
    {
        if (yIndToRemove > 0)
        {
            BHolder().list[xInd][zInd].RemoveAt(yIndToRemove);
        }
        else
        {
            BHolder().list[xInd][zInd][yIndToRemove].Clear();
        }
    }

    public static void FillWithEmpty(int xInd, int zInd, int yIndFinal) // yIndFinal is inclusive
    {
        Vector3 refPos = new Vector3(0, 0, 0);
        List<BoxEntry> pillar = BHolder().list[xInd][zInd];

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

    public static bool IsLastLevel()
    {
        return curLevelIdx_ == bHolders_.Count - 1;
    }

    public static bool CheckIntegrity()
    {
        bool res = true;
        for (int i = 0; i < BHolder().list.Count; ++i) // x
        {
            for (int j = 0; j < BHolder().list[i].Count; ++j) // z
            {
                for (int k = 0; k < BHolder().list[i][j].Count; ++k) // y
                {
                    BoxEntry boxEntry = BHolder().list[i][j][k];
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

    public int analysisCount = 0;
    public int spaceWarpCount = 0;
    public int synthesisCount = 0;
    public int levitatorCount = 0;

    public bool levelPlayable = false;

    public List<List<List<BoxEntry>>> list = new List<List<List<BoxEntry>>>();
}
