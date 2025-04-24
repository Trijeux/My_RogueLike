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
        
        
        var nextStage = levelRootGameObject.GetComponentsInChildren<NextStage>();

        nextStage[0].SetEnemyManager(enemyManager);
        
        var goCaves = levelRootGameObject.GetComponentsInChildren<GoCave>();

        if (goCaves.Length > 0)
        {
            foreach (var goCave in goCaves)
            {
                goCave.SetEnemyManager(enemyManager);
            }
        }
    }
}
