using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public List<BlockType> blockTypes;
    public SnapToGrid snapToGrid;

    private int currentIndex = 0;

    public int selectedBlockIndex = 0;

    void Start()
    {
        if (blockTypes.Count > 0)
        {
            SelectBlock(0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))  
        {
            currentIndex = (currentIndex + 1) % blockTypes.Count;
            SelectBlock(currentIndex);
        }
    }



    public void SelectBlock(int index)
    {
        if (index >= 0 && index < blockTypes.Count)
        {
            selectedBlockIndex = index;
            Debug.Log("Selected Block: " + blockTypes[index].name);
        }
        
    }
    public GameObject GetSelectedBlock()
    {
        return blockTypes[selectedBlockIndex].prefab;
    }
}


