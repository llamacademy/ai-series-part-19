using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject FollowTarget;
    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private Vector3 Offset;

    private void Update()
    {
        Camera.transform.position = FollowTarget.transform.position + Offset;
    }
}
