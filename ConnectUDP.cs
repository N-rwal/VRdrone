using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine;

public class VRDataSenderV2 : MonoBehaviour
{
    public static VRDataSenderV2 Instance { get; private set; }
    public byte[] receivedValues = new byte[4];

    public string sbcIP = "192.168.1.162";
    public int sbcPort = 5005;             // sending port
    public int headsetPort = 5006;         // receiving port

    private UdpClient udpClient;    // sending client
    private UdpClient udpListener;  // receiving listener
    private IPEndPoint remoteEndPoint, sbcEndpoint;

    void Awake()
    {
        if (Instance == null)   //again, a single singleton instance
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // setup UDP client for sending
        udpClient = new UdpClient();

        // setup UDP client for receiving
        udpListener = new UdpClient(headsetPort);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        udpListener.BeginReceive(ReceiveCallback, null);  //start receiving in another thread

        // this is the sbc target (for ex: the RPi)
        sbcEndpoint = new IPEndPoint(IPAddress.Parse(sbcIP), sbcPort);
    }

    void Update()
    {
        // sends the data
        Vector2 rightJoystick = InputManager.Instance.rightJoystickValue;
        string dataToSend = rightJoystick.x + "," + rightJoystick.y;
        byte[] data = Encoding.UTF8.GetBytes(dataToSend);
        udpClient.Send(data, data.Length, sbcEndpoint);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        byte[] data = udpListener.EndReceive(ar, ref remoteEndPoint);

        if (data.Length == 4)
        {
            // receive values, do a for loop later
            receivedValues[0] = data[0];
            receivedValues[1] = data[1];
            receivedValues[2] = data[2];
            receivedValues[3] = data[3];

            //Debug.Log($"Received Values: {receivedValues[0]}, {receivedValues[1]}, {receivedValues[2]}, {receivedValues[3]}");
        }

        // resume listening
        udpListener.BeginReceive(ReceiveCallback, null);
    }

    private void OnApplicationQuit()
    {
        // close ports on exit
        if (udpClient != null)
            udpClient.Close();

        if (udpListener != null)
            udpListener.Close();
    }
}
