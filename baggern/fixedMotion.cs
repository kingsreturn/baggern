using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using UnityEngine;

public class fixedMotion : MonoBehaviour { 

    //public GameObject gameObject;
    //public Collider collider;
    public int port;

    private bool simXend;//define whether simulation X is ended;
    //private bool coll;

    private byte[] bufferIn;
    private byte[] bufferOut;

    //private double posColl;
    private double angSimx;
    private float angSimx1;
    private double timeSimx;

    private int nFromSimX;
    private int nToSimX;
    private int recBytes;
    private int sendBytes;

    private Socket socket;
    private Socket client;

    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        //posColl = 0;
        //coll = false;

        bufferIn  = new byte[64];
        bufferOut = new byte[64];

        position = new Vector3(0, 0, 0);//Eigangsposition in position eintragen;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//neuen Socket erstellen;
        IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);//Verbindung erstellen;

        socket.Bind(ipe);//verbunden mit bestimmte IP Address und Port;
        socket.Listen(1);//Maximum 1 Connection;

        client = socket.Accept();//client ist verbunden mit socket;
        recBytes = client.Receive(bufferIn);//store the data from client to bufferIn;
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

        /*if (coll)//if the ball hit the ground,then store ? in poscoll;
        {
            //Vector3 closestPoint = coll.ClosestPointOnBounds(explosionPos);
            //float distance = Vector3.Distance(closestPoint, explosionPos); 
            posColl = (double)Vector3.Distance(collider.ClosestPointOnBounds(collider.transform.position),
                collider.ClosestPointOnBounds(transform.position));// 
        }
        else
        {
            posColl = 0;
        }*/

        //Debug.Log(posColl);

        /*
        position.y = (float) posSimx;
        position.x = (float) posColl*0;
        */
        angSimx1 = Convert.ToSingle(angSimx);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, angSimx1, 0));//transform the object to specific posotion;

        //Debug.Log(Time.time);
        

    }

    void TcpCom()
    {
        recBytes = client.Receive(bufferIn);//store data from client to bufferIn;
        if (recBytes <= 0)//when there is no data in bufferIn then stop the function;
        {
            simXend = true;
            return;
        }
        timeSimx = BitConverter.ToDouble(bufferIn,  4);

        //posColl= (double) collider.ClosestPoint();

        angSimx  = BitConverter.ToDouble(bufferIn, 24);//the position of masse in y axis;
        bufferOut = bufferIn;
        //Array.Copy(BitConverter.GetBytes(posColl),0,bufferOut,24,8);//give the real position from unity to bufferOut;
        client.Send(bufferIn);

    }

    void SizeBuffers()
    {
        Array.Resize(ref bufferIn,  24 + 16 * nFromSimX);
        //Array.Resize(ref bufferOut, 24 + 16 * nToSimX);
        //Array.Resize(ref bufferOut, 0);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        //posColl = 0;
        //Destroy(collision.other);
        coll = true;
        Debug.Log("Beginn Kollision\t" + Time.time);
    }

    private void OnCollisionExit(Collision collision)
    {
        //posColl = 0;
        coll = false;
        Debug.Log("Ende Kollision\t" + Time.time);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (coll)
        {
            Vector3 collPoint = collision.other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            posColl = (double)Vector3.Distance(position, collPoint);
            //Debug.Log(collision.other.gameObject.name + "\t" + collPoint + "\t" + posColl);
        }
        else
        {
            posColl = 0;
            //Debug.Log("EMPTY" + "\t" + "EMPTY" + "\t" + posColl);
        }


    }*/
    /*
    private void OnTriggerEnter(Collider other)//difference between oncollisionenter?
    {
        Debug.Log("Kollision start\t" + Time.time);
        coll = true;
        collider = other;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Kollision endet\t" + Time.time);
        coll = false;
        collider = null;
    }

    private void OnTriggerStay(Collider other)
    {
        //Vector3 collPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
    }
    */
}
