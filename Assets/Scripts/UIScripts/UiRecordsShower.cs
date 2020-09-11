using UnityEngine;

public class UiRecordsShower : MonoBehaviour
{
    [SerializeField] private GameObject _scorePanelPrefab;
    [SerializeField] private GameObject _content;

    private UiRecord _uiRecord;

    private void Start()
    {
        _uiRecord = _scorePanelPrefab.GetComponent<UiRecord>();

        short count = 0;
        foreach (var record in DataSaver.GetRecords())
        {
            count++;
            _uiRecord.SetData(count, record);
            Instantiate(_scorePanelPrefab, _content.transform);
        }
    }
}
