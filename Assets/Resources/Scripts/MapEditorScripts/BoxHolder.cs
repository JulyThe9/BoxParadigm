using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BHWrapper
{
    public static BoxHolder bHolder = new BoxHolder();
}

public class BoxHolder
{
    public int length = 0;
    public int width = 0;

    public int startXInd = 0;
    public int startYInd = 0;
    public int startZInd = 0;
    public bool startGiven = false;

    public List<List<List<BoxEntry>>> list = new List<List<List<BoxEntry>>>();
}
