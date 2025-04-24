using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private int currentMonster = 0;

    public List<GameObject> Monsters { get; } = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMonster != transform.childCount)
        {
            if (currentMonster > transform.childCount)
            {
                currentMonster = 0;
            }
            Monsters.Clear();
            for (int i = 0; i <= currentMonster; i++)
            {
                Monsters.Add(transform.GetChild(i).gameObject);
            }
            currentMonster = transform.childCount;
        }
    }

    public void DestroyAllEnemy()
    {
        foreach (var monster in Monsters)
        {
           Destroy(monster);
        }
    }
}
