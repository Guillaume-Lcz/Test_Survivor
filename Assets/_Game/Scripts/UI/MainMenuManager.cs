using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        var playBtn = GameObject.Find("PlayButton").GetComponent<UnityEngine.UI.Button>();
        playBtn.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));

        var quitBtn = GameObject.Find("QuitButton").GetComponent<UnityEngine.UI.Button>();
        quitBtn.onClick.AddListener(() => {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });
    }
}
