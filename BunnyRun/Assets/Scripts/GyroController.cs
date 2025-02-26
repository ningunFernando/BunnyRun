using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : MonoBehaviour
{
    [Header("Gyroscope attributes")]
    public float speed = 5f;
    [Tooltip("Higher Values made it less sensible")]public float tiltSensibility;

    private bool gyroEnabled;
    private Gyroscope gyro;
    private Quaternion baseRotation;

    private void Start()
    {
        gyroEnabled = SystemInfo.supportsGyroscope;

        if (gyroEnabled)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            baseRotation = gyro.attitude;
        }
        else
        {
            Debug.LogError("This Device doesn't suppot gyroscope");
        }
    }

    private void FixedUpdate()
    {
        if (gyroEnabled)
        {
            float tilt = GetTiltNotmalized();

            transform.position += new Vector3(tilt * speed * Time.fixedDeltaTime, 0, 0);
        }
    }

    float GetTiltNotmalized()
    {
        Quaternion currentRotation = gyro.attitude;

        var eulerAngles = currentRotation.eulerAngles.z - baseRotation.eulerAngles.z;

        float normalizedTilt = Mathf.Clamp(eulerAngles / 1f, -1f, 1f);

        return normalizedTilt;
    }

}
