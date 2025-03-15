using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTurbinDrone : MonoBehaviour
{
    [SerializeField]
    private Transform turbine1;
    [SerializeField]
    private Transform turbine2;
    [SerializeField]
    private Transform turbine3;
    [SerializeField]
    private Transform turbine4;

    [SerializeField]
    private float rotationSpeed = 500f;

    private bool isRotating = false;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure all turbines are assigned
        if (turbine1 == null || turbine2 == null || turbine3 == null || turbine4 == null)
        {
            Debug.LogError("Please assign all turbines in the inspector!");
        }
    }

    public void PlayTurbineAnimation()
    {
        isRotating = true;
    }

    public void StopTurbineAnimation()
    {
        isRotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            RotateTurbines();
        }
    }

    private void RotateTurbines()
    {
        turbine1.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        turbine2.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        turbine3.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        turbine4.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
    }
}
