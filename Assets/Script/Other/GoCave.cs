// Project : My_RogueLike
// Script by : Nanatchy

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GoCave : MonoBehaviour
{
    #region Attributs

    [SerializeField] private Transform spawnPoint;
    private Cave _cave;
    private bool _goodGenerator = false;
    private EnemyManager _enemyManager;
    public Transform SpawnPoint => spawnPoint;

    #endregion

    #region Methods
    public void SetEnemyManager(EnemyManager enemyManager)
    {
	    _enemyManager = enemyManager;
    }

    #endregion

    #region Behaviors
	
    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (other.CompareTag("Player"))
	    {
		    _enemyManager.DestroyAllEnemy();
		    _cave.GetPlayerAndEXit(other.gameObject, spawnPoint);
		    _cave.EnterCave();
	    }
    }
    
	private void Start()
    {
    }
	
    private void Update()
    {
	    if (!_goodGenerator)
	    {
		    var targetList = GameObject.FindGameObjectWithTag("GeneratorCave");
		    
		    Debug.Log(targetList.name);
		    
		    if (targetList != null)
		    {
			    _cave = targetList.GetComponent<Cave>();

			    if (_cave != null)
			    {
				    _goodGenerator = true;
			    }
		    }
	    }
    }
	
	#endregion
}
