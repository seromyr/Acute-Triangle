using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_7_DynamicObstacles : MonoBehaviour
{
    private GameObject[] obstacles;
    private float[] obstacleTimer;

    public float delay, checkPoint;

    private void Start()
    {
        checkPoint = 2f;

        obstacles = new GameObject[transform.childCount];
        obstacleTimer = new float[obstacles.Length];

        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i] = transform.GetChild(i).gameObject;
            obstacleTimer[i] = Random.Range(0.1f, 2f);
            obstacles[i].SetActive(Random.value > 0.6f);
        }
    }

    private void Update()
    {
        for (int i = 0; i < obstacleTimer.Length; i++)
        {
            if (obstacleTimer[i] > 0)
            {
                obstacleTimer[i] -= (Time.deltaTime * Random.Range(0.1f, 2f));

                if (obstacleTimer[i] <= 0)
                {
                    obstacleTimer[i] = Random.Range(2f, 3f);
                    obstacles[i].SetActive(Random.value > 0.6f);
                }
            }
        }
    }
}
