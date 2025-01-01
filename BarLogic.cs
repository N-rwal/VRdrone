using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I replaced the previous floats with ints as I moved from meters to centimeters, this might break some things

public class SensorHUD : MonoBehaviour
{
    public RectTransform[] sensorBars; // applly a rect transform component in the Unity Inspector
    private int[] distances;         // array to store distances

    // minimum valid range (in cm)
    public int greenThreshold = 150;
    public int yellowThreshold = 80;
    public int redThreshold = 30;

    public float updateInterval = 0.5f;
    private float timer = 0f;

    void Start()
    {
        distances = new int[sensorBars.Length]; // initialize distances array
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= updateInterval)
        {
            byte[] values = VRDataSenderV2.Instance.receivedValues; //requests the values from the UDP singleton
        for (int i = 0; i < distances.Length; i++)  // application of values, random or concrete
        {
            //distances[i] = Random.Range(0.1f, 2f); // Simulating sensor data
            distances[i] = values[i]; // applying the received values
        }

        // Update each sensor pack
        for (int i = 0; i < sensorBars.Length; i++)
        {
            UpdateSensorBars(sensorBars[i], distances[i]);
        }
            timer = 0f;
        }  
    }

    void UpdateSensorBars(RectTransform sensorPack, int distance)
    {
        // find the individual bars, they should be children of the bar 'pack' in the hierarchy
        Transform greenBar = sensorPack.Find("Green");
        Transform yellowBar = sensorPack.Find("Yellow");
        Transform redBar = sensorPack.Find("Red");

        // logic to activate bar's conditions based on distance
        bool greenActive = distance > yellowThreshold;
        bool yellowActive = distance <= greenThreshold && distance > redThreshold;
        bool redActive = distance <= redThreshold;

        // activate the actual bars based on conditions
        greenBar.gameObject.SetActive(greenActive || yellowActive || redActive);
        yellowBar.gameObject.SetActive(yellowActive || redActive);
        redBar.gameObject.SetActive(redActive);
    }
}