using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;


namespace GameUtils
{
    public class ImageDownloader
    {
        public static void RequestDownload(MonoBehaviour currentBehaviour ,string url, Action<Texture2D> onComplete)
        {
            currentBehaviour.StartCoroutine(DownloadProfileImg(url, onComplete));
        }
        public static IEnumerator DownloadProfileImg(string url, Action<Texture2D> onComplete)
        {
            UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(url);
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error in download :: " + unityWebRequest.error);
            }
            if (unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Access Denied ::  " + unityWebRequest.error);
                onComplete(null);
            }
            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Download Successfully");
                Texture2D tex = ((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture;
                onComplete(tex);
            }
        }

    }

}
