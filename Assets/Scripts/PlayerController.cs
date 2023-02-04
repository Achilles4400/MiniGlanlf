using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject trunkPiecePrefab; 
    public GameObject leavesPrefab;
    public float raiseDuration; // The trunk grows during raiseDuration seconds
    public float bloomDuration; // The trunk grows during raiseDuration seconds
    public float branchDuration; // The branch grows during branchDuration seconds
    public float raiseVerticalSpeed;
    public float raiseLateralSpeed;
    public float branchForwardSpeed;
    public float branchLateralSpeed;
    public float trunkSpawnTimeInterval;
    
    private enum GrowthState
    {
        raise,
        bloom,
        branch,
        drop
    }
    private GrowthState growthState;
    private float raiseTimer = 0;
    private float bloomTimer = 0;
    private float branchTimer = 0;
    private float trunkSpawnTimer = 0;
    private GameObject leaves;
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
        switch (growthState)
        {
            case GrowthState.raise:
                // 1 raise the trunk
                raisePhase();
                break;
            case GrowthState.bloom:
                // 2 grow the leaves
                bloomPhase();
                break;
            case GrowthState.branch:
                // 3 grow the branch
                branchPhase();
                break;
            case GrowthState.drop:
                // 4 drop the acorn
                dropPhase();
                break;
            default:
                Debug.Log("Vous etes des glands!");
                break;
        }
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
        else
        {
            growthState = GrowthState.bloom;
            raiseTimer = 0;
            trunkSpawnTimer = 0;
        }
    }
    
    private void bloomPhase()
    {
        // Spawn leaves
        if (bloomTimer == 0)
        {
            leaves = Instantiate(leavesPrefab, transform.position, leavesPrefab.transform.rotation);
            leaves.transform.localScale = Vector3.zero;
        }
        // Grow leaves
        bloomTimer += Time.fixedDeltaTime;
        if (bloomTimer < bloomDuration)
        {
            // Rescale new trunk piece
            leaves.transform.localScale = leavesPrefab.transform.localScale * bloomTimer / bloomDuration;
        }
        else
        {
            growthState = GrowthState.branch;
            bloomTimer = 0;
        }
    }
    private void branchPhase()
    {
        branchTimer += Time.fixedDeltaTime;
        if (branchTimer < branchDuration)
        {
            // Get controller key
            horizontalInput = Input.GetAxis("Horizontal");

            // Move player
            transform.Translate(Vector3.forward * Time.fixedDeltaTime * branchForwardSpeed);
            transform.Translate(Vector3.right * Time.fixedDeltaTime * branchLateralSpeed * horizontalInput);

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
                float scaleFactor = 0.1f + 0.1f * (1 - branchTimer / branchDuration);
                newTrunkPiece.transform.localScale = new Vector3(
                    trunkPiecePrefab.transform.localScale.x * scaleFactor,
                    trunkPiecePrefab.transform.localScale.y,
                    trunkPiecePrefab.transform.localScale.z * scaleFactor);
                // Rotate new trunk piece
                newTrunkPiece.transform.Rotate(new Vector3(90, 0, 0));
            }
        }
        else
        {
            growthState = GrowthState.drop;
            branchTimer = 0;
            trunkSpawnTimer = 0;
        }
    }

    private void dropPhase()
    {

    }
}
