﻿using UnityEngine;

public static class DebugMethods
{
    public static void PrintPillar(int xInd, int zInd)
    {
        Debug.Log("===============================================");
        foreach (BoxEntry boxEntry in BHWrapper.bHolder.list[xInd][zInd])
        {
            Debug.Log(boxEntry.ToString());
        }
        Debug.Log("===============================================");
    }
}
