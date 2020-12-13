using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Tool")]
public class Tool : ScriptableObject
{
    public string name_;
    public ObjectTypes.ToolTypes type_;
    public GameObject prefab_;
}
