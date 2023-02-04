using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameManager gameManager;
    private List<Transform> goalPos = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            goalPos.Add(transform.GetChild(i));
        }

        transform.Translate(goalPos[Random.Range(0, goalPos.Count)].position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision trigger");
        switch (other.tag)
        {
            case "Player":
                gameManager.CompleteLevel();
                break;

            default:
                Debug.Log("Collision triggered");
                break;
        }
    }
}
