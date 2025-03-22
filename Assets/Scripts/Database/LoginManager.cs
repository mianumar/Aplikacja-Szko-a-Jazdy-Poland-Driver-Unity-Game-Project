using Firebase.Extensions;
using Firebase;
using UnityEngine;
using Firebase.Auth;
using System;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    private FirebaseAuth auth;

    private FirebaseUser loggedUser = null;

    private void Start()
    {
        DatabaseHandler.FirebaseInitEvent += OnFirebaseInitialized;
    }

    /// <summary>
    /// Invoke on firebase initialized.
    /// </summary>
    private void OnFirebaseInitialized()
    {
        Debug.Log("Login Manager :: Firebase Initialization Done Callback");
        auth = DatabaseHandler.Instance.Auth;
        auth.StateChanged += Auth_StateChanged;
    }

    private void Auth_StateChanged(object sender, System.EventArgs e)
    {
        Debug.Log("Sender :: "+sender.ToString());
        Debug.Log("Event Args :: "+e.ToString());

        //if (auth.CurrentUser != null)
        //{
        //    // User is signed In 
        //    loggedUser = auth.CurrentUser;
        //    GameManager.OnLoginDoneAction?.Invoke(loggedUser);
        //}
        //else
        {
            LoginAsGuest();
        }
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
                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                Debug.LogError("LoginAsGuest :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                string expMessage = task.Exception.InnerExceptions[0].Message;
                Debug.LogError("LoginAsGuest encountered an error: " + expMessage);
            }
            if (task.IsCanceled)
            {
                Debug.LogError("LoginAsGuest request is cancled: ");
            }
            if (task.IsCompleted)
            {
                AuthResult result = task.Result;
                loggedUser = result.User;

                Debug.LogFormat("Firebase Guest user created successfully: {0} ({1})",
                           loggedUser.DisplayName, loggedUser.UserId);
            }

        });
        if (loggedUser != null)
        {
            GameManager.OnLoginDoneAction?.Invoke(loggedUser);
        }
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
                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                Debug.LogError("CreateUserWithEmailAndPasswordAsync :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                string expMessage = task.Exception.InnerExceptions[0].Message;
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + expMessage);
            }
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPassword request is cancled: ");

            }
            if (task.IsCompleted)
            {
                AuthResult result = task.Result;
                loggedUser = result.User;

                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                           loggedUser.DisplayName, loggedUser.UserId);
            }
        });
        if (loggedUser != null)
        {
            GameManager.OnLoginDoneAction?.Invoke(loggedUser);
        }
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
                FirebaseException firebaseException = task.Exception.GetBaseException() as FirebaseException;
                Debug.LogError("LoginUserWithEmailAndPassword :: ErrorCode :: " + (AuthError)firebaseException.ErrorCode);
                string expMessage = task.Exception.InnerExceptions[0].Message;
                Debug.LogError("LoginUserWithEmailAndPassword encountered an error: " + expMessage);
            }
            if (task.IsCanceled)
            {
                Debug.LogError("LoginUserWithEmailAndPassword request is cancled: ");

            }
            if (task.IsCompleted)
            {
                AuthResult result = task.Result;
                loggedUser = result.User;

                Debug.LogFormat("Firebase user Login with Email successfully: {0} ({1})",
                           loggedUser.DisplayName, loggedUser.UserId);
            }
        });
        if (loggedUser != null)
        {
            GameManager.OnLoginDoneAction?.Invoke(loggedUser);
        }
    }

    public void LogOut()
    {
        auth.SignOut();
        auth = null;
    }

    private void OnDisable()
    {
        DatabaseHandler.FirebaseInitEvent -= OnFirebaseInitialized;
    }

    private void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= Auth_StateChanged;
            auth = null;
        }
    }
}
