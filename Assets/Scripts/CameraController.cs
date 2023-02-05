using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public Quaternion currentRotation;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = player.transform.position - transform.position;
    }

    private void LateUpdate()
    {
        if (player.isDirectionTriggered)
        {
            float desiredAngle = player.transform.eulerAngles.y;
            currentRotation = Quaternion.Euler(0, desiredAngle, 0);
        }
        if (!player.isIdle)
        {
            transform.position = player.transform.position - (currentRotation * offset);
            transform.LookAt(player.transform);
        }
    }
}
