using UnityEngine;

public class BoxData : MonoBehaviour
{
    // TODO: make private back once stable
    public int _xInd = 0;
    public int _yInd = 0;
    public int _zInd = 0;

    public int xInd
    {
        get { return _xInd; }
        set { _xInd = value; }
    }

    public int yInd
    {
        get { return _yInd; }
        set { _yInd = value; }
    }

    public int zInd
    {
        get { return _zInd; }
        set { _zInd = value; }
    }

    public BoxData ShallowCopy()
    {
        BoxData boxData = new BoxData();
        boxData.xInd = xInd;
        boxData.yInd = yInd;
        boxData.zInd = zInd;
        return boxData;
    }

    public void UpdateBoxData(BoxData otherBoxData)
    {
        xInd = otherBoxData.xInd;
        yInd = otherBoxData.yInd;
        zInd = otherBoxData.zInd;
    }
}
