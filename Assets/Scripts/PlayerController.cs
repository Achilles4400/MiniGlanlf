using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject trunkPiecePrefab;
    public float raiseHeight;
    public float raiseVerticalSpeed;
    public float raiseLateralSpeed;
    public float trunkSpawnTimeInterval;

    private float timer = 0;
    private float horizontalInput = 0;
    private float verticalInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        timer = trunkSpawnTimeInterval;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update is called EXACTLY once per frame
    private void FixedUpdate()
    {
        raiseTree();
    }

    private void raiseTree()
    {
        if (transform.position.y < raiseHeight)
        {
            // Get controller key
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // Move player
            transform.Translate(Vector3.up * Time.fixedDeltaTime * raiseVerticalSpeed);
            transform.Translate(Vector3.right * Time.fixedDeltaTime * raiseLateralSpeed * horizontalInput);
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * raiseLateralSpeed * verticalInput);

            // Spawn trunk piece
            timer += Time.fixedDeltaTime; // Increment trunk piece spawn timer
            if (timer >= trunkSpawnTimeInterval)
            {
                // Reset trunk piece spawn timer
                timer = 0;
                // Instantiate trunk piece
                GameObject newTrunkPiece = Instantiate(
                    trunkPiecePrefab, transform.position, trunkPiecePrefab.transform.rotation);
                // Rescale new trunk piece
                float scaleFactor = 0.2f + 0.8f * (1 - transform.position.y / raiseHeight);
                newTrunkPiece.transform.localScale = new Vector3(
                    trunkPiecePrefab.transform.localScale.x * scaleFactor,
                    trunkPiecePrefab.transform.localScale.y,
                    trunkPiecePrefab.transform.localScale.z * scaleFactor);
            }
        }
    }
}
