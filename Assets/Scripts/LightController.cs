using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public GameObject dirLight;
    public float lightRotationSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        dirLight.transform.Rotate(Vector3.right * Time.fixedDeltaTime * lightRotationSpeed);
        //dirLight.GetComponent<Light>().transform.Rotate(Vector3.right * Time.fixedDeltaTime * lightRotationSpeed);
        //dirLight.GetComponent<Light>().intensity = intensityMagnitude();
    }

    private float intensityMagnitude()
    {
        float xRot = dirLight.transform.rotation.eulerAngles.x;
        Debug.Log(dirLight.transform.rotation.eulerAngles.x + " " + dirLight.transform.localEulerAngles.x);

        //Debug.Log(xRot + " -> " + Mathf.Cos(0.01745329251f * xRot));
        return (1 + Mathf.Cos(0.01745329251f * xRot)) / 2;
    }
}
