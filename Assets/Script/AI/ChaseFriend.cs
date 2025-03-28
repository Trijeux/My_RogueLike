using System;
using UnityEngine;
using Pathfinding;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ChaseFriend : MonoBehaviour
{
    private EnnemieManager _ennemieManager;
    private AIPath aiPath;
    [SerializeField] private float moveSpeed;
    private float distanceToTarget;
    [SerializeField] private float stoppingDistanceThreshold;
    private bool isGoodDistanceForGrap;
    private CapsuleCollider2D _collider2DTrigger;

    public bool IsGoodDistanceForGrap => isGoodDistanceForGrap;

    public Transform Target { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        aiPath = GetComponent<AIPath>();
        var enemyManager = GameObject.FindGameObjectWithTag("EnemyManager");
        _ennemieManager = enemyManager.GetComponent<EnnemieManager>();
        _collider2DTrigger = GetComponentInChildren<CapsuleCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Target == null && _ennemieManager.Monsters.Count > 0)
        {
            isGoodDistanceForGrap = false;
            var isValideEnemy = false;
            var numberTest = 0;
            do
            {
                var random = Random.Range(0, _ennemieManager.Monsters.Count);
                if (_ennemieManager.Monsters[random].CompareTag("Support"))
                {
                    numberTest++;
                }
                else
                {
                    var targetChild = _ennemieManager.Monsters[random].GetComponent<ActiveChild>();
                    if (!targetChild.supportIsHere)
                    {
                        Target = _ennemieManager.Monsters[random].GetComponent<Transform>();
                        isValideEnemy = true;
                    }
                }
                if (numberTest >= 100)
                {
                    isValideEnemy = true;
                }
            } while (!isValideEnemy);
        }
        else if(Target != null)
        {
            aiPath.maxSpeed = moveSpeed;
        
            distanceToTarget = Vector3.Distance(transform.position, Target.position);
            if (distanceToTarget > stoppingDistanceThreshold && !isGoodDistanceForGrap)
            {
            
                aiPath.destination = Target.position;
                isGoodDistanceForGrap = false;
          
            }
            else
            {
                transform.position = Target.position + new Vector3(0.5f,0.5f,0);
                _collider2DTrigger.enabled = false;
                isGoodDistanceForGrap = true;
            }
        }
    }
}
