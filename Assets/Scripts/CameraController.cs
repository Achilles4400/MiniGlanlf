using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;

    private Quaternion currentRotation;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        currentRotation = Quaternion.Euler(0, 0, 0);
        offset = player.transform.position - transform.position;
    }

    private void LateUpdate()
    {
        if (player.isDirectionTriggered)
        {
            // Align with player
            currentRotation = Quaternion.Euler(0, player.transform.eulerAngles.y, 0);
        }
        if (!player.isIdle)
        {
            transform.position = player.transform.position - (currentRotation * offset);
            transform.LookAt(player.transform);
        }
    }

    public void addRotation(Quaternion q)
    {
        currentRotation *= q;
    }
}
