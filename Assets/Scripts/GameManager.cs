using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject successLevelUI;

    //Distance threshold
    [SerializeField] private int[] distThresholds = new int[4]; //To set in editor
    private Transform playerPos, goalPost;
    [SerializeField] public int closeness;
    [SerializeField] private float distance;

    private void Start()
    {
        playerPos = GameObject.Find("Player").transform;
        goalPost = GameObject.Find("Goal").transform;
    }



    private void OnDrawGizmos()
    {
        foreach(float dist in distThresholds)
        {
            Gizmos.DrawWireSphere(goalPost.position, dist);
        }

    }


    private void distanceManagement()
    {
        int i = 0;
        // closeness = 0 -> Close; closeness = 3 -> Far
        distance = Mathf.Abs((playerPos.position - goalPost.position).magnitude);
        

        foreach(int distThresh in distThresholds)
        {
            if(distance <= distThresh)
            {
                break;
            }
            i++;
        }
        closeness = i;
        
    }


    public void CompleteLevel()
    {
        Debug.Log("Level Complete!");
        successLevelUI.SetActive(true);
    }


    private void FixedUpdate()
    {
        distanceManagement();
    }
}
