using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private List<Transform> goalPos = new List<Transform>();
    private Transform goal;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        
        goal = transform.GetChild(0);
        for (int i = 1; i < transform.childCount; i++)
        {
            goalPos.Add(transform.GetChild(i));
        }

        goal.Translate(goalPos[Random.Range(0, goalPos.Count)].position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "GameOverSurface":
                Debug.Log("game over");
                break;

            case "Player":
                Debug.Log("success");
                
                break;

            default:
                break;
        }
    }
}
