using Firebase.Auth;
using NUnit.Framework;
using System;
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
    public static UnityAction<FirebaseUser> OnLoginDoneAction;

    //Public Variables
    public ScreenType currentScreen = ScreenType.NONE;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        OnLoginDoneAction += OnLoginDone;
        DatabaseHandler.FirebaseInitEvent += OnFirebaseInitEvent;
    }

    /// <summary>
    /// On Firebase Initialized
    /// </summary>
    private void OnFirebaseInitEvent()
    {
        Debug.Log("Game Manager :: Firebase Initialization Done Callback");
        InitializedGame();
    }

    /// <summary>
    /// On Login done callback
    /// </summary>
    private async void OnLoginDone(FirebaseUser loggedUser)
    {
        Debug.Log("Game Manager :: Login Done Callback");

        UserDataModel userDataModel = new UserDataModel();
        userDataModel.UserId = loggedUser.UserId;

        userGameSettings = await DatabaseHandler.Instance.GetUserGameSettings(loggedUser.UserId);

        if (userGameSettings != null)
        {
            userDataModel.UserGameSettings = userGameSettings;
        }
        else
        {
            userDataModel.UserGameSettings = new GameSettings();
        }

        userDataModel.UserSummaryData = new SummaryData();

        await DatabaseHandler.Instance.SaveUser(userDataModel);
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
        OnLoginDoneAction -= OnLoginDone;
        DatabaseHandler.FirebaseInitEvent -= OnFirebaseInitEvent;
    }

}
