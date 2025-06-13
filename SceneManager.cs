using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}
