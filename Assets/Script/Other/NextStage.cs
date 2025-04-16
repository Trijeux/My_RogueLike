// Project : My_RogueLike
// Script by : Nanatchy

using Edgar.Unity;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    #region Attributs

	private DungeonGeneratorGrid2D generator;
	private bool _goodGenerator = false;

    #endregion

    #region Methods



    #endregion

    #region Behaviors

    private void OnTriggerEnter2D(Collider2D other)
    {
	    if (other.CompareTag("Player"))
	    {
		    generator.Generate();
	    }
    }

    private void Start()
    {
	    
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
