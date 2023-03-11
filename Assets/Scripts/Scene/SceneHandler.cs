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
        yield return SceneManager.LoadSceneAsync(0);

        yield return new WaitForSeconds(3f);
        SavingWrapper.Instance.LoadAll();

        canvasGroup.alpha = 0;


    }

    public void CaptureState()
    {
       //
    }

    public void RestoreState()
    {
        //
    }
}
