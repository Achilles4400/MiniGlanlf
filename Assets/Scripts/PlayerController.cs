using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject trunkPiecePrefab;
    public GameObject leavesPrefab;
    public GameObject acornPrefab;
    public GameObject arrowPrefab;
    public float raiseDuration;
    public float branchDuration;
    public float bloomDuration;
    public float growAcornDuration;
    public float raiseVerticalSpeed;
    public float raiseLateralSpeed;
    public float branchForwardSpeed;
    public float branchLateralSpeed;
    public float arrowRotationSpeed;
    public float trunkSpawnTimeInterval;
    public float acornSpawnShift;
    public float stillnessThreshold;
    public float initialDrag;
    public float dragIncreaseExponent;

    private Rigidbody rb;
    private SphereCollider collider;
    private GameObject leaves;
    private GameObject acorn;
    private GameObject arrow;
    private enum GrowthState
    {
        raise,
        bloom,
        enterDirection,
        direction,
        branch,
        growAcorn,
        enterDrop,
        drop,
        reset
    }
    private GrowthState growthState;
    private float raiseTimer = 0;
    private float bloomTimer = 0;
    private float branchTimer = 0;
    private float growAcornTimer = 0;
    private float trunkSpawnTimer = 0;
    private bool isSpacePressed = false;
    private float horizontalInput = 0;
    private float verticalInput = 0;
    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        trunkSpawnTimer = trunkSpawnTimeInterval;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<SphereCollider>();
        isOnGround = true; // Start on ground
        rb.useGravity = false; // No fall
        collider.enabled = false; // No collision
        rb.drag = initialDrag;
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    private void getInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpacePressed = true;
        }
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
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
            case GrowthState.enterDirection:
                // 3 enter direction phase
                enterDirectionPhase();
                break;
            case GrowthState.direction:
                // 4 chose direction
                directionPhase();
                break;
            case GrowthState.branch:
                // 5 grow the branch
                branchPhase();
                break;
            case GrowthState.growAcorn:
                // 6 grow acorn
                growAcornPhase();
                break;
            case GrowthState.enterDrop:
                // 7 enter drop phase
                enterDropPhase();
                break;
            case GrowthState.drop:
                // 8 drop the acorn
                dropPhase();
                break;
            case GrowthState.reset:
                // 9 reset rotation
                resetPhase();
                break;
            default:
                Debug.Log("Vous etes des glands!");
                break;
        }
        isSpacePressed = false;
    }

    private void raisePhase()
    {
        isOnGround = false;
        raiseTimer += Time.fixedDeltaTime;
        if (raiseTimer < raiseDuration)
        {
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
            // End raise phase
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
            // End bloom phase
            growthState = GrowthState.enterDirection;
            bloomTimer = 0;
        }
    }

    private void enterDirectionPhase()
    {
        arrow = Instantiate(arrowPrefab, transform.position, arrowPrefab.transform.rotation);
        growthState = GrowthState.direction; // End enter direction phase
    }

    private void directionPhase()
    {
        arrow.transform.Rotate(Vector3.up * Time.fixedDeltaTime * arrowRotationSpeed);
        if (isSpacePressed)
        {
            // End direction phase
            growthState = GrowthState.branch;
            transform.rotation = arrow.transform.rotation;
            Destroy(arrow);
        }
    }

    private void branchPhase()
    {
        branchTimer += Time.fixedDeltaTime;
        if (branchTimer < branchDuration)
        {
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
                    trunkPiecePrefab, transform.position, transform.rotation);
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
            // End branch Phase
            growthState = GrowthState.growAcorn;
            branchTimer = 0;
            trunkSpawnTimer = 0;
        }
    }

    private void growAcornPhase()
    {
        // Spawn acorn
        if (growAcornTimer == 0)
        {
            Debug.Log("Pop");
            transform.Translate(Vector3.forward * acornSpawnShift);
            acorn = Instantiate(acornPrefab, transform.position, acornPrefab.transform.rotation);
            acorn.transform.localScale = Vector3.zero;
        }
        // Grow acorn
        growAcornTimer += Time.fixedDeltaTime;
        if (growAcornTimer < growAcornDuration)
        {
            // Rescale acorn
            acorn.transform.localScale = acornPrefab.transform.localScale * growAcornTimer / growAcornDuration;
        }
        else
        {
            // End grow acorn phase
            growthState = GrowthState.enterDrop;
            growAcornTimer = 0;
        }
    }

    private void enterDropPhase()
    {
        rb.useGravity = true;
        collider.enabled = true;
        growthState = GrowthState.drop;
    }

    private void dropPhase()
    {
        acorn.transform.position = transform.position; // Acorn follows player
        rb.drag *= dragIncreaseExponent;
        // End drop Phase
        if (isStill())
        {
            rb.drag = initialDrag;
            growthState = GrowthState.reset;
            rb.useGravity = false;
            collider.enabled = false;
        }
    }

    public void resetPhase()
    {
        transform.rotation = Quaternion.identity;
        growthState = GrowthState.raise;
    }

    private bool isStill()
    {
        if (rb.velocity.magnitude < stillnessThreshold && isOnGround)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Collide with ground
            isOnGround = true;
        }
    }
}
