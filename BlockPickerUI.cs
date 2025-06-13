using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockPickerUI : MonoBehaviour
{
    public BlockManager blockManager;
    public GameObject buttonPrefab;
    public Transform buttonParent;

    private List<Button> buttons = new List<Button>();
    private int selectedIndex = 0;

    void Start()
    {
        for (int i = 0; i < blockManager.blockTypes.Count; i++)
        {
            int index = i;
            BlockType block = blockManager.blockTypes[i];

            GameObject btnObj = Instantiate(buttonPrefab, buttonParent);
            Button btn = btnObj.GetComponent<Button>();
            Image icon = btnObj.transform.GetChild(0).GetComponent<Image>();
            icon.sprite = block.icon;

            int indexCopy = index;
            btn.onClick.AddListener(() => SelectBlock(indexCopy));
            buttons.Add(btn);
        }

        HighlightButton(0);
    }

    void SelectBlock(int index)
    {
        blockManager.SelectBlock(index);
        Debug.Log("Button clicked! Index: " + index);
        HighlightButton(index);

    }

    void HighlightButton(int index)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            var colors = buttons[i].colors;
            colors.normalColor = (i == index) ? Color.yellow : Color.white;
            buttons[i].colors = colors;
        }

        selectedIndex = index;
    }
}

