using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject Store;
    [SerializeField] private Camera Camera;

    [SerializeField] private float transitionDuration ;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private float transitionTimer = 0.0f;
    private bool isTransitioning = false;
    private bool isMainMenu = true;

    void Start()
    {
        initialPosition = Camera.transform.position;
        initialRotation = Camera.transform.rotation;
    }

    public void OpenStore()
    {
        if (isTransitioning) return;

        isMainMenu = false;
        MainMenu.SetActive(false);



        targetPosition = new Vector3(-8.344361f, 5.722942f, 10.39388f);
        targetRotation = Quaternion.Euler(20, 184, 0);

        transitionTimer = 0.0f;
        isTransitioning = true;
    }

    public void ReturnToMainMenu()
    {
        if (isTransitioning) return;

        isMainMenu = true;
        Store.SetActive(false);



        targetPosition = initialPosition; 
        targetRotation = initialRotation;

        transitionTimer = 0.0f;
        isTransitioning = true;
        print(isMainMenu);
    }

    void Update()
    {
        if (isTransitioning)
        {

            transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(transitionTimer / transitionDuration);

            if (isMainMenu)
            {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, initialPosition, t); 
                Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, initialRotation, t);
                if (Vector3.Distance(Camera.transform.position, initialPosition) < 0.01f &&
       Quaternion.Angle(Camera.transform.rotation, initialRotation) < 0.1f)
                {
                    t = 1;
                    MainMenu.SetActive(true);
                }
            }
            else
            {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, targetPosition, t);
                Camera.transform.rotation = Quaternion.Slerp(Camera.transform.rotation, targetRotation, t);
                if (Vector3.Distance(Camera.transform.position, targetPosition) < 0.01f &&
        Quaternion.Angle(Camera.transform.rotation, targetRotation) < 0.1f)
                {
                    t = 1;
                    Store.SetActive(true);
                }
            }

            if (t >= 1f)
            {
                isTransitioning = false;
            }
        }
    }
}