using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] levelPrefabs;
    [SerializeField] private float timeBetweenSpawn=6f;
    private bool canCreate = true;
    private void Start()
    {
        PlayerMover.onDeath += StopCreate;
        CreateNewPart();
    }
    private void Update()
    {
        //
    }
    private void CreateNewPart()
    {
        var newPart = Instantiate(levelPrefabs[Random.Range(0, 3)]);
        newPart.transform.position = gameObject.transform.position;
        Observable.Timer(System.TimeSpan.FromSeconds(timeBetweenSpawn)).TakeUntilDisable(this).Where(x=>canCreate).Subscribe(x => CreateNewPart());
    }
    private void StopCreate()
    {
        canCreate = false;
        //this.enabled = false;
        //Destroy(gameObject);
    }
}
