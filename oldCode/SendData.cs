using System.Collections;
using System.Collections.Generic;

using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.XR;

public class VRDataSender : MonoBehaviour
{
    public string sbcIP = "192.168.1.62";  // SBC's IP address
    public int sbcPort = 5005;             // Port number for UDP communication

    private UdpClient udpClient;

    void Start()
    {
        // Initialize the UDP client
        udpClient = new UdpClient();
    }

    void Update()
    {
        // Get the headset's rotation using Unity's XR framework
        Quaternion headsetRotation = InputTracking.GetLocalRotation(XRNode.Head);
        Vector3 eulerRotation = headsetRotation.eulerAngles;
/*
        float yaw = eulerRotation.y;
        float pitch = eulerRotation.x;
        float roll = eulerRotation.z;
*/
        // Get controller inputs (example for a joystick)
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Create a string with the data to send
        string dataToSend = joystickInput.x + "," + joystickInput.y;
        //string dataToSend = yaw + "," + pitch + "," + roll + "," + joystickInput.x + "," + joystickInput.y;

        // Convert the string to bytes and send it to the SBC
        byte[] data = Encoding.UTF8.GetBytes(dataToSend);
        udpClient.Send(data, data.Length, sbcIP, sbcPort);
    }

    private void OnApplicationQuit()
    {
        // Close the UDP client when the application quits
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
