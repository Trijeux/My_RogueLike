using System;
using UnityEngine;
using Pathfinding;
using UnityEngine.Serialization;

public class Chase : MonoBehaviour
{
    private AIPath aiPath;
    [SerializeField] private float moveSpeed;
    private float distanceToTarget;
    [SerializeField] private float stoppingDistanceThreshold;
    private bool isGoodDistanceForAttack;

    public bool IsGoodDistanceForAttack => isGoodDistanceForAttack;

    public Transform Target { get; private set; }

    private void Awake()
    {
        var targetList = GameObject.FindGameObjectsWithTag("Player");
        Target = targetList[0].GetComponent<Transform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        aiPath = GetComponent<AIPath>();
    }

    // Update is called once per frame
    private void Update()
    {
        
        aiPath.maxSpeed = moveSpeed;
        //Move to target position
        //aiPath.destination = target.position;
        distanceToTarget = Vector3.Distance(transform.position, Target.position);
        if (distanceToTarget < stoppingDistanceThreshold)
        {
            //Chase when the player is far
            aiPath.destination = transform.position;
            isGoodDistanceForAttack = true;
            //Chase when player is near
            // aiPath.destination = target.position;
        }
        else
        {
            //Chase when the player is far
            
            aiPath.destination = Target.position;
            isGoodDistanceForAttack = false;
            //Chase when player is near
            // aiPath.destination=transform.position;
        }
    }
}
