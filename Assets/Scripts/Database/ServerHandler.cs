using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerHandler : MonoBehaviour
{
    public string url = "http://play.saad1430.site/polish/api/";
    private string apiUrl = "http://play.saad1430.site/polish/api/getAllSimple.php?limit=10&start=0";



    public string getAllMcqs = "getAllMcqs.php";
    public string getAllSimpleQuestions = "getAllSimple.php";
    public string getCount = "getCount.php";
    public string getMcqs = "getMcqs.php";
    public string getMcqsCount = "getMcqsCount.php";
    public string getSimpleCount = "getSimpleCount.php";


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

    void Start()
    {
        int id = PlayerPrefs.GetInt("DBUserID");

        StartCoroutine(GetAllSimpleQuestions());

    }

    IEnumerator GetAllSimpleQuestions()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

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
                Questions_APIResponse response = JsonConvert.DeserializeObject<Questions_APIResponse>(jsonResponse);

                if (response.status == "success")
                {
                    foreach (Question question in response.data)
                    {
                        Debug.Log($"ID: {question.id}, Question: {question.question}, Answer: {question.answer}, Categories: {question.categories}");
                    }
                }
                else
                {
                    Debug.LogError("API Error: " + response.message);
                }
            }
        }

    }




    [System.Serializable]
    public class Question
    {
        public int id;
        public string question;
        public string answer;
        public string media;
        public string media_link;
        public string categories;
    }

    [System.Serializable]
    public class Questions_APIResponse
    {
        public string status;
        public List<Question> data;
        public string message; // Only used if an error occurs
    }

    void OnApplicationQuit()
    {

        int id = PlayerPrefs.GetInt("DBUserID");
        //StartCoroutine(UpdateLastPlayDate(id, OnLastPlayDateUpdated));
        Debug.Log("Application is quitting.");

    }



}
