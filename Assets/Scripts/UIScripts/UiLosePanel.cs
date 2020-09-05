using System;
using UnityEngine;

public class UiLosePanel : MonoBehaviour
{
    [SerializeField] private GameObject _losePanel;

    private void OnEnable()
    {
        Shield.OnLoseGame += ShieldOnLoseGame;
    }
    
    private void OnDisable()
    {
        Shield.OnLoseGame -= ShieldOnLoseGame;
    }

    private void ShieldOnLoseGame(object sender, EventArgs e)
    {
        _losePanel.SetActive(true);
    }
}
