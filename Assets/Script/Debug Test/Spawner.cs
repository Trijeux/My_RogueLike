using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyManager;
    
    [SerializeField] private GameObject spawnerCac;
    [SerializeField] private GameObject spawnerDistance;
    [SerializeField] private GameObject spawnerSupport;

    [SerializeField] private GameObject cacEnemy;
    [SerializeField] private GameObject distanceEnemy;
    [SerializeField] private GameObject SupportEnemy;

    public void SpawnCac()
    {
        Instantiate(cacEnemy, spawnerCac.transform.position, Quaternion.identity, enemyManager.transform);
    }
    
    public void SpawnDistance()
    {
        Instantiate(distanceEnemy, spawnerDistance.transform.position, Quaternion.identity, enemyManager.transform);
    }

    public void SpawnSupport()
    {
        Instantiate(SupportEnemy, spawnerSupport.transform.position, Quaternion.identity, enemyManager.transform);
    }
    
    public void AllDispawn()
    {
        for (int i = enemyManager.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(enemyManager.transform.GetChild(i).gameObject);
        }
    }
}
