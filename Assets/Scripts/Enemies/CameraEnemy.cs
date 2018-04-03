using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnemy : MonoBehaviour {

    [SerializeField] private HingeJoint2D joint;
    [SerializeField] private float waitingTime = 2;     //Time that camera awaits until change direction

    private bool changingDirection = false;

    // Update is called once per frame
    void Update () {

        if ((joint.limitState != JointLimitState2D.Inactive) && (!changingDirection) )
        {
            changingDirection = true;
            Invoke("changeMotorDirection", waitingTime);
        }
    }
    
    private void changeMotorDirection()
    {
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = joint.motor.motorSpeed * -1;        
        joint.motor = motor;

        changingDirection = false;
    }

    private void stopMotor()
    {
        JointMotor2D motor = joint.motor;
        motor.motorSpeed = 0;

        joint.motor = motor;
    }
}
