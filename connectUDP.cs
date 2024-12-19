using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

/*
    Orange PI ip: 192.168.1.62
    Raspberry PI ip on my network: 192.168.1.167
    Raspberry PI ip on iut network: 192.168.0.103
*/

public class VRDataSenderV2 : MonoBehaviour
{
    public string sbcIP = "192.168.0.105";
    public int sbcPort = 5005;             // sending port
    public int headsetPort = 5006;         // receiving port

    private UdpClient udpClient;    // sending client
    private UdpClient udpListener;  // receiving listener
    private IPEndPoint remoteEndpoint;

    void Start()
    {
        // setup UDP client for sending
        udpClient = new UdpClient();

        // setup UDP client for receiving
        udpListener = new UdpClient(headsetPort);
        udpListener.Client.Blocking = false;  // non-blocking mode, any sender

        // this is the sbc target (for ex: the RPi)
        remoteEndpoint = new IPEndPoint(IPAddress.Parse(sbcIP), sbcPort);
    }

    void Update()
    {
        // sends the data
        Vector2 rightJoystick = InputManager.Instance.rightJoystickValue;
        string dataToSend = rightJoystick.x + "," + rightJoystick.y;    //should look like (-1,-1) both negative here although values from -1 to 1
        byte[] data = Encoding.UTF8.GetBytes(dataToSend);
        udpClient.Send(data, data.Length, remoteEndpoint);

        // receive the data
        try
        {
            if (udpListener.Available > 0)
            {
                byte[] receivedData = udpListener.Receive(ref remoteEndpoint);
                string receivedMessage = Encoding.UTF8.GetString(receivedData);
                //Debug.Log("I got this: " + receivedMessage); //debug, I use the android studio logcat feature
            }
        }
        catch (SocketException) 
        {
            // ignore non-blocking exceptions
        }
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
