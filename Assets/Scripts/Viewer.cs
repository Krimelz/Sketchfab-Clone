using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewer : MonoBehaviour
{
    public float distanceToObject = -5f;
    [Space]
    public float rotationSpeed = 5f;
    public float shiftSpeed = 1f;
    public float scaleSpeed = 1f;
    [Space]
    public float rotationLerp = 5f;
    public float shiftLerp = 5f;
    public float scaleLerp = 5f;

    private Quaternion rotation;
    private Vector3 shift;
    private Camera mainCamera;
    private float rotationX;
    private float rotationY;
    private float shiftX;
    private float shiftY;
    private float oldDistance;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 0)
        {
            oldDistance = -1f;
        }
        if (Input.touchCount == 1)
        {
            Rotate();
        }
        else if (Input.touchCount == 2)
        {
            Vector2 firstTouchDelta = Input.GetTouch(0).deltaPosition;
            Vector2 secondTouchDelta = Input.GetTouch(1).deltaPosition;

            float dot = Vector2.Dot(firstTouchDelta.normalized, secondTouchDelta.normalized);

            if (dot <= -0.8f)
            {
                Scale();
            }
            else if (dot >= 0.8f)
            {
                Shift();
            }
        }

        transform.position = Vector3.Lerp(
            transform.position,
            shift, 
            shiftLerp * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotation,
            rotationLerp * Time.deltaTime);

        mainCamera.transform.localPosition = new Vector3(
            0f,
            0f,
            Mathf.Lerp(mainCamera.transform.localPosition.z, distanceToObject, scaleLerp * Time.deltaTime));
    }

    private void Rotate()
    {
        rotationX -= Input.GetTouch(0).deltaPosition.y * rotationSpeed * Time.deltaTime;
        rotationY += Input.GetTouch(0).deltaPosition.x * rotationSpeed * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    private void Scale()
    {
        Vector2 firstTouchPos = Input.GetTouch(0).position;
        Vector2 secondTouchPos = Input.GetTouch(1).position;

        float newDistance = Vector2.Distance(firstTouchPos, secondTouchPos);

        if (oldDistance > 0f)
        {
            distanceToObject -= (oldDistance - newDistance) * scaleSpeed * Time.deltaTime;
        }

        if (distanceToObject >= -0.05f)
        {
            distanceToObject = -0.05f;
        }

        oldDistance = newDistance;
    }

    private void Shift()
    {
        shiftX += Input.GetTouch(0).deltaPosition.x * shiftSpeed * Time.deltaTime;
        shiftY += Input.GetTouch(0).deltaPosition.y * shiftSpeed * Time.deltaTime;

        shift = rotation * new Vector3(-shiftX, -shiftY, 0f);
    }
}
