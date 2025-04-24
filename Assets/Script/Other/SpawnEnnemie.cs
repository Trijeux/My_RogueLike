// Project : My_RogueLike
// Script by : Nanatchy

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnnemie : MonoBehaviour
{
    #region Attributs

    [SerializeField] private float minTimeSpawn;
    [SerializeField] private float maxTimeSpawn;

    [SerializeField] private List<GameObject> enemy;

    private bool _timeSetForNextSpawn = false;

    private float _timeRand;

    private float _timerSpawn;

    private bool _FirstSpawn = false;

    private EnemyManager _enemyManager;
    
    private string _lastSpawn = "C";

    private string _spawn;
    
    private List<KeyValuePair<string, MarkovLinkEnemy>> _markovEnemy = new List<KeyValuePair<string, MarkovLinkEnemy>>();

    private float _timerFistSpawn = 0;
    
    #endregion

    #region Methods

    private void SpawnEnemy()
    {
        if (!_timeSetForNextSpawn)
        {
            _timeRand = Random.Range(minTimeSpawn, maxTimeSpawn);
            _timeSetForNextSpawn = true;
        }
        else
        {
            _timerSpawn += Time.deltaTime;
        }

        if (_timerSpawn > _timeRand)
        {
            _timerSpawn = 0;
            _timeSetForNextSpawn = false;
            Generate();
            Instantiate(enemy[StringToInt()], gameObject.transform.position, quaternion.identity, _enemyManager.gameObject.transform);
        }


    }

    private int StringToInt()
    {
        return _spawn switch
        {
            "C" => 0,
            "D" => 1,
            "S" => 2,
            _ => 0
        };
    }
    
    public void Generate()
    {
        var currentSpawn = _lastSpawn;
        var availableSpawn = new List<KeyValuePair<string, MarkovLinkEnemy>>();
        
        availableSpawn = _markovEnemy
            .Where(n => n.Key == currentSpawn)
            .OrderByDescending(n => n.Value.Weight)
            .ToList();

        var sumWeights = availableSpawn.Sum(n => n.Value.Weight);

        if (availableSpawn.Count > 0)
        {
            var idxElement = Random.Range(0, sumWeights);
            var partialsum = 0;

            foreach (var availableSpawns in availableSpawn)
            {
                partialsum += availableSpawns.Value.Weight;
                if (idxElement < partialsum)
                {
                    //AvailableNames;
                    currentSpawn = availableSpawns.Value.Enemy;
                    break;
                }
            }

            _lastSpawn = currentSpawn;
            _spawn = currentSpawn;
        }
    }
    
    private void CreatCondition()
    {
        _markovEnemy.Add(new KeyValuePair<string, MarkovLinkEnemy>("C", new MarkovLinkEnemy("C")));
        _markovEnemy.Add(new KeyValuePair<string, MarkovLinkEnemy>("C", new MarkovLinkEnemy("D", 2)));
        _markovEnemy.Add(new KeyValuePair<string, MarkovLinkEnemy>("C", new MarkovLinkEnemy("S", 3)));

        _markovEnemy.Add(new KeyValuePair<string, MarkovLinkEnemy>("D", new MarkovLinkEnemy("C", 3)));
        _markovEnemy.Add(new KeyValuePair<string, MarkovLinkEnemy>("D", new MarkovLinkEnemy("D")));
        _markovEnemy.Add(new KeyValuePair<string, MarkovLinkEnemy>("D", new MarkovLinkEnemy("S",2)));

        _markovEnemy.Add(new KeyValuePair<string, MarkovLinkEnemy>("S", new MarkovLinkEnemy("C", 2)));
        _markovEnemy.Add(new KeyValuePair<string, MarkovLinkEnemy>("S", new MarkovLinkEnemy("D", 3)));
        _markovEnemy.Add(new KeyValuePair<string, MarkovLinkEnemy>("S", new MarkovLinkEnemy("S")));
    }

    public void SetEnemyManager(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }
    
    #endregion

    #region Behaviors

    private void Start()
    {
        CreatCondition();
        switch (Random.Range(0, 2))
        {
            case 0:
                _lastSpawn = "C";
                break;
            case 1:
                _lastSpawn = "D";
                break;
        }
    }

    private void Update()
    {
        if (_enemyManager != null)
        {
            if (!_FirstSpawn)
            {
                if (_timerFistSpawn > 2f)
                {
                    Generate();
                    Instantiate(enemy[StringToInt()], gameObject.transform.position, quaternion.identity, _enemyManager.gameObject.transform);
                    _FirstSpawn = true;
                    _timerFistSpawn = 0;
                }
                _timerFistSpawn += Time.deltaTime;
            }
        
            SpawnEnemy();
        }
    }

    #endregion
}
