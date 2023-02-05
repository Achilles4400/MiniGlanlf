using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water : MonoBehaviour
{

    private Transform player;
    public float buoyancy;
    public float streamForce;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Sphere").transform;
    }

    private void floatingPlayer()
    {
        if (player.position.y - this.transform.position.y < 0)
        {
            player.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, buoyancy, 0f), ForceMode.Acceleration);
        }
        player.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(streamForce, 0f, 0f), ForceMode.Force);
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.tag);
        switch(other.tag)
        {
            case "Player":
                floatingPlayer();
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
