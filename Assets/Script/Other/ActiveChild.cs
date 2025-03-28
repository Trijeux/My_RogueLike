using UnityEngine;
using UnityEngine.Serialization;

public class ActiveChild : MonoBehaviour
{
    [SerializeField] private GameObject child;
    public bool supportIsHere = false;
    
    public GameObject Child => child;
    public void Active()
    {
        child.SetActive(true);
    }
    
    public void Deactive()
    {
        child.SetActive(false);
    }
}
