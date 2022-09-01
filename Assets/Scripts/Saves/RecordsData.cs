using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class RecordsData
{
    private readonly int _maxRecordsCount = 99;
    
    public List<int> Records = new List<int>();

    public void AddRecord(int score)
    {
        Records.Add(score);
        Records.Sort();
        Records.Reverse();
        
        if (Records.Count > _maxRecordsCount)
        {
            Records.Remove(Records.Last());
        }
    }
}