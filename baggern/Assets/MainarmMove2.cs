using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainarmMove2 : MonoBehaviour {
    public float rotSpeed;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.I)) {    //forward
            MoveI();
        }
        if (Input.GetKey(KeyCode.J)) {   //back
            MoveJ();
        }
    }
    void MoveI() {
        transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime); //围绕某轴旋转    }
    }
    void MoveJ() {
        transform.Rotate(Vector3.back * rotSpeed * Time.deltaTime);
    }
}
