using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewBlueprint", menuName = "Puzzle/Blueprint")]
public class Blueprint : ScriptableObject
{
    public List<BlockData> blocks;
}

[System.Serializable]
public struct BlockData
{
    public Vector3Int position;
    public Quaternion rotation;
    public string blockID;
}

