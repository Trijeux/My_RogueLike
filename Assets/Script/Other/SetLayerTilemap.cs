// Project : My_RogueLike
// Script by : Nanatchy


using System.Collections;
using Script.Player;
using UnityEngine;

public class SetLayerTilemap : MonoBehaviour
{
    #region Attributs

    private Transform _tilemaps;
    private Transform _wall;
    private Transform _collideable;
    private bool pathGood;
    [SerializeField] private PlayerMove _playerMove;
    [SerializeField] private ChageEtage _ui;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject backGround;
    [SerializeField] private string tagObstacle;
    
    #endregion
    
    
    #region Methods

    private void SherchTileMap()
    {
	    bool goodTilemaps = false;
	    do
	    {
		    _tilemaps = transform.Find("Tilemaps");
		    if (_tilemaps != null)
		    {
			    bool goodWallAndCollid = false;
			    do
			    {
				    _wall = _tilemaps.Find("Walls");
				    _collideable = _tilemaps.Find("Collideable");
				    if (_wall != null && _collideable != null)
				    {
					    int layerIndex = Mathf.RoundToInt(Mathf.Log(_layerMask.value, 2));
					    
					    _wall.gameObject.layer = layerIndex;
					    _wall.gameObject.tag = tagObstacle;
					    _collideable.gameObject.layer = layerIndex;
					    _collideable.gameObject.tag = tagObstacle;
					    
					    goodWallAndCollid = true;
				    }
			    } while (!goodWallAndCollid);
			    
			    goodTilemaps = true;
		    }
	    } while (!goodTilemaps);
    }

    #endregion

    #region Behaviors
	
	private void Start()
	{
		SherchTileMap();
	}
	
    private void Update()
    {
	    if (_tilemaps == null || _wall == null || _collideable == null)
	    {
		    SherchTileMap();
		    pathGood = false;
	    }

	    
	    int layerIndex = Mathf.RoundToInt(Mathf.Log(_layerMask.value, 2));
	    if (_wall.gameObject.layer == layerIndex && _collideable.gameObject.layer == layerIndex && !pathGood)
	    {
		    StartCoroutine("WaitAndScan");
		    pathGood = true;
	    }
    }
    
    private IEnumerator WaitAndScan()
    {
	    _playerMove.SetActiveInput(false);
	    yield return new WaitForSeconds(2f);
	    Instantiate(backGround, gameObject.transform);
	    AstarPath.active.Scan();
	    _ui.DeactiveSwitchDungeon();
	    _playerMove.SetActiveInput(true);
    }
	
	#endregion
}
