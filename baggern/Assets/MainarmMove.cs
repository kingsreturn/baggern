using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainarmMove : MonoBehaviour {
    public float speed1;
    public float rotSpeed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W)) {  //up
            MoveW();
        }
        if (Input.GetKey(KeyCode.S)) {   //down
            MoveD();
        }
      



     
	}
    void MoveW() {
      

        transform.Rotate(Vector3.forward*rotSpeed * Time.deltaTime); //围绕某轴旋转
    }
    void MoveD() {
        

        transform.Rotate(Vector3.back* rotSpeed * Time.deltaTime);
    }
   
}
