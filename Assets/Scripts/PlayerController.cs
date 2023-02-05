using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject trunkPiecePrefab;
    public GameObject leavesPrefab;
    public GameObject acornPrefab;
    public GameObject arrowPrefab;
    public GameObject mainCamera;
    public CameraController cameraController;
    public float idleRotationSpeed;
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
    public bool isIdle;
    public bool isDirectionTriggered;
    public bool isInDeathZone = false;

    private Rigidbody rb;
    private new SphereCollider collider;
    private GameObject leaves;
    private GameObject acorn;
    private GameObject arrow;
    private enum GrowthState
    {
        idle,
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
    public float horizontalInput = 0;
    public float verticalInput = 0;
    private bool isOnGround;
    
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        trunkSpawnTimer = trunkSpawnTimeInterval;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<SphereCollider>();
        cameraController = mainCamera.GetComponent<CameraController>();
        isOnGround = true; // Start on the ground
        rb.useGravity = false; // No fall
        collider.enabled = false; // No collision
        rb.drag = initialDrag;
        isIdle = true;
        isDirectionTriggered = false;
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
            case GrowthState.idle:
                // 0 wait for space bar
                idlePhase();
                break;
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

    private void idlePhase()
    {
        mainCamera.transform.RotateAround(transform.position, Vector3.up,
            Time.fixedDeltaTime * horizontalInput * idleRotationSpeed);
        if (isSpacePressed)
        {
            isIdle = false;
            growthState = GrowthState.raise;
        }
    }

    private void raisePhase()
    {
        isOnGround = false;
        raiseTimer += Time.fixedDeltaTime;
        if (raiseTimer < raiseDuration)
        {
            // Move player
            Vector3 myForward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z).normalized;
            Vector3 myRight = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z).normalized;
            transform.Translate(Vector3.up * Time.fixedDeltaTime * raiseVerticalSpeed);
            transform.Translate(myForward * Time.fixedDeltaTime * raiseLateralSpeed * verticalInput, Space.World);

            // TODO here
            transform.Translate(myRight * Time.fixedDeltaTime * raiseLateralSpeed * horizontalInput, Space.World);
            //cameraController.currentRotation *= Quaternion.Euler(0, dy, 0);

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
            isDirectionTriggered = true;
            Destroy(arrow);
        }
    }

    private void branchPhase()
    {
        isDirectionTriggered = false;
        branchTimer += Time.fixedDeltaTime;
        if (branchTimer < branchDuration)
        {
            // Move player
            Vector3 myForward = new Vector3(mainCamera.transform.forward.x, 0, mainCamera.transform.forward.z).normalized;
            Vector3 myRight = new Vector3(mainCamera.transform.right.x, 0, mainCamera.transform.right.z).normalized;
            transform.Translate(myForward * Time.fixedDeltaTime * branchForwardSpeed, Space.World);
            transform.Translate(myRight * Time.fixedDeltaTime * branchLateralSpeed * horizontalInput, Space.World);

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
        acorn.transform.rotation = transform.rotation; // Acorn follows player
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
        if (isInDeathZone)
        {
            gameManager.Death();
        }
        transform.rotation = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0);
        growthState = GrowthState.raise;
    }

    private void alignWithCamera()
    {
        transform.rotation = Quaternion.Euler(mainCamera.transform.rotation.x, 0, mainCamera.transform.rotation.z);
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

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Goal":
                gameManager.CompleteLevel();
                break;

            case "GameOverSurface":
                isInDeathZone = true;
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "GameOverSurface":
                isInDeathZone = false;
                break;
        }
    }
}
