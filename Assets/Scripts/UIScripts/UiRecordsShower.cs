using UnityEngine;

[RequireComponent(typeof(UiRecord))]
public class UiRecordsShower : MonoBehaviour
{
    [SerializeField] private DataContainer _dataContainer;
    [SerializeField] private GameObject _scorePanelPrefab;
    [SerializeField] private GameObject _content;

    private UiRecord _uiRecord;

    private void Start()
    {
        _uiRecord = _scorePanelPrefab.GetComponent<UiRecord>();

        short count = 0;
        foreach (var record in _dataContainer.GetRecords())
        {
            count++;
            _uiRecord.SetData(count, record);
            Instantiate(_scorePanelPrefab, _content.transform);
        }
    }
}
