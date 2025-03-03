using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroController : PlayerSubject
{
    [Header("Gyroscope attributes")]
    public float speedMin = 1f;  // Velocidad mínima al mover el personaje
    public float speedMax = 5f;  // Velocidad máxima
    [Tooltip("Higher Values make it less sensitive")]
    public float tiltSensitivity = 30f;
    [Tooltip("Rotation angles where the controller will not send any value")]
    public float deadZone = 5f;
    private Rigidbody rb;
    

    private bool gyroEnabled;
    private Gyroscope gyro;
    private Quaternion baseRotation;
    private float currentSpeed = 0f; // Velocidad actual suavizada

    private void Start()
    {
        gyroEnabled = SystemInfo.supportsGyroscope;
        rb = GetComponent<Rigidbody>();

        if (gyroEnabled)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            baseRotation = gyro.attitude;
        }
        else
        {
            Debug.LogError("This device doesn't support gyroscope");
        }
    }

    private void FixedUpdate()
    {
        if (gyroEnabled)
        {
            float tilt = GetTiltNormalized();
            tilt *= -1; // Invertir si es necesario

            // Determinar la velocidad dependiendo de la inclinación
            float targetSpeed = Mathf.Lerp(speedMin, speedMax, Mathf.Abs(tilt));

            // Suavizar la transición de la velocidad
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * 5f);

            // Aplicar movimiento
            transform.position += new Vector3(tilt * currentSpeed * Time.fixedDeltaTime, 0, 0);
        }
    }

    float GetTiltNormalized()
    {
        Quaternion currentRotation = gyro.attitude;

        float eulerAnglesZ = currentRotation.eulerAngles.z - baseRotation.eulerAngles.z;

        if (eulerAnglesZ > 180f) eulerAnglesZ -= 360f;
        if (eulerAnglesZ < -180f) eulerAnglesZ += 360f;

        // Aplicamos la "dead zone"
        if (Mathf.Abs(eulerAnglesZ) < deadZone)
        {
            return 0f;
        }

        // Normalizamos la inclinación
        float normalizedTilt = eulerAnglesZ / tiltSensitivity;

        return Mathf.Clamp(normalizedTilt, -1f, 1f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("carrot"))
        {
            NotifyObservers(PlayerEnum.CollectCarrots);
        }
        if (other.CompareTag("hazard"))
        {
            NotifyObservers(PlayerEnum.Die);
        }
    }
   

    void Update()
    {
        Move();

    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ) * 20;
        Vector3 newVelocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        rb.velocity = newVelocity;
    }
}
