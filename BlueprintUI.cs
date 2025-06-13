using UnityEngine;
using UnityEngine.UI;

public class BlueprintUIManager : MonoBehaviour
{
    public GameObject blueprintPanel;
    public float displayDuration = 5f;

    private float timer = 0f;
    private bool isShowing = false;

    public void ShowBlueprint()
    {
        blueprintPanel.SetActive(true);
        timer = displayDuration;
        isShowing = true;
    }

    void Update()
    {
        if (isShowing)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                blueprintPanel.SetActive(false);
                isShowing = false;
            }
        }
    }
}
