using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public static class LevelManager
{
    public static void DeserializeLevelFiles()
    {
        string premadeLevelPath = Application.dataPath + GlobalVariables.premadeLevelsSubpath;
        DirectoryInfo info = new DirectoryInfo(Application.dataPath + GlobalVariables.premadeLevelsSubpath);
        FileInfo[] levelFiles = info.GetFiles();
        int maxLegitLevelNum = levelFiles.Length;

        XmlSerializer serializer = new XmlSerializer(typeof(BoxHolder));

        for (int i = 1; i <= maxLegitLevelNum; ++i)
        {
            for (int fileIdx = 0; fileIdx < levelFiles.Length; ++fileIdx)
            {
                if (levelFiles[fileIdx].Name[0].ToString() == i.ToString() &&
                    Path.GetExtension(levelFiles[fileIdx].Name) == GlobalVariables.levelFileExtension)
                {
                    FileStream stream = new FileStream(premadeLevelPath + levelFiles[fileIdx].Name, FileMode.Open);
                    BoxHolder curLevel = serializer.Deserialize(stream) as BoxHolder;
                    if (curLevel.levelPlayable)
                    {
                        BHWrapper.BHoldersAdd(curLevel);
                    }
                    stream.Close();
                    break;
                }
            }
        }
    }
}
