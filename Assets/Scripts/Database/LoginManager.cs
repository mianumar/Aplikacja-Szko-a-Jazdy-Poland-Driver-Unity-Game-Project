using Firebase.Extensions;
using Firebase;
using UnityEngine;
using Firebase.Auth;

public class LoginManager : MonoBehaviour
{
    private FirebaseAuth auth;

    private FirebaseUser loggedUser = null;

    private void Start()
    {
        auth = DatabaseHandler.Instance.Auth;

        auth.StateChanged += Auth_StateChanged;
    }

    private void Auth_StateChanged(object sender, System.EventArgs e)
    {
        Debug.Log("Sender :: "+sender.ToString());
        Debug.Log("Event Args :: "+e.ToString());
    }


    /// <summary>
    /// Create anonymous user in firebase database
    /// </summary>
    public async void LoginAsGuest()
    {
        await auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {

            }
            if (task.IsCanceled)
            {

            }
            if (task.IsCompleted)
            {
                AuthResult result = task.Result;
                loggedUser = result.User;

                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                           loggedUser.DisplayName, loggedUser.UserId);
            }

        });
    }

    /// <summary>
    /// Create user if not exist in firebase database
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public async void CreateUserWithEmailAndPassword(string username , string password)
    {
        await auth.CreateUserWithEmailAndPasswordAsync(username, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {

            }
            if (task.IsCanceled)
            {

            }
            if (task.IsCompleted)
            {
                AuthResult result = task.Result;
                loggedUser = result.User;

                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                           loggedUser.DisplayName, loggedUser.UserId);
            }
        });
    }

    /// <summary>
    /// Login existing user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public async void LoginUserWithEmailAndPassword(string username, string password)
    {
        await auth.SignInWithEmailAndPasswordAsync(username, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {

            }
            if (task.IsCanceled)
            {

            }
            if (task.IsCompleted)
            {
                AuthResult result = task.Result;
                loggedUser = result.User;

                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                           loggedUser.DisplayName, loggedUser.UserId);
            }
        });
    }

    private void OnDisable()
    {
        auth.StateChanged -= Auth_StateChanged;
        auth = null;
    }
}
