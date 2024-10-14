using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    public float speed = 1.0f;                // Movement speed
    public float rotationSpeed = 2.0f;        // Rotation speed for turning toward the target
    private Vector3 targetPosition;           // Target position to move to
    private bool shouldMove = false;          // Should the robot move?
    private bool isTurning = false;           // Is the robot currently turning?
    
    private Animator animator;                // Reference to the Animator component

    void Start()
    {
        // Get the Animator component attached to the robot
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isTurning)
        {
            // Rotate the robot towards the target while in idle
            RotateTowardsTarget();

            // Once the rotation is done, stop turning and start walking
            if (IsFacingTarget())
            {
                isTurning = false;
                shouldMove = true;
                // Start the walking animation once the robot is facing the target
                animator.SetBool("IsWalking", true);
            }
        }

        if (shouldMove)
        {
            // Move the robot towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Check if the robot has reached the target position
            if (transform.position == targetPosition)
            {
                shouldMove = false;
                // Stop the walking animation when the robot reaches the target
                animator.SetBool("IsWalking", false);
            }
        }
    }

    // Method to set the new target and trigger rotation (not movement yet)
    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        isTurning = true;  // Begin turning to face the target
        shouldMove = false; // Ensure movement is disabled until rotation is complete

        // Ensure the robot is in idle state while turning
        animator.SetBool("IsWalking", false);
    }

    // Method to rotate the robot towards the target position
    private void RotateTowardsTarget()
    {
        // Calculate the direction to the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculate the target rotation to face the target
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the target using Slerp (optional for smoother rotation)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    // Method to check if the robot is approximately facing the target
    private bool IsFacingTarget()
    {
        // Calculate the direction to the target
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Check if the forward vector of the robot is almost aligned with the direction to the target
        float dotProduct = Vector3.Dot(transform.forward, direction);

        // If the dot product is close to 1, the robot is facing the target
        return dotProduct > 0.99f;  // Adjust threshold for precision
    }
}
