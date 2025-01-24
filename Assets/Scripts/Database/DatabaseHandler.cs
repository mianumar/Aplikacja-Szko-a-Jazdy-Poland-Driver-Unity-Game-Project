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

        GameManager.FirebaseInitEvent?.Invoke();
    }

    public async void SaveUser(UserDataModel userData)
    {
        string dataInJSON = JsonUtility.ToJson(userData);
        Debug.Log("Save User :: JSON DATA :: " + dataInJSON);
        await dbRef.Child("Users").Child(userData.UserId).SetRawJsonValueAsync(dataInJSON);
    }

    public async Task<List<UserDataModel>> GetAllUsers()
    {
        List<UserDataModel> UsersList = new List<UserDataModel>();
        await dbRef.Child("Users").GetValueAsync().ContinueWithOnMainThread(task =>
        {

            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Get User: User not found " + task.Exception);
            }
            else if (task.IsCompleted)
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

    public async Task<QADatabaseModel> GetAllQAData()
    {
        QADatabaseModel databaseModel = null;
        await dbRef.Child("QADatabase").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {

            }
            if (task.IsCanceled)
            {

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

    public async Task<GameSettings> GetUserGameSettings(string userID)
    {
        GameSettings gameSettings = null;
        await dbRef.Child("Users").Child(userID).Child("UserGameSettings").GetValueAsync().
            ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {

                }
                if (task.IsCanceled)
                {

                }
                if (task.IsCompleted)
                {
                    DataSnapshot result = task.Result;

                    string jsonData = result.GetRawJsonValue();

                    gameSettings = JsonConvert.DeserializeObject<GameSettings>(jsonData);

                }
            });

        return gameSettings;
    }


}
