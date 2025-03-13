
using System.Linq;
using UnityEngine;

public class GameConstants
{
    public const int MAX_QUESTION_COUNT = 32;
    public const int PASS_MARKS = 68;
    public const int MAX_GAME_POINT = 74;
    public static string GetFileExtensionFromUrl(string url)
    {
        url = url.Split('?')[0];
        url = url.Split('/').Last();
        return url.Contains('.') ? url.Substring(url.LastIndexOf('.')) : "";
    }

    public static int GetSimpleQuestionsPoint(int count)
    {
        if(count <= 0)
            return 0;
        else if(count <= 10)
            return 3;
        else if(count <= 16)
            return 2;
        else
            return 1;

    }

    public static int GetSpecialQuestionsPoint(int count)
    {
        if (count <= 0) return 0;
        else if (count <= 6)
            return 3;
        else if (count <= 10)
            return 2;
        else
            return 1;
    }

    public static Color GetColorFromHexCode(string hexCode)
    {
        Color color;

        ColorUtility.TryParseHtmlString(hexCode, out color);

        return color;
    }

}
