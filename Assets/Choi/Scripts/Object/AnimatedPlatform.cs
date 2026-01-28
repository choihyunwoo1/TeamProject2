using UnityEngine;

public class AnimatedPlatform : MonoBehaviour
{
    public Vector3 DeltaMovement { get; private set; }

    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        DeltaMovement = transform.position - lastPosition;
        lastPosition = transform.position;
    }
}
