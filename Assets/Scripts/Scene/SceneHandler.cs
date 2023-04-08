using UnityEngine;
using UnityEngine.SceneManagement;
using FantasyTown.Saving;
using System.Collections;

public class SceneHandler : MonoBehaviour,ISaveable

{
    [SerializeField] private CanvasGroup canvasGroup;

    private static SceneHandler s_instance;
    private int _currentLevelIndex;

    private SceneHandler() { }

    private void Awake()
    {
        if(s_instance == null) { s_instance = this; }
        else
        {
            Destroy(this);
        }
    }

    public static SceneHandler Instance { get => s_instance; }

    public void LoadGame()
    {
        StartCoroutine(LoadTheGame());
    }

    private IEnumerator LoadTheGame()
    {
        canvasGroup.alpha = 1;
        SavingWrapper.Instance.LoadMainData();
        _currentLevelIndex = SavingWrapper.Instance.GetLevelIndexFromData();
        print("Loading scene: " + _currentLevelIndex);
        yield return SceneManager.LoadSceneAsync(_currentLevelIndex);
        yield return new WaitForSeconds(1f);
        SavingWrapper.Instance.LoadAll();
        yield return new WaitForSeconds(1f);
        canvasGroup.alpha = 0;


    }

    public void CaptureState()
    {
        _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SavingWrapper.Instance.Data.levelIndex = _currentLevelIndex;
        print("saving scene: " + _currentLevelIndex);
    }

    public void RestoreState()
    {
        //
    }
}
