using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerHandler : MonoBehaviour
{
    public string BASE_URL = "https://admin.aplikacjaszkolajazdy.pl/api/";
    private string API_TOKEN = "?token=qbTon8Hk1dUX02mp";

    [SerializeField] private string getAllSpecializedQuestion = "getAllSpecialized.php";
    [SerializeField] private string getAllSimpleQuestions = "getAllSimple.php";
    [SerializeField] private string getCount = "getCount.php";
    [SerializeField] private string getSpecializedQuestion = "getSpecialized.php";
    [SerializeField] private string getSpecializedCount = "getSpecializedCount.php";
    [SerializeField] private string getSimpleCount = "getSimpleCount.php";
    [SerializeField] private string getSimpleQuestion = "getSimple.php";

    private string getAllSimpleRandomQuestion = "getAllSimpleRandom.php";
    private string getAllSpecialRandomQuestions = "getAllSpecializedRandom.php";


    //public Text userIDText;

    public static ServerHandler instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        DontDestroyOnLoad(this);
    }

    async void Start()
    {
        int id = PlayerPrefs.GetInt("DBUserID");

        // StartCoroutine(GetAllSimpleQuestions());

        //int count = await GetSpecilizedQuestionsCount();

        //await GetRandomSimpleQuestion(10).ContinueWith(task =>
        //{
        //    if (task.IsCompleted)
        //    {
        //        SimpleQuestionDataModel dataModel = task.Result;
        //        if (dataModel != null)
        //        {
        //            string extention = GameConstants.GetFileExtensionFromUrl(dataModel.media_link);

        //            Debug.Log("Ext :: " + extention);
        //        }
        //    }
        //});

        
    }

   

    /// <summary>
    /// Get list of Simple Questions with limit
    /// </summary>
    /// <param name="startIndex"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<SimpleQuestionListDataModel> GetSimpleQusetionsInRange(int startIndex, int limit)
    {
        SimpleQuestionListDataModel simpleQuestionListDataModel = null;
        string url = Path.Combine(BASE_URL, getAllSimpleQuestions);
        url = url + API_TOKEN + "&start=" + startIndex + "&limit=" + limit;

        Debug.Log("GetSimpleQusetionsInRange :: url :: == " + url);
        using UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // Parse JSON response
            string jsonResponse = request.downloadHandler.text.ToString();
            Debug.Log("Response: " + jsonResponse);
            // Deserialize JSON into C# object
            simpleQuestionListDataModel = JsonConvert.DeserializeObject<SimpleQuestionListDataModel>(jsonResponse);

            if (simpleQuestionListDataModel.status == "success")
            {
                //foreach (SimpleQuestionDataModel question in simpleQuestionListDataModel.dataList)
                //{
                //    Debug.Log($"ID: {question.id}, Question: {question.question}, Answer: {question.answer}, Categories: {question.categories}");
                //}
            }
            else
            {
               // Debug.LogError("API Error: " + simpleQuestionListDataModel.message);
            }

        }
        return simpleQuestionListDataModel;

    }

    public async Task<SimpleQuestionListDataModel> GetAllRandomSimpleQusetions(int limit , string catregory)
    {
        SimpleQuestionListDataModel simpleQuestionListDataModel = null;
        string url = Path.Combine(BASE_URL, getAllSimpleRandomQuestion);
        url = url + API_TOKEN +  "&limit=" + limit + "&category=" + catregory;

        Debug.Log("GetSimpleQusetionsInRange :: url :: == " + url);
        using UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // Parse JSON response
            string jsonResponse = request.downloadHandler.text.ToString();
            Debug.Log("Response: " + jsonResponse);
            // Deserialize JSON into C# object
            simpleQuestionListDataModel = JsonConvert.DeserializeObject<SimpleQuestionListDataModel>(jsonResponse);

            if (simpleQuestionListDataModel.status == "success")
            {
                //foreach (SimpleQuestionDataModel question in simpleQuestionListDataModel.dataList)
                //{
                //    Debug.Log($"ID: {question.id}, Question: {question.question}, Answer: {question.answer}, Categories: {question.categories}");
                //}
            }
            else
            {
                // Debug.LogError("API Error: " + simpleQuestionListDataModel.message);
            }

        }
        return simpleQuestionListDataModel;

    }

    /// <summary>
    /// Get Random Simple Question 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SimpleQuestionDataModel> GetRandomSimpleQuestion(int id)
    {
        SimpleQuestionDataModel question = null;
        string url = Path.Combine(BASE_URL, getSimpleQuestion);
        url = url + API_TOKEN + "&id=" + id;

       // Debug.Log("GetRandomSimpleQuestion :: url :: == " + url);
        using UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
           // Debug.Log("Response: " + jsonResponse);
            SimpleQuestionResponseModel result = JsonConvert.DeserializeObject<SimpleQuestionResponseModel>(jsonResponse);
            question = result.data;
        }

        return question;
    }

    /// <summary>
    /// Get Random Specialized question
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Specialized quesion </returns>
    public async Task<SpecializedQuestionModel> GetRandomSpecilizedQuestion(int id)
    {
        SpecializedQuestionModel question = null;
        string url = Path.Combine(BASE_URL, getSpecializedQuestion);
        url = url + API_TOKEN + "&id=" + id;

       // Debug.Log("GetRandomSpecilizedQuestion :: url :: == " + url);
        using UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
          //  Debug.Log("Response: " + jsonResponse);
            SpecializedQuestionResponseModel result = JsonConvert.DeserializeObject<SpecializedQuestionResponseModel>(jsonResponse);
            question = result.data;
        }

        return question;
    }


    public async Task<SpecializedQuestionListDataModel> GetSpecializedQusetionsInRange(int startIndex, int limit)
    {
        SpecializedQuestionListDataModel specialQusetionsDataList = null;
        string url = Path.Combine(BASE_URL, getAllSpecializedQuestion);
        url = url + API_TOKEN + "&start=" + startIndex + "&limit=" + limit;

        Debug.Log("GetSpecializedQusetionsInRange :: url :: == " + url);
        using UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // Parse JSON response
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("Response: " + jsonResponse);

            // Deserialize JSON into C# object
            specialQusetionsDataList = JsonConvert.DeserializeObject<SpecializedQuestionListDataModel>(jsonResponse);

        }
        return specialQusetionsDataList;

    }

    /// <summary>
    /// Get Random Special questions with limit
    /// </summary>
    /// <param name="limit"></param>
    /// <param name="catregory"></param>
    /// <returns></returns>
    public async Task<SpecializedQuestionListDataModel> GetAllRandomSpecialQusetions(int limit, string catregory)
    {
        SpecializedQuestionListDataModel specialQusetionsDataList = null;
        string url = Path.Combine(BASE_URL, getAllSpecialRandomQuestions);
        url = url + API_TOKEN + "&limit=" + limit + "&category=" + catregory;

        Debug.Log("GetSimpleQusetionsInRange :: url :: == " + url);
        using UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // Parse JSON response
            string jsonResponse = request.downloadHandler.text.ToString();
            Debug.Log("Response: " + jsonResponse);
            // Deserialize JSON into C# object
            specialQusetionsDataList = JsonConvert.DeserializeObject<SpecializedQuestionListDataModel>(jsonResponse);

           

        }
        return specialQusetionsDataList;

    }

    /// <summary>
    /// Get Specialized Question list count
    /// </summary>
    /// <returns>Count of the list </returns>
    public async Task<int> GetSpecilizedQuestionsCount()
    {
        int dataCount = 0;
        string url = Path.Combine(BASE_URL, getSpecializedCount);
        url = url + API_TOKEN;

        Debug.Log("GetSpecilizedQuestionsCount :: url :: == " + url);
        using UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("GetSpecilizedQuestionsCount :: Response :: " + jsonResponse);
            DataCountModel result = JsonConvert.DeserializeObject<DataCountModel>(jsonResponse);
            dataCount = int.Parse(result.data);

        }

        return dataCount;
    }

    /// <summary>
    /// Get Simple Question list count
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetSimpleQuestionsCount()
    {
        int dataCount = 0;
        string url = Path.Combine(BASE_URL, getSimpleCount);
        url = url +API_TOKEN;

        Debug.Log("GetSimpleQuestionsCount :: url :: == " + url);
        using UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            string jsonResponse = request.downloadHandler.text;
            Debug.Log("GetSimpleQuestionsCount :: Response :: " + jsonResponse);
            DataCountModel result = JsonConvert.DeserializeObject<DataCountModel>(jsonResponse);
            dataCount = int.Parse(result.data);

        }

        return dataCount;
    }



    /// <summary>
    /// 
    /// </summary>
   
    void OnApplicationQuit()
    {

        int id = PlayerPrefs.GetInt("DBUserID");
        //StartCoroutine(UpdateLastPlayDate(id, OnLastPlayDateUpdated));
        Debug.Log("Application is quitting.");

    }

}

[System.Serializable]
public class SimpleQuestionDataModel
{
    public int id {  get; set; }
    public string question { get; set; }
    public int answer { get; set; }
    public string media { get; set; }
    public string media_link { get; set; }
    public string categories { get; set; }
    public string frame_image { get; set; }
}

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class SimpleQuestionListDataModel
{
    public string status {get; set;}
    public List<SimpleQuestionDataModel> data { get; set;}
   // public string message; // Only used if an error occurs
}

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class SimpleQuestionResponseModel
{
    public string status { get; set; }
    public SimpleQuestionDataModel data { get; set; }
}
/// <summary>
/// 
/// </summary>

[System.Serializable]
public class DataCountModel
{
    public string status;
    public string data;

}

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class SpecializedQuestionModel
{
    public int id;
    public int question_number;
    public string question;
    public string option_a;
    public string option_b;
    public string option_c;
    public string answer;
    public string media;
    public string media_link;
    public string categories;
}
/// <summary>
/// 
/// </summary>
[System.Serializable]
public class SpecializedQuestionResponseModel
{
    public string status { get; set; }
    public SpecializedQuestionModel data { get; set; }
}

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class SpecializedQuestionListDataModel
{
    public string status { get; set; }
    public List<SpecializedQuestionModel> data { get; set; }
}


