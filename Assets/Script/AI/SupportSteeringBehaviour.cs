using UnityEngine;
using UnityEngine.Serialization;

public class SupportSteeringBehaviour : MonoBehaviour
{
    [Header("ChaseFriend")] [SerializeField] private float chaseFriendFactor = 1f;
    private ChaseFriend _chaseFriend;
    
    [Header("Attach")] [SerializeField] private float attachFactor = 1f;

    public float ChaseFactor
    {
        get => chaseFriendFactor;
        set => chaseFriendFactor = value;
    }

    public float FleeFactor
    {
        get => attachFactor;
        set => attachFactor = value;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _chaseFriend = GetComponent<ChaseFriend>();
    }
}
