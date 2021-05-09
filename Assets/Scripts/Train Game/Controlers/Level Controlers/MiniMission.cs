using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMission : MonoBehaviour
{
    public int countMis;    // Количество миссий
    public GameObject stationPref;  // Префаб станции

    private Vector2Int gridSize;
    private bool[,] aviableGrid;

    bool inMission;         // Миссия идет

    void Start()
    {
        gridSize = Camera.main.GetComponent<BuildingsGrid>().gridSize;
        aviableGrid = new bool[gridSize.x, gridSize.y];
        int countCells = gridSize.x * gridSize.y;
        int tempCountMis = countMis;



        for (int i = 0; i<gridSize.x; i++)
            for (int j = 0; j<gridSize.y; j++)
            {
                aviableGrid[i, j] = true;
            }

        for (int i = 0; i < gridSize.x; i++)
            for (int j = 0; j < gridSize.y; j++)
            {
                if (aviableGrid[i, j])
                {
                    int rand = Random.Range(0, countCells);
                    if (rand<tempCountMis)
                    {
                        tempCountMis--;
                        Camera.main.GetComponent<BuildingsGrid>().InstantiateBuild(i, j, stationPref.GetComponent<Building>());

                        AviableAround(i, j);
                    }

                    countCells--;
                }
            }

        inMission = false;
    }


    void Update()
    {
        if (!inMission && countMis > 0)
        {
            //Camera.main.GetComponent<RespawnMode>().RandomRespawn();
            inMission = true;
        }
    }


    void AviableAround(int i, int j)
    {
        if (i == 0 && j == 0)
        {
            aviableGrid[1, 0] = false;
            aviableGrid[1, 1] = false;
            aviableGrid[0, 1] = false;
        }
        else if (i == 0 && j == gridSize.y - 1)
        {
            aviableGrid[1, j] = false;
            aviableGrid[1, j - 1] = false;
            aviableGrid[0, j - 1] = false;
        }

        else if (i == gridSize.x - 1 && j == 0)
        {
            aviableGrid[i - 1, 0] = false;
            aviableGrid[i - 1, 0] = false;
            aviableGrid[1, 0] = false;
        }
        else if (i == gridSize.x - 1 && j == gridSize.y - 1)
        {
            aviableGrid[i - 1, j] = false;
            aviableGrid[i - 1, j - 1] = false;
            aviableGrid[i, j - 1] = false;
        }
        else if (i == 0)
        {
            aviableGrid[0, j - 1] = false;
            aviableGrid[1, j - 1] = false;
            aviableGrid[1, j] = false;
            aviableGrid[1, j + 1] = false;
            aviableGrid[0, j + 1] = false;
        }
        else if (i == gridSize.x - 1)
        {
            aviableGrid[i, j - 1] = false;
            aviableGrid[i - 1, j - 1] = false;
            aviableGrid[i - 1, j] = false;
            aviableGrid[i - 1, j + 1] = false;
            aviableGrid[i, j + 1] = false;
        }
        else if (j == 0)
        {
            aviableGrid[i - 1, 0] = false;
            aviableGrid[i - 1, 1] = false;
            aviableGrid[i, 1] = false;
            aviableGrid[i + 1, 1] = false;
            aviableGrid[i + 1, 0] = false;
        }
        else if (j == gridSize.y - 1)
        {
            aviableGrid[i - 1, j] = false;
            aviableGrid[i - 1, j - 1] = false;
            aviableGrid[i, j - 1] = false;
            aviableGrid[i + 1, j - 1] = false;
            aviableGrid[i + 1, j] = false;
        }
        else
        {
            aviableGrid[i - 1, j - 1] = false;
            aviableGrid[i - 1, j] = false;
            aviableGrid[i - 1, j + 1] = false;
            aviableGrid[i, j + 1] = false;
            aviableGrid[i + 1, j + 1] = false;
            aviableGrid[i + 1, j] = false;
            aviableGrid[i + 1, j - 1] = false;
            aviableGrid[i, j - 1] = false;
        }
    }


    public void TrainOnStation()
    {
        inMission = false;
        countMis--;
    }
}