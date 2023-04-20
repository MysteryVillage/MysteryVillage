using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "River", menuName = "New River")]
public class River : ScriptableObject
{
    public string name;
    public List<RiverCheckpointData> checkpoints;
}

[System.Serializable]
public class RiverCheckpointData
{
    public float x;
    public float y;
    public float z;
    public float width;
    public float rotation;
}
