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

    public int Etage { get; private set; } = -1;

    #endregion

    #region Methods

    public void AddEtage()
    {
        Etage++;
    }
    
    public void SetEtage()
    {
        dungeonEtage.text = $"Dungeon\nE. {Etage}";
        caveEtage.text = $"Cave\nE. {Etage}";
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
