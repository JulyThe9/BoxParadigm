using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Tool")]
public class Tool : ScriptableObject
{
    public string name_;
    public ObjectTypes.ToolTypes type_;
    public bool doubleHanded_;
    public GameObject prefab_;
    public GameObject leftProjectilePrefab_;
    public GameObject rightProjectilePrefab_;
}
