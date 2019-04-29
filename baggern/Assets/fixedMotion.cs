using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using UnityEngine;

public class fixedMotion : MonoBehaviour { 

    public int port;

    private bool simXend;//define whether simulation X is ended;

    private byte[] bufferIn;
    private byte[] bufferOut;

    private double angSimxd;
    private float angSimxf;

    private double timeSimx;

    private int nFromSimX;
    private int nToSimX;
    private int recBytes;
    private int sendBytes;

    private Socket socket;
    private Socket client;

    //private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        bufferIn  = new byte[64];
        bufferOut = new byte[64];

        //position = new Vector3(0, 0, 0);//Eigangsposition in position eintragen;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//neuen Socket erstellen;
        IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);//Verbindung erstellen;

        socket.Bind(ipe);//verbunden mit bestimmte IP Address und Port;
        socket.Listen(1);//Maximum 1 Connection;

        client = socket.Accept();//client ist verbunden mit socket;
        recBytes  = client.Receive(bufferIn);//store the data from client to bufferIn;
        Array.Resize(ref bufferOut, recBytes);//resize bufferOut;
        Array.Copy(bufferIn,  0, bufferOut,  0, 12);
        Array.Copy(bufferIn, 16, bufferOut, 12,  4);//Warum sollen wir die Datenposition wechseln?
        Array.Copy(bufferIn, 12, bufferOut, 16,  4);

        nFromSimX = BitConverter.ToInt32(bufferIn, 12);//number of transmitter channel
        nToSimX   = BitConverter.ToInt32(bufferIn, 16);//number of receiver channel

        sendBytes = client.Send(bufferOut);//send data from bufferOut to client;
        simXend = false;

        SizeBuffers();//resize for data packet;
        TcpCom();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        while (timeSimx < Time.time && !simXend)
        {
            TcpCom();
        }

        angSimxf = Convert.ToSingle(angSimxd);

        gameObject.transform.Rotate(0, 0, angSimxf, Space.Self);//transform the object to specific posotion;       
    }

    void TcpCom()
    {
        recBytes = client.Receive(bufferIn);//store data from client to bufferIn;
        if (recBytes <= 0)//when there is no data in bufferIn then stop the function;
        {
            simXend = true;
            client.Close();
            socket.Close();
            return;
        }
        timeSimx = BitConverter.ToDouble(bufferIn,  4);

        angSimxd  = BitConverter.ToDouble(bufferIn, 24);//the position of masse in y axis;
        bufferOut = bufferIn;     
        client.Send(bufferIn);
    }

    void SizeBuffers()
    {
        Array.Resize(ref bufferIn,  24 + 16 * nFromSimX);
        //Array.Resize(ref bufferOut, 24 + 16 * nToSimX);
    }
}
