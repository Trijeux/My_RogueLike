using System;
using Edgar.Unity;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerProcesse", menuName = "Scriptable Objects/SpawnerProcesse")]
public class SpawnerProcesse : DungeonGeneratorPostProcessingGrid2D
{ 
    private EnemyManager enemyManager;
    

    public override void Run(DungeonGeneratorLevelGrid2D level)
    {
        var target = FindObjectsByType<EnemyManager>(FindObjectsSortMode.InstanceID);
        if (target.Length > 1)
        {
            Debug.LogError("Trop d'EnemyManager");
        }
        enemyManager = target[0];
        
        
        var levelRootGameObject = level.RootGameObject;
        
        var spawnPoints = levelRootGameObject.GetComponentsInChildren<SpawnEnnemie>();

        foreach (var spawnPoint in spawnPoints)
        {
            spawnPoint.SetEnemyManager(enemyManager);
        }
    }
}
