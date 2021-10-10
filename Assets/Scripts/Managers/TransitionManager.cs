using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance;

    [SerializeField] private GameObject transitionImage;

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private CanvasGroup m_CanvasGroup;
    private bool m_AntiOnSceneLoadedOnFirstScene;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        transitionImage.SetActive(false);
        TryGetComponent(out m_CanvasGroup);
    }

    public void StartTransition(string newScene)
    {
        if (!Application.CanStreamedLevelBeLoaded(newScene))
        {
            Debug.LogWarning("Scene \"" + newScene + "\" cannot be loaded or don't exist");
            return;
        }

        transitionImage.SetActive(true);
        LeanTween.value(0, 1, 1f)
            .setOnUpdate((float value) =>
            {
                m_CanvasGroup.alpha = value;
            })
            .setOnComplete(() =>
            {
                SceneManager.LoadScene(newScene);
            });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!m_AntiOnSceneLoadedOnFirstScene)
        {
            m_AntiOnSceneLoadedOnFirstScene = true;
            return;
        }

        LeanTween.value(1, 0, 1f)
            .setOnUpdate((float value) =>
            {
                m_CanvasGroup.alpha = value;
            })
            .setOnComplete(() =>
            {
                transitionImage.SetActive(false);
            });
    }
}
