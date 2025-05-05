// Project : My_RogueLike
// Script by : Nanatchy

using Edgar.Unity;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    #region Attributs

    private GameObject test;
    
	private DungeonGeneratorGrid2D generator;
	private bool _goodGenerator = false;
	private ChageEtage _ui;
	private EnemyManager _enemyManager;

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
		    _ui.AddEtage();
		    _ui.SetEtage();
		    _ui.ActiveSwitchDungeon();
		    generator.Generate();
		    _enemyManager.DestroyAllEnemy();
	    }
    }

    private void Start()
    {
	    do
	    {
		    var targetList = GameObject.FindGameObjectWithTag("UI");
		    
		    if (targetList != null)
		    {
			    _ui = targetList.GetComponent<ChageEtage>();
		    }
	    } while (_ui == null);
    }
	
    private void Update()
    {
	    if (!_goodGenerator)
	    {
		    var targetList = GameObject.FindGameObjectsWithTag("GeneratorDungeon");
		    
		    if (targetList != null)
		    {
			    var rand = Random.Range(0, targetList.Length);
			    
			    generator = targetList[rand].GetComponent<DungeonGeneratorGrid2D>();

			    if (generator != null)
			    {
				    _goodGenerator = true;
			    }
		    }
	    }
    }
	
	#endregion
}
