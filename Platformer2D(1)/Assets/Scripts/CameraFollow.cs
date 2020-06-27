using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;

    Vector3 targetPos;
    public Transform target;

    Vector3 finalPos;

    public Vector2 limitsY;

    //public float followSpeed;

    //void Update()
    //{
    //   targetPos = Vector2.Lerp(transform.position, target.position, Time.deltaTime * followSpeed * Mathf.Abs(target.position.x - transform.position.x));
    //}

    private void Update()
    {
        targetPos = target.position;
    }

    private void LateUpdate()
    {
        finalPos = targetPos + offset;
        finalPos.y = Mathf.Clamp(finalPos.y, limitsY.x, limitsY.y);
        transform.position = finalPos;
    }
}
