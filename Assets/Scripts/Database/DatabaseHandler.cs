using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;


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

        //FirebaseApp.Create();
        //InitializeFirebase();
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
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
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
    }

    public async void SaveUser(UserDataModel userData)
    {
        string dataInJSON = JsonUtility.ToJson(userData);
        Debug.Log("Save User :: JSON DATA :: "+dataInJSON);
        await dbRef.Child("Users").Child(userData.UserId).SetRawJsonValueAsync(dataInJSON);
    }




}
