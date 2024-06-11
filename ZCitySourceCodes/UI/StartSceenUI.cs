using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceenUI : MonoBehaviour
{

    public void StratBtn()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ExitBtn()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
