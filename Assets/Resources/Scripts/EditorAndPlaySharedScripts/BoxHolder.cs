using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoxHolderWrapper
{
    public static BoxHolder bHolder = new BoxHolder();
}

public class BoxHolder
{
    public int length = 0;
    public int width = 0;
    public List<List<List<BoxEntry>>> list = new List<List<List<BoxEntry>>>();
}
