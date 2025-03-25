using UnityEngine;
using UnityEngine.Serialization;

public class SupportSteeringBehaviour : MonoBehaviour
{
    [Header("Walk")] [SerializeField] private float walkFactor = 1f; /*vers L'ennemie le plus proche et sans bouclier*/
    
    [Header("Flee")] [SerializeField] private float fleeFactor = 1f; /*lache un bombe et sprint*/
    private Flee _flee;

    public float WalkFactor
    {
        get => walkFactor;
        set => walkFactor = value;
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
