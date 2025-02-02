using System.Collections.Generic;

[System.Serializable]
public class QADataModel
{
    public int QID { get; set; }
    public string Question { get; set; }
    public string QuestionType { get; set; }
    public int ReadingTime { get; set; }
    public int AnsTime { get; set; }
    public int Points { get; set; }
    public List<string> Options { get; set; }
    public string CorrectAns { get; set; }
    public string ImageUrl { get; set; }
    public string VideoUrl { get; set; }
}
[System.Serializable]
public class QADatabaseModel
{
    public int MaxTotalPoints { get; set; }
    public int MinPointsToPass { get; set; }

    public List<QADataModel> QADataList { get; set; }
}
