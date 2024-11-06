//used and built off of this reference: https://youtu.be/fESM_UIg1rA?si=h8z5sQxSnq3bZS35 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 5f; //how fast the ball rolls
    private Rigidbody rigid;

    public int i = 3;
    public int j = 6;
    public int k = 9;

    public DataIO data; //drag the gameobject (DataManager) into the Data field in the inspector panel

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>(); //the ball's rigid body

    }

    // Update is called once per frame
    private void Update()
    {
        //update IMU values
        i = data.iFromIMU;
        j = data.jFromIMU;
        k = data.kFromIMU;

        if (i < -40) //right
        {
            rigid.AddForce(Vector3.right * speed);
        }
        else if (i > 40) //left
        {
            rigid.AddForce(Vector3.right * -speed);
        }

        if (j < -40) //forward
        {
            rigid.AddForce(Vector3.forward * speed);
        }
        else if (j > 40) //backwards
        {
            rigid.AddForce(Vector3.forward * -speed);
        } 

        /*
         * Arrow key inputs
         * 
        if (Input.GetAxis("Horizontal") > 0)
        {
            rigid.AddForce(Vector3.right * speed);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rigid.AddForce(Vector3.right * -speed);
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            rigid.AddForce(Vector3.forward * speed);
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            rigid.AddForce(Vector3.forward * -speed);
        }
        */

    }
}
