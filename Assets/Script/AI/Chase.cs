using UnityEngine;
using Pathfinding;
using UnityEngine.Serialization;

public class Chase : MonoBehaviour
{
    private AIPath aiPath;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;
    private float distanceToTarget;
    [SerializeField] private float stoppingDistanceThreshold;

    /*Ne pourra pas être là a la fin*/ [Header("Temp Serializ")] [SerializeField] public bool IsHit = false;
    public bool IsGoodDistanceForAttack = false;

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
        if (distanceToTarget < stoppingDistanceThreshold)
        {
            //Chase when the player is far
            aiPath.destination = transform.position;
            IsGoodDistanceForAttack = true;
            //Chase when player is near
            // aiPath.destination = target.position;
        }
        else
        {
            //Chase when the player is far
            
            aiPath.destination = target.position;
            IsGoodDistanceForAttack = false;
            //Chase when player is near
            // aiPath.destination=transform.position;
        }
    }
}
