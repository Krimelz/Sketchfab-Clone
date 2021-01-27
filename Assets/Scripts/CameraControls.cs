using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform cameraTarget;
    public float distanceToObject = 5f;
    public float rotationSpeed = 5f;
    public float shiftSpeed = 5f;
    public float scaleSpeed = 5f;

    private Quaternion rotation;
    private Vector3 offset;
    private Vector3 endPos;
    private float rotationX;
    private float rotationY;
    private float shiftX;
    private float shiftY;
    private float oldDistance;

    void Start()
    {
        offset.z = distanceToObject;
        endPos = transform.position = cameraTarget.position - offset;
        transform.LookAt(cameraTarget);
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Rotate();
        }
        else if (Input.touchCount == 2)
        {
            Vector2 firstTouchDelta = Input.GetTouch(0).deltaPosition;
            Vector2 secondTouchDelta = Input.GetTouch(1).deltaPosition;

            if (firstTouchDelta.magnitude <= 5f)
                firstTouchDelta = Vector2.zero;

            if (secondTouchDelta.magnitude <= 5f)
                secondTouchDelta = Vector2.zero;

            float dot = Vector2.Dot(firstTouchDelta.normalized, secondTouchDelta.normalized);

            if (dot <= -0.4f)
            {
                Scale();
            }
            else if (dot >= 0.6f)
            {
                Shift();
            }  
        }

        transform.position = Vector3.Lerp(transform.position, endPos, rotationSpeed * Time.deltaTime);
        transform.LookAt(cameraTarget);
        cameraTarget.rotation = transform.rotation;
    }

    private void Rotate()
    {
        rotationX -= Input.GetTouch(0).deltaPosition.y * rotationSpeed * Time.deltaTime;
        rotationY += Input.GetTouch(0).deltaPosition.x * rotationSpeed * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -89f, 89f);

        rotation = Quaternion.Euler(rotationX, rotationY, 0f);

        endPos = cameraTarget.position - (rotation * offset);
    }

    private void Scale()
    {
        Vector2 firstTouchPos = Input.GetTouch(0).position;
        Vector2 secondTouchPos = Input.GetTouch(1).position;

        float newDistance = Vector2.Distance(firstTouchPos, secondTouchPos);

        if (oldDistance > newDistance)
        {
            offset.z += scaleSpeed * Time.deltaTime;
        }
        else if (oldDistance < newDistance)
        {
            offset.z -= scaleSpeed * Time.deltaTime;
        }

        if (offset.z <= 0.05f)
        {
            offset.z = 0.05f;
        }

        endPos = cameraTarget.position - (rotation * offset);
        oldDistance = newDistance;
    }   

    private void Shift()
    {
        shiftX = Input.GetTouch(0).deltaPosition.x * shiftSpeed * Time.deltaTime;
        shiftY = Input.GetTouch(0).deltaPosition.y * shiftSpeed * Time.deltaTime;

        Vector3 shift = new Vector3(shiftX, shiftY, 0f);

        transform.Translate(-shift);
        cameraTarget.Translate(-shift);

        endPos = transform.position;
    }
}
