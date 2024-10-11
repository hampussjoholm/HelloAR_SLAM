using UnityEngine;

public class RobotMovement : MonoBehaviour
{
    public float speed = 1.0f;            // Speed of the robot
    public Vector3 targetPosition;        // Target position for the robot to move towards
    private bool shouldMove = false;      // Should the robot be moving

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            // Move the robot towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Stop moving if the robot reaches the target position
            if (transform.position == targetPosition)
            {
                shouldMove = false;
            }
        }
    }

    // This method sets the target position and initiates movement
    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        shouldMove = true;
    }
}
