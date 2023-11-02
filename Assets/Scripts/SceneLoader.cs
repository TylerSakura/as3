using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
}
