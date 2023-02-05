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
        player = GameObject.Find("GG").transform;
    }

    private void floatingPlayer()
    {
        player.gameObject.GetComponent<Rigidbody>().drag = 0.01f;
        if (player.position.y - this.transform.position.y < 0)
        {
            player.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, buoyancy, 0f), ForceMode.Acceleration);
        }
        player.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 0f, -streamForce), ForceMode.Force);
    }

    private void OnTriggerStay(Collider other)
    {
        switch(other.tag)
        {
            case "Player":
                floatingPlayer();
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        switch (other.tag)
        {
            case "Player":
                //player.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
