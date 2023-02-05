using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject successLevelUI;

    private audioManager audioManager;

    //Distance threshold
    [SerializeField] private int[] distThresholds = new int[3]; //To set in editor
    private Transform playerPos, goalPos;
    [SerializeField] public int closeness;
    [SerializeField] private float distance;

    private void Start()
    {
        audioManager = GameObject.Find("Sound Manager").GetComponent<audioManager>();
        playerPos = GameObject.Find("GG").transform;
        goalPos = GameObject.Find("Goal").transform;
    }



    private void OnDrawGizmos()
    {
        foreach(float dist in distThresholds)
        {
            Gizmos.DrawWireSphere(goalPos.position, dist);
        }

    }


    private void distanceManagement()
    {
        int i = 0;
        // closeness = 0 -> Close; closeness = 3 -> Far
        distance = Mathf.Abs((playerPos.position - goalPos.position).magnitude);
        

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
        foreach(AudioSource aud in audioManager.speaker)
        {
            aud.enabled = false;
        }
    }


    private void FixedUpdate()
    {
        distanceManagement();
    }
}
