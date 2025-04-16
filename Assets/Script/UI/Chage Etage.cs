// Project : My_RogueLike
// Script by : Nanatchy

using System;
using TMPro;
using UnityEngine;

public class ChageEtage : MonoBehaviour
{
    #region Attributs

    [SerializeField] private GameObject blackScreen;
    [SerializeField] private TextMeshProUGUI dungeonEtage;
    [SerializeField] private TextMeshProUGUI caveEtage;
    private int etage = 0;

    #endregion

    #region Methods

    public void SetEtage()
    {
        etage++;
        dungeonEtage.text = $"Dungeon\nE. {etage}";
        caveEtage.text = $"Cave\nE. {etage}";
    }

    public void ActiveSwitchDungeon()
    {
        blackScreen.SetActive(true);
        dungeonEtage.gameObject.SetActive(true);
    }

    public void DeactiveSwitchDungeon()
    {
        blackScreen.SetActive(false);
        dungeonEtage.gameObject.SetActive(false);
    }
    
    public void ActiveSwitchCave()
    {
        blackScreen.SetActive(true);
        caveEtage.gameObject.SetActive(true);
    }

    public void DeactiveSwitchCave()
    {
        blackScreen.SetActive(false);
        caveEtage.gameObject.SetActive(false);
    }
    
    #endregion
}
