using UnityEngine;
using UnityEngine.UI;
public class ButtonSoundPlayer : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = clickSound;

        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => PlaySound());

            if (btn.GetComponent<HoverScaler>() == null)
            {
                btn.gameObject.AddComponent<HoverScaler>();
            }
        }
    }

    void PlaySound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}


