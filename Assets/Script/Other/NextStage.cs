// Project : My_RogueLike
// Script by : Nanatchy

using Edgar.Unity;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    #region Attributs

	private DungeonGeneratorGrid2D generator;
	private bool _goodGenerator = false;
	private ChageEtage _ui;

    #endregion

    #region Methods



    #endregion

    #region Behaviors

    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (other.CompareTag("Player"))
	    {
		    _ui.SetEtage();
		    _ui.ActiveSwitchDungeon();
		    generator.Generate();
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
		    var targetList = GameObject.FindGameObjectsWithTag("Generator");
		    
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
