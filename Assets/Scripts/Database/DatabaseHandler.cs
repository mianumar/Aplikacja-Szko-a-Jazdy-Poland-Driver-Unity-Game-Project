using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Firebase.Extensions;
using NUnit.Framework;
using System.Collections.Generic;


public class DatabaseHandler : MonoBehaviour
{
    private static DatabaseHandler instance;

    public static DatabaseHandler Instance { get { return instance; } }

    private DatabaseReference dbRef;
    private FirebaseAuth auth;

    public FirebaseAuth Auth => auth;

    public delegate void FirebaseInitEventHandler();
    public static event FirebaseInitEventHandler FirebaseInitEvent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        FirebaseApp.Create();
        InitializeFirebase();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    /// <summary>
    /// Initialize firebase 
    /// </summary>
    private async void InitializeFirebase()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                auth = FirebaseAuth.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.

                Debug.Log("Firebase Initialized");
            }
            else
            {
                Debug.LogErrorFormat("Could not resolve all Firebase dependencies: {0}", dependencyStatus);
                // Firebase Unity SDK is not safe to use here.
            }
        });

        FirebaseInitEvent?.Invoke();
    }


    /// <summary>
    /// Save Users Data into database.
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    public async void SaveUser(UserDataModel userData)
    {
        string dataInJSON = JsonConvert.SerializeObject(userData);
        Debug.Log("Save User :: JSON DATA :: " + dataInJSON);
        await dbRef.Child("Users").Child(userData.UserId).SetRawJsonValueAsync(dataInJSON).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("User Data Saved Successfully ");
            }
            if (task.IsFaulted)
            {
                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                Debug.LogError("SaveUser :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                string expMessage = task.Exception.InnerExceptions[0].Message;
                Debug.LogError("SaveUser encountered an error: " + expMessage);
            }
        });
    }

    public async Task<UserDataModel> GetUserData(string userId)
    {
        UserDataModel dataModel = null;
        await dbRef.Child("Users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                Debug.LogError("GetUserData :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                string expMessage = task.Exception.InnerExceptions[0].Message;
                Debug.LogError("GetUserData encountered an error: " + expMessage);
            }
            if (task.IsCanceled)
            {
                Debug.LogError("GetUserData request is cancled: ");
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                string jsonData = snapshot.GetRawJsonValue();
                if (jsonData != null)
                {
                    dataModel = JsonConvert.DeserializeObject<UserDataModel>(jsonData);
                }

            }
        });

        return dataModel;
    }


    /// <summary>
    /// Get All Users Data
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserDataModel>> GetAllUsers()
    {
        List<UserDataModel> UsersList = new List<UserDataModel>();
        await dbRef.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {

            if (task.IsFaulted)
            {
                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                Debug.LogError("GetAllUsers :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                string expMessage = task.Exception.InnerExceptions[0].Message;
                Debug.LogError("GetAllUsers encountered an error: " + expMessage);
            }
            if (task.IsCanceled)
            {
                Debug.LogError("GetAllUsers request is cancled: ");
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                var users = snapshot.Children;
                // Debug.LogError("All users: " + snapshot.GetRawJsonValue());
                foreach (var user in users)
                {
                    UserDataModel currentUser = JsonConvert.DeserializeObject<UserDataModel>(user.GetRawJsonValue());
                    UsersList.Add(currentUser);
                }
                // Debug.LogError("UserCount: " + allUsers.Count);
            }
        });

        return UsersList;
    }

    /// <summary>
    /// Get All set of questions and answers
    /// </summary>
    /// <returns></returns>
    public async Task<QADatabaseModel> GetAllQAData()
    {
        QADatabaseModel databaseModel = null;
        await dbRef.Child("QADatabase").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                Debug.LogError("GetAllQAData :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                string expMessage = task.Exception.InnerExceptions[0].Message;
                Debug.LogError("GetAllQAData encountered an error: " + expMessage);
            }
            if (task.IsCanceled)
            {
                Debug.LogError("GetAllQAData request is cancled: ");
            }
            if (task.IsCompleted)
            {
                DataSnapshot result = task.Result;

                string jsonData = result.GetRawJsonValue();

                databaseModel = JsonConvert.DeserializeObject<QADatabaseModel>(jsonData);

            }
        });

        return databaseModel;
    }

    /// <summary>
    /// Get Users Game Settings
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public async Task<GameSettings> GetUserGameSettings(string userID)
    {
        GameSettings gameSettings = null;
        await dbRef.Child("Users").Child(userID).Child("UserGameSettings").GetValueAsync().
            ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                    Debug.LogError("GetUserGameSettings :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                    string expMessage = task.Exception.InnerExceptions[0].Message;
                    Debug.LogError("GetUserGameSettings encountered an error: " + expMessage);
                }
                if (task.IsCanceled)
                {
                    Debug.LogError("GetUserGameSettings request is cancled: ");
                }
                if (task.IsCompleted)
                {
                    DataSnapshot result = task.Result;

                    string jsonData = result.GetRawJsonValue();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        gameSettings = JsonConvert.DeserializeObject<GameSettings>(jsonData);
                    }

                }
            });

        return gameSettings;
    }

    /// <summary>
    /// Save User Settings to database
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="gameSettings"></param>
    public async void SaveUserGameSettings(string userId, GameSettings gameSettings)
    {
        string dataInJSON = JsonConvert.SerializeObject(gameSettings);
        Debug.Log("Save UserGameSettings :: JSON DATA :: " + dataInJSON);
        await dbRef.Child("Users").Child(userId).Child("UserGameSettings").SetRawJsonValueAsync(dataInJSON).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("User Settings Saved Successfully ");
            }
            if (task.IsFaulted)
            {
                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                Debug.LogError("SaveUserGameSettings :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                string expMessage = task.Exception.InnerExceptions[0].Message;
                Debug.LogError("SaveUserGameSettings encountered an error: " + expMessage);
            }
        });
    }


    /// <summary>
    /// Get User Game Summary
    /// </summary>
    /// <param name="userID"></param>
    /// <returns></returns>
    public async Task<SummaryData> GetUserGameSummary(string userID)
    {
        SummaryData summaryData = null;
        await dbRef.Child("Users").Child(userID).Child("UserSummaryData").GetValueAsync().
            ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                    Debug.LogError("GetUserGameSummary :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                    string expMessage = task.Exception.InnerExceptions[0].Message;
                    Debug.LogError("GetUserGameSummary encountered an error: " + expMessage);
                }
                if (task.IsCanceled)
                {
                    Debug.LogError("GetUserGameSummary request is cancled: ");
                }
                if (task.IsCompleted)
                {
                    DataSnapshot result = task.Result;

                    string jsonData = result.GetRawJsonValue();
                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        summaryData = JsonConvert.DeserializeObject<SummaryData>(jsonData);
                    }

                }
            });

        return summaryData;
    }

    /// <summary>
    /// Save User Game Summary to database
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="gameSummary"></param>
    public async void SaveUserGameSummary(string userId, SummaryData gameSummary)
    {
        string dataInJSON = JsonConvert.SerializeObject(gameSummary);
        Debug.Log("Save UserSummaryData :: JSON DATA :: " + dataInJSON);
        await dbRef.Child("Users").Child(userId).Child("UserSummaryData").SetRawJsonValueAsync(dataInJSON).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("User Summary Saved Successfully ");
            }
            if (task.IsFaulted)
            {
                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                Debug.LogError("SaveUserGameSummary :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                string expMessage = task.Exception.InnerExceptions[0].Message;
                Debug.LogError("SaveUserGameSummary encountered an error: " + expMessage);
            }
        });
    }


}
