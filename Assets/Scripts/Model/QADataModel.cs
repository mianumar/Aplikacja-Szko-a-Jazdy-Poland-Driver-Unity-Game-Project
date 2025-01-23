using System.Collections.Generic;

public class QADataModel
{
    public int QID { get; set; }
    public string Question { get; set; }
    public List<string> Options { get; set; }
    public string CorrectAns { get; set; }
}

public class QADatabaseModel
{
    public int MaxTotalPoints { get; set; }
    public int MinPointsToPass { get; set; }

    public List<QADataModel> QADataList { get; set; }
}
