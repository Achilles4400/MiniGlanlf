using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject trunkPiecePrefab;
    public float raiseDuration; // The trunk grows during raiseDuration seconds
    public float branchDuration; // The branch grows during branchDuration seconds
    public float raiseVerticalSpeed;
    public float raiseLateralSpeed;
    public float trunkSpawnTimeInterval;

    private enum growthState
    {
        raise,
        branch,
        drop
    }
    private float raiseTimer = 0;
    private float trunkSpawnTimer = 0;
    private float horizontalInput = 0;
    private float verticalInput = 0;

    // Start is called before the first frame update
    void Start()
    {
        trunkSpawnTimer = trunkSpawnTimeInterval;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Update is called EXACTLY once per frame
    private void FixedUpdate()
    {
        raisePhase();
        //branchPhase();
        //dropPhase();
    }

    private void raisePhase()
    {
        raiseTimer += Time.fixedDeltaTime;
        if (raiseTimer < raiseDuration)
        {
            // Get controller key
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // Move player
            transform.Translate(Vector3.up * Time.fixedDeltaTime * raiseVerticalSpeed);
            transform.Translate(Vector3.right * Time.fixedDeltaTime * raiseLateralSpeed * horizontalInput);
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * raiseLateralSpeed * verticalInput);

            // Spawn trunk piece
            trunkSpawnTimer += Time.fixedDeltaTime;
            if (trunkSpawnTimer >= trunkSpawnTimeInterval)
            {
                // Reset trunk piece spawn timer
                trunkSpawnTimer = 0;
                // Instantiate trunk piece
                GameObject newTrunkPiece = Instantiate(
                    trunkPiecePrefab, transform.position, trunkPiecePrefab.transform.rotation);
                // Rescale new trunk piece
                float scaleFactor = 0.2f + 0.8f * (1 - raiseTimer / raiseDuration);
                newTrunkPiece.transform.localScale = new Vector3(
                    trunkPiecePrefab.transform.localScale.x * scaleFactor,
                    trunkPiecePrefab.transform.localScale.y,
                    trunkPiecePrefab.transform.localScale.z * scaleFactor);
            }
        }
    }
    private void branchPhase()
    {
        //horizontalInput = Input.GetAxis("Horizontal");
    }
    private void dropPhase()
    {

    }
}
