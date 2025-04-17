// Project : My_RogueLike
// Script by : Nanatchy

using System;
using System.Collections;
using UnityEngine;

public class Cave : MonoBehaviour
{
    #region Attributs

    [SerializeField] private GameObject dungeon;
    [SerializeField] private GameObject cave;
    [SerializeField] private ChageEtage _ui;

    private Transform _exitSpawn;
    private GeneratorPcg _generatorPcg;
    private GameObject _player;
    

    #endregion

    #region Methods
    
    public void EnterCave()
    {
	    _generatorPcg.StartGeneration();
	    if (_generatorPcg.CurrentEscalier.TryGetComponent(out ExitCave exitCave))
	    {
		    _player.transform.position = exitCave.SpawnPoint.position;
	    }
	    else
	    {
		    _player.transform.position = _generatorPcg.CurrentEscalier.transform.position;
	    }
	    dungeon.SetActive(false);
	    cave.SetActive(true);
	    StartCoroutine("WaitAndScanEnter");
    }

    public void ExitCave()
    {
	    cave.SetActive(false);
	    dungeon.SetActive(true);
	    _player.transform.position = _exitSpawn.position;
	    StartCoroutine("WaitAndScanExit");
    }
    
    public void GetPlayerAndEXit(GameObject player, Transform exitSpawn)
    {
	    _player = player;
	    _exitSpawn = exitSpawn;
    }
    
    #endregion

    #region Behaviors
	
	private void Start()
	{
		_generatorPcg = GetComponent<GeneratorPcg>();
	}
    
    private IEnumerator WaitAndScanEnter()
    {
	    _ui.ActiveSwitchCave();
	    yield return new WaitForSeconds(2f);
	    AstarPath.active.Scan();
	    _ui.DeactiveSwitchCave();
    }
    
    private IEnumerator WaitAndScanExit()
    {
	    _ui.ActiveSwitchDungeon();
	    yield return new WaitForSeconds(2f);
	    AstarPath.active.Scan();
	    _ui.DeactiveSwitchDungeon();
    }
	
	#endregion
}
