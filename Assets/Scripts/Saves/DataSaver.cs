using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DataSaver : MonoBehaviour
{
    public static int WeaponClass
    {
        get => _weaponClass;
        set => _weaponClass = value;
    }
    
    private static RecordsSaveData _recordsSaveData = new RecordsSaveData();
    private static int _weaponClass = 1;
    
    private const string RecordKey = "RecordKey";
    private const string ChosenWeapon = "ChoosenWeapon";
    

    /// <summary>
    /// Returns high score on local leader board
    /// </summary>
    /// <returns></returns>
    public static int GetHighRecord()
    {
        if (_recordsSaveData.Records.Count > 0)
        {
            return _recordsSaveData.Records[0];
        }

        return 0;
    }
    
    /// <summary>
    /// Adding new score
    /// </summary>
    /// <param name="score"></param>
    public static void AddRecord(int score)
    {
        _recordsSaveData.AddRecord(score);
        
        foreach (var record in GetRecords())
        {
            Debug.Log(record);
        }
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
        if (PlayerPrefs.HasKey(RecordKey))
        {
            _recordsSaveData = JsonUtility.FromJson<RecordsSaveData>(PlayerPrefs.GetString(RecordKey));
            
            foreach (var record in GetRecords())
            {
                Debug.Log(record);
            }
        }

        if (PlayerPrefs.HasKey(ChosenWeapon))
        {
            _weaponClass = PlayerPrefs.GetInt(ChosenWeapon, 1);
        }
    }

    private void Save()
    {
        PlayerPrefs.SetString(RecordKey, JsonUtility.ToJson(_recordsSaveData));
        PlayerPrefs.SetInt(ChosenWeapon, _weaponClass);
        PlayerPrefs.Flush();
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }
#endif
    
    private void OnApplicationQuit()
    {
        Save();
    }
}

[Serializable]
public class RecordsSaveData
{
    public List<int> Records = new List<int>();
    private int _maxRecordsCount = 99;

    public void AddRecord(int score)
    {
        Debug.Log("Add new record");

        // Проверка на нахождение рекорда с тем же количеством очков в памяти
        // Если такой рекорд уже есть - выходим из метода
        foreach (var record in Records)
        {
            if (record == score)
            {
                return;
            }
        }

        // Сортировка по убыванию при добавлении нового рекорда
        Records.Add(score);
        Records.Sort();
        Records.Reverse();

        // В списке рекордов не может быть больше чем {_maxRecordsCount} позиций
        if (Records.Count > _maxRecordsCount)
        {
            Debug.Log("Remove");
            Records.Remove(Records.Last());
        }
    }
}
