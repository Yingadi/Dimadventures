using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform target;

    Rigidbody2D targetRb;

    SpriteRenderer targetSr;

    [Range(1, 10), SerializeField]
    float smoothFactorX = 1;

    [Range(1, 10), SerializeField]
    float smoothFactorY = 1;

    [SerializeField]
    float horizontalOffset;

    int horizontalOffsetDirection;

    [SerializeField]
    float verticalOffset;

    [SerializeField]
    Vector2 minValues, maxValues;

    Transform camTransform;

    bool cameraIsMoving = false;

    void FindPlayer()
    {
        camTransform = Camera.main.transform;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null && players.Length > 0)
        {
            target = players[0].transform;
            targetSr = players[0].GetComponent<SpriteRenderer>();
            targetRb = players[0].GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            FindPlayer();
        }
        else
        {
            Follow();
        }
    }

    void Follow()
    {
        horizontalOffsetDirection = targetSr.flipX ? -1 : 1;
        Vector3 followPosition = new Vector3(target.position.x + horizontalOffset * horizontalOffsetDirection, target.position.y + verticalOffset, camTransform.position.z);
        Vector3 boundPosition = new Vector3(
            Math.Clamp(followPosition.x, minValues.x, maxValues.x),
            Math.Clamp(followPosition.y, minValues.y, maxValues.y),
            camTransform.position.z);
        float smoothXPosition = Mathf.Lerp(camTransform.position.x, boundPosition.x, Math.Clamp(smoothFactorX - Math.Abs(targetRb.velocity.x), smoothFactorX, 10) * Time.fixedDeltaTime);
        float smoothYPosition = Mathf.Lerp(camTransform.position.y, boundPosition.y, (smoothFactorY + Math.Abs(targetRb.velocity.y)) * Time.fixedDeltaTime);
        Vector3 smoothPosition = new Vector3(smoothXPosition, smoothYPosition, camTransform.position.z);
        camTransform.position = smoothPosition;
        if (Math.Abs(camTransform.position.x - boundPosition.x) < 1.5f && Math.Abs(camTransform.position.y - boundPosition.y) < 1.5f) cameraIsMoving = false;
        else cameraIsMoving = true;
    }

    public bool CameraIsMoving()
    {
        return cameraIsMoving;
    }

    public void ChangeLimits(float newMinLimitX, float newMinLimitY, float newMaxLimitX, float newMaxLimitY)
    {
        minValues.x = newMinLimitX;
        minValues.y = newMinLimitY;
        maxValues.x = newMaxLimitX;
        maxValues.y = newMaxLimitY;
    }
}
