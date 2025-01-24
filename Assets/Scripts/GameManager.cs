using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ScreenType
{
    NONE,
    SPLASH,
    MAIN_MENU,
    SETTINGS,
    QASCREEN,
    SUMMARY,
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<QADataModel> questionList;

    private GameSettings userGameSettings;

    // Events
    public static UnityEvent OnLoginDoneEvent;
    public static UnityEvent FirebaseInitEvent;

    //Public Variables
    public ScreenType currentScreen = ScreenType.NONE;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        OnLoginDoneEvent?.AddListener(OnLoginDone);
        FirebaseInitEvent.AddListener(OnFirebaseInitialized);
    }


    /// <summary>
    /// On Login done callback
    /// </summary>
    private void OnLoginDone()
    {

    }

    /// <summary>
    /// On Firebase Initialized
    /// </summary>
    private void OnFirebaseInitialized()
    {
        InitializedGame();
    }


    /// <summary>
    /// Load all the data from database
    /// </summary>
    public async void InitializedGame()
    {
        QADatabaseModel qaData = await DatabaseHandler.Instance.GetAllQAData();

        questionList = qaData.QADataList;

    }

    private void OnDisable()
    {
        OnLoginDoneEvent?.RemoveListener(OnLoginDone);
        FirebaseInitEvent.RemoveListener(OnFirebaseInitialized);

    }

}
