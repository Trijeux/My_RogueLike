using UnityEngine;
using UnityEngine.Serialization;

public class SupportSteeringBehaviour : MonoBehaviour
{
    [Header("Chase")] [SerializeField] private float chaseFactor = 1f;
    private Chase _chase;
    
    [Header("Flee")] [SerializeField] private float fleeFactor = 1f; /*va vere le support*/
    private Flee _flee;

    public float ChaseFactor
    {
        get => chaseFactor;
        set => chaseFactor = value;
    }

    public float FleeFactor
    {
        get => fleeFactor;
        set => fleeFactor = value;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _chase = GetComponent<Chase>();
        _flee = GetComponent<Flee>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (chaseFactor > 0)
        {
            _chase.enabled = true;
        }
        else
        {
            _chase.enabled = false;
        }
        _flee.enabled = fleeFactor > 0;
    }
}
