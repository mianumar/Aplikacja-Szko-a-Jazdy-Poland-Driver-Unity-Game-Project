using Firebase.Auth;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using static Unity.VisualScripting.Member;
using Firebase.Extensions;
using System.Linq;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using UnityEditor;

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
    private SummaryData _userSummaryData = new SummaryData();


    private string url = "https://res.cloudinary.com/prod/video/upload/e_preview:duration_12:max_seg_3/me/preview-coffee.mp4";

    public string VIDEO_FILE_DIR_SIMPLE;
    public string VIDEO_FILE_DIR_SPECIAL;

    public const int MAX_BASIC_QUESTION_COUNT = 20;
    public const int MAX_SPECIAL_QUESTION_COUNT = 12;

    private int totalSimpleQuestionCount;
    private int totalSpecialQuestionCount;

    private List<SimpleQuestionDataModel> randomSimpleQstnList = new List<SimpleQuestionDataModel>();
    private List<SpecializedQuestionModel> randomSpecialQstnList = new List<SpecializedQuestionModel>();

    private List<int> randomSimpleIndexces = new List<int>();
    private List<int> randomSpecialIndexces = new List<int>();

    public Dictionary<int, Sprite> SimpleQuestionSprites = new Dictionary<int, Sprite>();
    public Dictionary<int, Sprite> SpecialQuestionSprites = new Dictionary<int, Sprite>();

    private int currectSimpleIndex = 0;
    private int currectSpecialIndex = 0;

    public bool IsAllMediaDownloaded = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        VIDEO_FILE_DIR_SIMPLE = Application.persistentDataPath + "/Videos/SIMPLE/";
        VIDEO_FILE_DIR_SPECIAL = Application.persistentDataPath + "/Videos/SPECIAL/";
        OnLoginDoneAction += OnLoginDone;
        DatabaseHandler.FirebaseInitEvent += OnFirebaseInitEvent;
        DeleteAllFiles();
        InitializedGame();
        //SetupVideoPlayer("test");
    }

    private void DeleteAllFiles()
    {
        DirectoryInfo dirSimple = new DirectoryInfo(VIDEO_FILE_DIR_SIMPLE);
        DirectoryInfo dirSpecial = new DirectoryInfo(VIDEO_FILE_DIR_SPECIAL);

        if (dirSimple.Exists)
        {

            foreach (FileInfo file in dirSimple.GetFiles())
            {
                file.Delete();
            }
        }
        if (dirSpecial.Exists)
        {
            foreach (FileInfo file in dirSpecial.GetFiles())
            {
                file.Delete();
            }
        }
        // AssetDatabase.Refresh();
    }

    public void SetupVideoPlayer(string filename)
    {
        // filePath = Application.dataPath + "/Resources/Videos/testvideo.mp4";
        var videofile = Resources.Load("Videos/" + filename);
        VideoClip videoClip = Resources.Load("Videos/test.mp4") as VideoClip;
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

    private void OnVideoPrepared(VideoPlayer source)
    {
        videoPlayer.prepareCompleted -= OnVideoPrepared;
        Debug.Log("Clip length :: " + source.length);
        videoPlayer.Play();
    }

    /// <summary>
    /// On Firebase Initialized
    /// </summary>
    private void OnFirebaseInitEvent()
    {
        Debug.Log("Game Manager :: Firebase Initialization Done Callback");

    }

    /// <summary>
    /// On Login done callback
    /// </summary>
    private async void OnLoginDone(FirebaseUser loggedUser)
    {
        Debug.Log("Game Manager :: Login Done Callback");

        userDataModel = await DatabaseHandler.Instance.GetUserData(loggedUser.UserId);

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
        // InitializedGame();
    }


    /// <summary>
    /// Load all the data from database
    /// </summary>
    public async void InitializedGame()
    {
        IsAllMediaDownloaded = false;
        randomSimpleQstnList.Clear();
        randomSpecialQstnList.Clear();
        await ServerHandler.instance.GetSpecilizedQuestionsCount().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {

                totalSpecialQuestionCount = task.Result;
                randomSpecialIndexces = GetRandomIndex(1, totalSpecialQuestionCount, MAX_SPECIAL_QUESTION_COUNT);
                if (randomSpecialIndexces.Count > 0)
                {
                    GetSpecialQuestionsData(randomSpecialIndexces[currectSpecialIndex]);
                }

            }
        });
        await ServerHandler.instance.GetSimpleQuestionsCount().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                totalSimpleQuestionCount = task.Result;

                randomSimpleIndexces = GetRandomIndex(1, totalSimpleQuestionCount, MAX_BASIC_QUESTION_COUNT);
                if (randomSimpleIndexces.Count > 0)
                {
                    GetSimpleQuestionsData(randomSimpleIndexces[currectSimpleIndex]);
                }
            }
        });

    }

    private void GetSimpleQuestionsData(int index)
    {
        ServerHandler.instance.GetRandomSimpleQuestion(index).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                randomSimpleQstnList.Add(task.Result);
                currectSimpleIndex++;
                if (currectSimpleIndex < randomSimpleIndexces.Count)
                {
                    GetSimpleQuestionsData(randomSimpleIndexces[currectSimpleIndex]);
                }
                else
                {
                    Debug.Log("All Simple Questions Downloaded successfully");
                    DownloadingMediaForSimpleQstn(randomSimpleQstnList[_simpleIndex]);
                }
            }
        });
    }

    private void GetSpecialQuestionsData(int index)
    {
        ServerHandler.instance.GetRandomSpecilizedQuestion(index).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                randomSpecialQstnList.Add(task.Result);
                currectSpecialIndex++;
                if (currectSpecialIndex < randomSpecialIndexces.Count)
                {
                    GetSpecialQuestionsData(randomSpecialIndexces[currectSpecialIndex]);
                }
                else
                {
                    Debug.Log("All Special Questions Downloaded successfully");
                    DownloadMediaForSpecialQuestions(randomSpecialQstnList[_specialIndex]);
                }
            }
        });
    }

    int _simpleIndex = 0;
    int count = 0;
    private void DownloadingMediaForSimpleQstn(SimpleQuestionDataModel data)
    {
        Debug.Log("DownloadingMediaForSimpleQstn");
        string extention = GameConstants.GetFileExtensionFromUrl(data.media_link);
        if (string.IsNullOrEmpty(extention))
            return;
        if (extention.Equals(".jpg"))
        {
            GameUtils.ImageDownloader.RequestDownload(this, data.media_link, (tex) =>
            {
                Sprite sp = SaveImageFile(tex);
                SimpleQuestionSprites.Add(data.id, sp);
                _simpleIndex++;
                if (_simpleIndex < randomSimpleQstnList.Count)
                {
                    //AssetDatabase.Refresh();
                    DownloadingMediaForSimpleQstn(randomSimpleQstnList[_simpleIndex]);
                }
                else
                {
                    Debug.LogError("All Media Downloaded :: Image");
                }
            });
        }
        else if (extention.Equals(".mp4"))
        {
            count += 1;
            Debug.LogError("Total Video File " + count);
            string filePath = string.Concat(VIDEO_FILE_DIR_SIMPLE, data.id, extention);
            GameUtils.VideoDownloader.RequestDownload(this, data.media_link, filePath, (result) =>
            {
                _simpleIndex++;
                if (_simpleIndex < randomSimpleQstnList.Count)
                {
                    // AssetDatabase.Refresh();
                    DownloadingMediaForSimpleQstn(randomSimpleQstnList[_simpleIndex]);
                }
                else
                {
                    Debug.LogError("All Media Downloaded :: Video");


                }
            });
        }

        if (_simpleIndex == randomSimpleQstnList.Count)
        {
            Debug.Log("ALL MEDIA DOWNLOADED SUCCESSFULLY..");
            IsAllMediaDownloaded = true;
        }

    }
    int _specialIndex = 0;
    private void DownloadMediaForSpecialQuestions(SpecializedQuestionModel data)
    {
        Debug.Log("DownloadMediaForSpecialQuestions");
        string extention = GameConstants.GetFileExtensionFromUrl(data.media_link);
        if (string.IsNullOrEmpty(extention))
            return;
        if (extention.Equals(".jpg"))
        {
            GameUtils.ImageDownloader.RequestDownload(this, data.media_link, (tex) =>
            {
                Sprite sp = SaveImageFile(tex);
                SpecialQuestionSprites.Add(data.id, sp);
                _specialIndex++;
                if (_specialIndex < randomSpecialQstnList.Count)
                {
                    //AssetDatabase.Refresh();
                    DownloadMediaForSpecialQuestions(randomSpecialQstnList[_specialIndex]);
                }
                else
                {
                    Debug.LogError("All Media Downloaded :: Image");
                }
            });
        }
        else if (extention.Equals(".mp4"))
        {
            string filePath = string.Concat(VIDEO_FILE_DIR_SPECIAL, data.id, extention);
            GameUtils.VideoDownloader.RequestDownload(this, data.media_link, filePath, (result) =>
            {
                _specialIndex++;
                if (_specialIndex < randomSpecialQstnList.Count)
                {
                    // AssetDatabase.Refresh();
                    DownloadMediaForSpecialQuestions(randomSpecialQstnList[_specialIndex]);
                }
                else
                {
                    Debug.LogError("All Media Downloaded :: Video");


                }
            });
        }

        if (_simpleIndex == randomSpecialQstnList.Count)
        {
            Debug.Log("ALL MEDIA DOWNLOADED SUCCESSFULLY..");
           
        }
    }

    private List<int> GetRandomIndex(int min, int max, int count)
    {
        System.Random random = new System.Random();
        return Enumerable.Range(min, max - 1).OrderBy(x => random.Next()).Take(count).ToList();

    }

    public SimpleQuestionDataModel GetSimpleQuestionFromList(int index)
    {
        if (index < 0 && index >= randomSimpleQstnList.Count)
            return null;
        return randomSimpleQstnList[index];
    }

    public SpecializedQuestionModel GetSpecialQuestionFromList(int index)
    {
        if (index < 0 && index >= randomSimpleQstnList.Count)
            return null;
        return randomSpecialQstnList[index];
    }

    public Sprite GetQuestionSprite(int id)
    {
        if (SimpleQuestionSprites.ContainsKey(id))
            return SimpleQuestionSprites[id];
        return null;
    }

    public Sprite GetSpecializedQuestioSprite(int id)
    {
        if (SpecialQuestionSprites.ContainsKey(id))
            return SpecialQuestionSprites[id];
        return null;
    }

    private Sprite SaveImageFile(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void UpdateUserSummaryData(int points ,ResultType resultType)
    {
        if(resultType == ResultType.CORRECT)
        {
            _userSummaryData.PointsAchieved += points;
            _userSummaryData.TotalCorrectAns += 1;
        }else if(resultType == ResultType.WRONG)
        {
            _userSummaryData.TotalWrongAns += 1;
        }else if(resultType == ResultType.SKIPPED)
        {
            _userSummaryData.TotalSkipedAns += 1;
        }
    }

    public void UpdateGameTimerInSummaryData(float totalGameTime)
    {
        _userSummaryData.TotalDurations = totalGameTime;
    }

    public SummaryData GetUserSummaryData()
    {
        return _userSummaryData;    
    }

    

    private void OnDisable()
    {
        OnLoginDoneAction -= OnLoginDone;
        DatabaseHandler.FirebaseInitEvent -= OnFirebaseInitEvent;
    }

}

[System.Serializable]
public class UserSummaryData
{
    public int qsntSkipped;
    public int wrongAns;
    public int correctAns;
    public int totalPoints;
}

public enum ResultType
{
    CORRECT = 1,
    WRONG = 2,
    SKIPPED = 3
}
