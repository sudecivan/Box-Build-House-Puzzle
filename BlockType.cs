using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BlockType", menuName = "Scriptable Objects/BlockType")]
public class BlockType : ScriptableObject
{
    public GameObject prefab;
    public GameObject customPreviewPrefab;
    public string blockName;
    public Color color;
    public Sprite icon;
    public string blockID;

    public List<BlockType> blockTypes;
    private int selectedBlockIndex; 
}
