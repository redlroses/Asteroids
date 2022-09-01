using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataContainer : MonoBehaviour
{
    private const string RecordKey = "RecordKey";
    private const string ChosenWeapon = "ChoosenWeapon";
    
    private RecordsData _recordsData;
    private string _weaponName;

    private void Awake()
    {
        _recordsData = PlayerPrefs.HasKey(RecordKey)
            ? JsonUtility.FromJson<RecordsData>(PlayerPrefs.GetString(RecordKey))
            : new RecordsData();
        
        _weaponName = PlayerPrefs.GetString(ChosenWeapon, typeof(Cannon).ToString());
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnDestroy()
    {
        Save();
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
    
    public int GetHighRecord()
    {
        if (_recordsData.Records.Count > 0)
        {
            return _recordsData.Records.First();
        }

        return 0;
    }

    public void AddRecord(int score)
    {
        _recordsData.AddRecord(score);
    }

    public List<int> GetRecords()
    {
        return new List<int>(_recordsData.Records);
    }

    public Type GetChosenWeapon()
    {
        return Type.GetType(_weaponName, true);
    }

    public void SetChosenWeaponByType<T>() where T : Weapon
    {
        _weaponName = typeof(T).ToString();
    }

    private void Save()
    {
        PlayerPrefs.SetString(RecordKey, JsonUtility.ToJson(_recordsData));
        PlayerPrefs.SetString(ChosenWeapon, _weaponName);
        PlayerPrefs.Save();
    }
}