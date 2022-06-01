using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake() 
    {
        Instance = this;
    }

    public void LoadScene(string scene)
    {
        Time.timeScale = 1f;
        AudioManager.Instance.ChangeMusicVolumeWithLerp(0, 1);
        TransitionManager.Instance.StartTransition(scene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
