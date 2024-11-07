
//built off of this reference: https://youtu.be/fESM_UIg1rA?si=h8z5sQxSnq3bZS35 
//referenced Unity Rigidbody.AddForce documentation 


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControllerAcceleration : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 15f; //how fast the ball rolls
    private Rigidbody rigid; //the ball's rigid body

    //IMU values, initialized to random variables
    public int i = 3;
    public int j = 6;
    public int k = 9;

    //rotation offsets to take into account that the starting rotation of the IMU probably won't be (0, 0, 0)
    public int iOffset = 0;
    public int jOffset = 0;

    //forward/backward and right/left acceleration of the ball
    public float iAcceleration = 0.0f;
    public float jAcceleration = 0.0f;

    int loop = 0; //counts the number of times Update() loops

    public DataIO data; //drag the gameobject (DataManager) into the Data field in the inspector panel

    private void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>(); //the ball's rigid body        
    }

    // Update is called once per frame
    private void Update()
    {
        
        if (loop == 3) //the first loop through Update() is too early to get initiate rotation data from the IMU, so wait until it loops a few times
        {
            iOffset = data.iFromIMU;
            jOffset = data.jFromIMU;
        }

        //update IMU values, range from -100 to 100
        //the IMU will likely not start at (0, 0, 0) rotation, so offset the incoming values based on the initial orientation of the IMU
        i = data.iFromIMU - iOffset;
        j = data.jFromIMU - jOffset;
        //k = data.kFromIMU; //k isn't used

        iAcceleration = speed * ((i * 1.0f) / 30.0f); //for left/right movement; I can only bend my body about 30 degrees left/right so /30
        jAcceleration = speed * ((j * 1.0f) / 30.0f); //for forwards/backwards movement

        if (i < -1) //right
        {
            rigid.AddForce(-iAcceleration, 0, 0, ForceMode.Acceleration);
        }
        else if (i > 1) //left
        {
            rigid.AddForce(-iAcceleration, 0, 0, ForceMode.Acceleration);
        }

        if (j < -1) //forward
        {
            rigid.AddForce(0,0, -jAcceleration, ForceMode.Acceleration);
        }
        else if (j > 1) //backwards, should probably less restrictive threshold since bending backwards is harder
        {
            rigid.AddForce(0, 0, -jAcceleration, ForceMode.Acceleration);
        }

        loop++;
    }
}

