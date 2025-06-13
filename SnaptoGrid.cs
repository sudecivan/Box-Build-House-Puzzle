using UnityEngine;
using System;
using System.Collections.Generic;
public class SnapToGrid : MonoBehaviour
{   public BlockManager blockManager; 
    public GameObject blockPrefab;
    public GameObject previewPrefab;
    public float gridSize = 1f;
    public LayerMask validPlacementSurfaces;
    private Quaternion currentRotation = Quaternion.identity;
    private Stack<GameObject> placedBlockStack = new Stack<GameObject>();

    public AudioClip placeSound; 
    private AudioSource audioSource;

    private GameObject previewObject;
    public Dictionary<Vector3Int, GameObject> placedBlocks = new Dictionary<Vector3Int, GameObject>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        previewObject = Instantiate(previewPrefab);
        previewObject.layer = LayerMask.NameToLayer("Preview");
        previewObject.GetComponent<Collider>().enabled = true;
        previewObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);
       

    }

    void Update()
    {
        SnapPreviewToMouse();

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRotation *= Quaternion.Euler(0f, 0f, 180f);
            previewObject.transform.rotation = currentRotation;
        }

        if (Input.GetMouseButtonDown(0) && IsMouseOverPlacementLayer())
        {
            PlaceBlock();
        }
        
    if (Input.GetMouseButtonDown(0))
    {
        int blocksLayerMask = LayerMask.GetMask("Block");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, blocksLayerMask))
        {
            Debug.Log("✅ HIT BLOCK: " + hit.collider.name);
        }
        else
        {
            Debug.Log("❌ No block hit.");
        }
    }
}

    
void SnapPreviewToMouse()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;
    
    // hit valid placement surfaces first
    if (Physics.Raycast(ray, out hit, 100f, validPlacementSurfaces))
    {
        Vector3 snappedPosition = GetSnappedPosition(hit.point);
        previewObject.transform.position = snappedPosition;
        previewObject.transform.rotation = currentRotation;
        AlignBottomToGrid(previewObject);
        return;
    }

    // hit any collider 
    if (Physics.Raycast(ray, out hit, 300f))
    {
        GameObject hitObj = hit.collider.gameObject;

        // Loop through placed blocks to see if the hit matches one of them
        foreach (var kvp in placedBlocks)
    {
            GameObject placedBlock = kvp.Value;
            if (placedBlock == null) continue; 

            if (hitObj.transform.root == placedBlock.transform)
                {
                    Vector3Int blockGridPos = kvp.Key;
                    Vector3Int normalDir = Vector3Int.RoundToInt(hit.normal);

                    Vector3Int targetGridPos = blockGridPos + normalDir;
                    Vector3 targetWorldPos = targetGridPos;

                    previewObject.transform.position = targetWorldPos;
                    previewObject.transform.rotation = currentRotation;
                    AlignBottomToGrid(previewObject);
                    return;
                }

     }


    }

    
}

Vector3 GetSnappedPosition(Vector3 point)
{
    float x = Mathf.Round(point.x / gridSize) * gridSize;
    float y = Mathf.Round(point.y / gridSize) * gridSize;
    float z = Mathf.Round(point.z / gridSize) * gridSize;
    return new Vector3(x, y, z);
}


Vector3 GetSnappedPosition(RaycastHit hit)
{
    return GetSnappedPosition(hit.point);
}




    void PlaceBlock()
    {
        Vector3Int gridPos = Vector3Int.RoundToInt(previewObject.transform.position);
        if (!placedBlocks.ContainsKey(gridPos))
        {
            GameObject selectedBlock = blockManager.GetSelectedBlock();
            GameObject newBlock = Instantiate(selectedBlock, gridPos, currentRotation);
            newBlock.layer = LayerMask.NameToLayer("Block");
            foreach (Transform child in newBlock.transform)
     {
       child.gameObject.layer = LayerMask.NameToLayer("Block");
     }
            AlignBottomToGrid(newBlock);
            placedBlocks.Add(gridPos, newBlock);
            placedBlockStack.Push(newBlock); 

            if (placeSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(placeSound);
            }
        }
    }

    void AlignBottomToGrid(GameObject obj)
    {
        Renderer rend = obj.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            float bottomY = rend.bounds.min.y;
            float offsetY = obj.transform.position.y - bottomY;

            obj.transform.position += new Vector3(0, offsetY, 0);
        }
    }
bool IsMouseOverPlacementLayer()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    return Physics.Raycast(ray, 100f, validPlacementSurfaces);
}
    public void SetBlockType(BlockType type)
    {
        blockPrefab = type.prefab;

        if (previewObject != null)
        {
            Destroy(previewObject);
        }

        previewObject = Instantiate(blockPrefab);
        previewObject.GetComponent<Collider>().enabled = false;
        previewObject.layer = LayerMask.NameToLayer("Preview");

        previewObject.transform.rotation = currentRotation;
    }


    public void UndoLastPlacement()
{
    if (placedBlockStack.Count > 0)
    {
        GameObject lastBlock = placedBlockStack.Pop();

        if (lastBlock != null)
        {
            Vector3Int gridPos = Vector3Int.RoundToInt(lastBlock.transform.position);

            placedBlocks.Remove(gridPos);
            Destroy(lastBlock);
        }
        else
        {
            Debug.LogWarning("Tried to undo a block that was already destroyed.");
        }
    }
}





}

