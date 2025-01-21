using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class DatabaseHandler : MonoBehaviour
{
    private static DatabaseHandler instance;

    public static DatabaseHandler Instance { get { return instance; } }

    private DatabaseReference dbRef;
    private FirebaseApp firebaseAppRef;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; 
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // InitializeFirebase();
    }

    private async void InitializeFirebase()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                firebaseAppRef = FirebaseApp.DefaultInstance;
                dbRef = FirebaseDatabase.DefaultInstance.RootReference;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });


    }


}
