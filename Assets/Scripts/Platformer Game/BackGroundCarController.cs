using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundCarController : MonoBehaviour
{
    [SerializeField]
    GameObject[] spawn, cars, destination;
    int carsId, spawnerId;
    float spawnTime, speed;

    public void Travel()
    {
        spawnerId = Random.Range(0, 2);
        carsId = Random.Range(0, 3);
        spawnTime = Random.Range(5.0f, 16.0f);
        speed = Random.Range(5.0f, 10.0f);
        GameObject temp = Instantiate(cars[carsId]);
        temp.transform.position = spawn[spawnerId].transform.position;
        temp.transform.localScale = spawn[spawnerId].transform.localScale;
        temp.GetComponent<SpriteRenderer>().sortingOrder = spawnerId + 2;
        temp.GetComponent<GoTo>().destination = destination[spawnerId];
        temp.GetComponent<GoTo>().speed = speed;
        StartCoroutine(SpawnCars(spawnTime));
    }

    IEnumerator SpawnCars(float spawnTime)
    {
        yield return new WaitForSeconds(spawnTime);
        Travel();
        StopCoroutine(SpawnCars(spawnTime));
    }
}
