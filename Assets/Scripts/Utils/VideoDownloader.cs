using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace GameUtils
{
    public class VideoDownloader
    {
        public static void RequestDownload(MonoBehaviour monoBehaviour, string url , string savePath , Action<bool> result)
        {
            monoBehaviour.StartCoroutine(DownloadVideo(url,savePath , result));
        }

        private static IEnumerator DownloadVideo(string url, string savePath, Action<bool> result)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
            unityWebRequest.downloadHandler = new DownloadHandlerFile(savePath);
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error in download :: " + unityWebRequest.error);
            }
            if (unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Access Denied ::  " + unityWebRequest.error);
                result(false);
            }
            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Video Download Successfully");
                result(true);
                
            }
        }
    }
}
