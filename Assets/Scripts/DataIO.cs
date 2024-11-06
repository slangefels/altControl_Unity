using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class DataIO : MonoBehaviour //Shannon's DataIO Unity script
{
    public GameObject objectToTrack; // Object to track and send position data

    //my variables
    public int iFromIMU = 1;
    public int jFromIMU = 2;
    public int kFromIMU = 3;


    UdpClient udpClient;
    IPEndPoint remoteEndPoint;

    void Start()
    {
        // Set up UDP to receive data from Arduino
        udpClient = new UdpClient(4211);
        remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        Debug.Log("BeginRecieve done");

        // Set up UDP to send data to Arduino (this IP address should be the microcontroller's IP address)
        remoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.187"), 4211); // Adjust IP and port if necessary

        Debug.Log("Start() finished");
    }

    void Update()
    {
        
        // Get the position of the tracked object
        //Vector3 position = new Vector3(5, 10, 15);
        Vector3 position = objectToTrack.transform.position; 

        // Format the position data as a string
        string positionData = $"X:{position.x},Y:{position.y},Z:{position.z}";

        // Convert the string to bytes
        byte[] data = Encoding.ASCII.GetBytes(positionData);

        // Send the UDP packet to the Arduino
        udpClient.Send(data, data.Length, remoteEndPoint);

        Debug.Log("Sent to Microcontroller: " + positionData);

        Debug.Log("End of Update() loop");

    }

    void ReceiveCallback(IAsyncResult ar)
    {
        // Receive the UDP packet from Arduino
        byte[] receivedBytes = udpClient.EndReceive(ar, ref remoteEndPoint);
        string receivedText = Encoding.ASCII.GetString(receivedBytes);
        Debug.Log("Received: " + receivedText);

        // Parse the received data (i, j, k values from the IMU)
        string[] data = receivedText.Split(',');
        if (data.Length == 3)
        {
            //i, j, k values are stored in their respective variables as ints
            int.TryParse(data[0].Split(':')[1].Trim(), out iFromIMU);
            int.TryParse(data[1].Split(':')[1].Trim(), out jFromIMU);
            int.TryParse(data[2].Split(':')[1].Trim(), out kFromIMU);
        }

        //print debug statement to see the values coming over from the microcontroller in the console window
        Debug.Log("iFromIMU = " + iFromIMU + ", jFromIMU = " + jFromIMU + ", kFromIMU = " + kFromIMU);

        // Continue receiving data
        udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);

    }

    void OnApplicationQuit()
    {
        // Close the UDP client on quit
        udpClient.Close();
    }
}