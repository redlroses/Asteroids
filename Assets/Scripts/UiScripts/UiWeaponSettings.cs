using UnityEngine;

public class UiWeaponSettings : MonoBehaviour
{
    [SerializeField] private DataContainer _dataContainer;
    [SerializeField] private UiDisplayText _currentWeaponText;
    
    public void SetCannon()
    {
        _dataContainer.SetChosenWeaponByType<Cannon>();
        _currentWeaponText.DisplayText(nameof(Cannon));
    }    
    
    public void SetLaser()
    {
        _dataContainer.SetChosenWeaponByType<Laser>();
        _currentWeaponText.DisplayText(nameof(Laser));
    }
}
