using UnityEngine;
using System.Collections;

public class RotationAroundPlanet : MonoBehaviour
{

    public Transform target;
    public float distance = 10.0f;

    // ZoomCameraMouse
    public float MouseWheelSensitivity = 5;
    public float MouseZoomMin = 1;
    public float MouseZoomMax = 7;

    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20;
    public float yMaxLimit = 80;

    private double x = 0.0f;
    private double y = 0.0f;

    Quaternion rotation;
    Vector3 position;

    public float smoothTime = 0.3f;

    private float xSmooth = 0.0f;
    private float ySmooth = 0.0f;
    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;

    private Vector3 posSmooth = Vector3.zero;
    //private Vector3 posVelocity = Vector3.zero;

    bool mousePressed = false;

    // Use this for initialization
    void Start()
    {
        Cursor.visible = true;

        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Make the rigid body not change rotation
        if (GetComponent<Rigidbody>())
            GetComponent<Rigidbody>().freezeRotation = true;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
            mousePressed = true;
        else if (Input.GetMouseButtonUp(0))
            mousePressed = false;
    }

    void LateUpdate()
    {

        if (target && mousePressed)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;

            xSmooth = Mathf.SmoothDamp(xSmooth, (float)x, ref xVelocity, smoothTime);
            ySmooth = Mathf.SmoothDamp(ySmooth, (float)y, ref yVelocity, smoothTime);

            ySmooth = ClampAngle(ySmooth, yMinLimit, yMaxLimit);

            rotation = Quaternion.Euler(ySmooth, xSmooth, 0);

            // posSmooth = Vector3.SmoothDamp(posSmooth,target.position,posVelocity,smoothTime);

            posSmooth = target.position; // no follow smoothing

            transform.rotation = rotation;
            transform.position = rotation * new Vector3(0.0f, 0.0f, -distance) + posSmooth;
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {

            if (distance >= MouseZoomMin && distance <= MouseZoomMax)
            {

                distance -= Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity;

                if (distance < MouseZoomMin) { distance = MouseZoomMin; }
                if (distance > MouseZoomMax) { distance = MouseZoomMax; }
            }
        }

        rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler((float)y, (float)x, 0), Time.deltaTime * 3);

        position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
