using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BHWrapper
{
    public static BoxHolder bHolder = new BoxHolder();

    public static void ClearBoxEntry(int xInd, int zInd, int yInd)
    {
        bHolder.list[xInd][zInd][yInd].Clear();
    }

    public static void UpdateBoxEntry(int xInd, int zInd, int yInd, BoxEntry boxEntry)
    {
        bHolder.list[xInd][zInd][yInd].Update(boxEntry);
    }

    // TODO: see where else can be used
    public static void RemoveFromPillar(int xInd, int zInd, int yIndToRemove)
    {
        bHolder.list[xInd][zInd].RemoveAt(yIndToRemove);
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
