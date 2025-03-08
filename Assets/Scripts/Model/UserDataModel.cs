
using System.Collections.Generic;

public class UserDataModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }

    public GameSettings UserGameSettings { get; set; }

    public SummaryData UserSummaryData { get; set; }

}

public class GameSettings
{
    public float VolumePercent { get; set; }
    public bool IsSoundOn { get; set; }
    public bool IsLightMode { get; set; }
}

public class SummaryData
{
    public int TotalCorrectAns { get; set; }
    public int TotalWrongAns { get; set; }

    public int TotalSkipedAns { get; set; }

    public float TotalDurations { get; set; }

    public int PointsAchieved { get; set; }

    public float PercentAchieved { get; set; }

    public List<QuestionAttempted> QuestionAttempedList { get; set; }

    public SummaryData(int totalCorrectAns, int totalWrongAns, int totalSkipedAns, float totalDurations,
        int pointsAchieved, float percentAchieved, List<QuestionAttempted> questionAttempedList)
    {
        TotalCorrectAns = totalCorrectAns;
        TotalWrongAns = totalWrongAns;
        TotalSkipedAns = totalSkipedAns;
        TotalDurations = totalDurations;
        PointsAchieved = pointsAchieved;
        PercentAchieved = percentAchieved;
        QuestionAttempedList = questionAttempedList;
    }

    public SummaryData() { }
}

public class QuestionAttempted
{
    public int QusetionNo { get; set; }
    public ResultType resultType { get; set; }

    public QuestionAttempted(int QuestionNo, ResultType resultType)
    {
        this.QusetionNo = QuestionNo;
        this.resultType = resultType; 
    }

}
