using UnityEngine;
using UnityEngine.Serialization;

public class DistanceSteeringBehaviour : MonoBehaviour
{
    [Header("Téléport")] [SerializeField] private float teleportFactor = 1f;
    
    [Header("Flee")] [SerializeField] private float fleeFactor = 1f; /*va vere le support*/
    private Flee _flee;

    public float TeleportFactor
    {
        get => teleportFactor;
        set => teleportFactor = value;
    }

    public float FleeFactor
    {
        get => fleeFactor;
        set => fleeFactor = value;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _flee = GetComponent<Flee>();
    }

    // Update is called once per frame
    private void Update()
    {
        _flee.enabled = fleeFactor > 0;
    }
}
