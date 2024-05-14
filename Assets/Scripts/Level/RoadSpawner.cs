using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private float timeBetweenSpawn=6f;
    [SerializeField] private GameObject finalPart;
    private bool canCreate = true;
    private int spawnedParts;
    private bool islast;
    private void Start()
    {
        PlayerMover.onDeath += StopCreate;
        CreateNewPart();
    }
    private void Update()
    {
        if  (spawnedParts>7)
        {
            islast = true;
        }
    }
    private GameObject GetPartToSpawn()
    {
        if (islast)
        {
            StopCreate();
            return finalPart;
        }
        else
        {
            return levelPrefabs[Random.Range(0, levelPrefabs.Length)];
        }
    }
    private void CreateNewPart()
    {
        var newPart = Instantiate(GetPartToSpawn());
        newPart.transform.position = gameObject.transform.position;
        Observable.Timer(System.TimeSpan.FromSeconds(timeBetweenSpawn)).TakeUntilDisable(this).Where(x=>canCreate).Subscribe(x => CreateNewPart());
        spawnedParts++;
    }
    private void StopCreate()
    {
        canCreate = false;
        //this.enabled = false;
        //Destroy(gameObject);
    }
}
