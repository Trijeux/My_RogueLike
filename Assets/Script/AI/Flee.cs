using UnityEngine;
using Pathfinding;
using UnityEngine.Serialization;

public class Flee : MonoBehaviour
{
    private AIPath aiPath;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;
    private float distanceToTarget;
    [SerializeField] private float stoppingDistanceThreshold;
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
        distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > stoppingDistanceThreshold)
        {
            //Chase when the player is far
            aiPath.destination = transform.position;
            
            //Chase when player is near
            // aiPath.destination = target.position;
        }
        else
        {
            //Chase when the player is far
            var destination = transform.position - target.position;
            
            aiPath.destination = destination;
            
            //Chase when player is near
            // aiPath.destination=transform.position;
        }
    }
}
