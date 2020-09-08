using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Saver : MonoBehaviour
{
    private static RecordsSaveData _recordsSaveData = new RecordsSaveData();
    private const string _recordKey = "RecordKey";

    /// <summary>
    /// Adding new score
    /// </summary>
    /// <param name="score"></param>
    public static void AddRecord(int score)
    {
        _recordsSaveData.AddRecord(score);
    }

    /// <summary>
    /// Get Records list
    /// </summary>
    /// <returns></returns>
    public static List<int> GetRecords()
    {
        return _recordsSaveData.Records;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(_recordKey))
        {
            _recordsSaveData = JsonUtility.FromJson<RecordsSaveData>(PlayerPrefs.GetString(_recordKey));

            foreach (var record in GetRecords())
            {
                Debug.Log(record);
            }
        }
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        PlayerPrefs.SetString(_recordKey, JsonUtility.ToJson(_recordsSaveData));
        PlayerPrefs.Flush();
    }
#endif
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(_recordKey, JsonUtility.ToJson(_recordsSaveData));
        PlayerPrefs.Flush();
    }
}

[Serializable]
public class RecordsSaveData
{
    public List<int> Records = new List<int>();
    private int _maxRecordsCount = 5; 
    
    public void AddRecord(int score)
    {
        // Проверка на нахождение рекорда с тем же количеством очков в памяти
        // Если такой рекорд уже есть - выходим из метода
        foreach (var record in Records)
        {
            if (record == score)
            {
                return;
            }
        }

        Records.Add(score);
        Records.Sort();
        Records.Reverse();

        // В списке рекордов не может быть больше чем {_maxRecordsCount} позиций
        if (Records.Count > _maxRecordsCount)
        {
            Records.Remove(Records.Last());
        }
    }
    
}