using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BlueprintValidator : MonoBehaviour
{
    public Blueprint targetBlueprint;
    public SnapToGrid snapToGrid;

    public GameObject successPanel;
    public GameObject failPanel;
    public TMP_Text failMessageText;
    public Button restartButton;

    public TMP_Text timerText;
    public float timeLimit = 180f;
    private float timeRemaining;
    private bool timerRunning = true;
    private bool structureChecked = false;
    public AudioClip timerSoundClip;
    private AudioSource timerAudioSource;

    [Range(0f, 1f)]
    public float timerSoundVolume = 0.1f;
    public Button submitButton;

    void Start()
    {
        timeRemaining = timeLimit;
        GameObject timerAudioGO = new GameObject("TimerAudioSource");
        timerAudioGO.transform.parent = this.transform;

        timerAudioSource = timerAudioGO.AddComponent<AudioSource>();
        timerAudioSource.clip = timerSoundClip;
        timerAudioSource.loop = true;
        timerAudioSource.volume = timerSoundVolume;
        timerAudioSource.playOnAwake = false;
        // Add the low-pass filter to simulate distant/muffled sound
        AudioLowPassFilter lowPass = timerAudioGO.AddComponent<AudioLowPassFilter>();
        lowPass.cutoffFrequency = 700f; // 500 Hz = very muffled


        if (restartButton != null)
            restartButton.onClick.AddListener(RestartScene);

        if (submitButton != null)
            submitButton.onClick.AddListener(CheckStructure); 
        
        timerAudioSource.Play();
    }

    void Update()
    {  { if (timerRunning && !structureChecked)
    {
        timeRemaining -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutes}:{seconds:00}";

        if (timeRemaining <= 0)
        {
            timerRunning = false;
            timeRemaining = 0;

            if (timerAudioSource.isPlaying)
                timerAudioSource.Stop();

            ShowFail("Time's up!");
        }
    }
}

    }

    public void CheckStructure()
    {
        if (structureChecked) return;

        structureChecked = true;
        timerRunning = false;
        if (timerAudioSource.isPlaying)
        timerAudioSource.Stop();

        Dictionary<Vector3Int, GameObject> placedBlocks = snapToGrid.placedBlocks;

        if (placedBlocks.Count != targetBlueprint.blocks.Count)
        {
            ShowFail("Incorrect number of blocks.");
            return;
        }

        foreach (BlockData block in targetBlueprint.blocks)
        {
            if (!BlockExistsNear(block.position))
            {
                Debug.LogWarning($"❌ Blueprint expects block at: {block.position}");
        foreach (var kvp in placedBlocks)
        {
            Debug.Log($"✅ Found block at: {kvp.Key}");
        }
        ShowFail($"Missing block at {block.position}");
        return;
            }
        }

        ShowSuccess();
    }
    
     bool BlockExistsNear(Vector3Int expectedPos, float tolerance = 1f)
    {
    foreach (var placed in snapToGrid.placedBlocks.Keys)
    {
        if (Vector3.Distance(placed, expectedPos) <= tolerance)
            return true;
    }
    return false;
    }

    void ShowSuccess()
    {
        successPanel.SetActive(true);
        failPanel.SetActive(false);
    }

    void ShowFail(string message)
    {
        failPanel.SetActive(true);
        successPanel.SetActive(false);

        if (failMessageText != null)
            failMessageText.text = message;
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}



