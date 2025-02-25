using Firebase.Auth;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using static Unity.VisualScripting.Member;

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

    //Properties
    public string UserID => userDataModel.UserId;
    public UserDataModel UserDataModel => userDataModel;

    public VideoPlayer videoPlayer;

    // Events
    public static UnityAction<FirebaseUser> OnLoginDoneAction;

    //Public Variables
    public ScreenType currentScreen = ScreenType.NONE;

    public int MaxTotalPoints;
    public int MinPointsToPass;

    private UserDataModel userDataModel;

    private string url = "https://res.cloudinary.com/prod/video/upload/e_preview:duration_12:max_seg_3/me/preview-coffee.mp4"; 
    

    private string filePath = string.Empty;
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

    public void SetupVideoPlayer()
    {
        filePath = Application.dataPath + "/Resources/Videos/testvideo.mp4";
        Debug.Log("File Path : " + filePath);
        GameUtils.VideoDownloader.RequestDownload(this, url, filePath, (flag) =>
        {
            if (flag)
            {
                var videoClip = Resources.Load<VideoClip>("/Videos/testvideo.mp4");
                videoPlayer.url = url;
                // Debug.Log("Clip length :: "+videoPlayer.clip.length);
                if (!videoPlayer.isPrepared)
                {
                    videoPlayer.prepareCompleted += OnVideoPrepared; // Subscribe to the event
                    videoPlayer.Prepare(); // Prepare the video asynchronously
                    Debug.Log("Preparing video to get length...");
                }
                else
                {
                    Debug.Log("Clip length :: " + videoPlayer.clip.length);
                    videoPlayer.Play();
                }
            }
        });
    }

    private void OnVideoPrepared(VideoPlayer source)
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        Debug.Log("Clip length :: "+source.length);
        videoPlayer.Play();
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
       
        userDataModel= await DatabaseHandler.Instance.GetUserData(loggedUser.UserId);

        // Save User Data if Not Found
        if (userDataModel == null)
        {
            userDataModel = new UserDataModel();
            userDataModel.UserId = loggedUser.UserId;
            userDataModel.UserName = string.IsNullOrEmpty(loggedUser.DisplayName) ? "Guest " + Random.Range(2000, 9999) : loggedUser.DisplayName;
            userDataModel.UserGameSettings = new GameSettings();
            userDataModel.UserSummaryData = new SummaryData();

            DatabaseHandler.Instance.SaveUser(userDataModel);
        }
        else
        {

        }
        
    }


    /// <summary>
    /// Load all the data from database
    /// </summary>
    public async void InitializedGame()
    {
       
        //QADatabaseModel qaData = await DatabaseHandler.Instance.GetAllQAData();

        //questionList = qaData.QADataList;
        //MaxTotalPoints = qaData.MaxTotalPoints;
        //MinPointsToPass = qaData.MinPointsToPass;

    }

    public List<QADataModel> GetAllQuestionAns()
    {
        return questionList;
    }

    private void OnDisable()
    {
        OnLoginDoneAction -= OnLoginDone;
        DatabaseHandler.FirebaseInitEvent -= OnFirebaseInitEvent;
    }

}
