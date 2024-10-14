using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class ARPlaceObject : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The prefab to be placed or moved.")]
        GameObject m_PrefabToPlace;

        [SerializeField]
        [Tooltip("The Scriptable Object Asset that contains the ARRaycastHit event.")]
        ARRaycastHitEventAsset m_RaycastHitEvent;

        GameObject m_SpawnedObject;

        public GameObject prefabToPlace
        {
            get => m_PrefabToPlace;
            set => m_PrefabToPlace = value;
        }

        public GameObject spawnedObject
        {
            get => m_SpawnedObject;
            set => m_SpawnedObject = value;
        }

        void OnEnable()
        {
            if (m_RaycastHitEvent == null || m_PrefabToPlace == null)
            {
                Debug.LogWarning($"{nameof(ARPlaceObject)} component on {name} has null inputs and will have no effect in this scene.", this);
                return;
            }

            if (m_RaycastHitEvent != null)
                m_RaycastHitEvent.eventRaised += PlaceObjectAt;
        }

        void OnDisable()
        {
            if (m_RaycastHitEvent != null)
                m_RaycastHitEvent.eventRaised -= PlaceObjectAt;
        }

        // This method is triggered when the raycast hit event occurs
        void PlaceObjectAt(object sender, ARRaycastHit hitPose)
        {
            // If no robot has been placed yet, spawn one
            if (m_SpawnedObject == null)
            {
                // Instantiate the robot at the hit position but do not move it yet
                m_SpawnedObject = Instantiate(m_PrefabToPlace, hitPose.pose.position, hitPose.pose.rotation, hitPose.trackable.transform.parent);

                // No need to call SetTarget here. The robot will stay still.
            }
            else
            {
                // If a robot has already been spawned, just move it to the new hit position
                RobotMovement robotMovement = m_SpawnedObject.GetComponent<RobotMovement>();
                if (robotMovement != null)
                {
                    // Move the robot to the new target location (the new AR plane touch)
                    robotMovement.SetTarget(hitPose.pose.position);
                }
            }
        }
    }
}
