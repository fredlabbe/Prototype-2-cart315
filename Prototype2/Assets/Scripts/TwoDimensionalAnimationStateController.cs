using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionalAnimationStateController : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f; 
    //max walk velocity
    public float maxWalkVel = 0.5f;
    //max run velocity
    public float maxRunVel = 2.0f;

    //increase performance 
    int VelocityZHash;
    int VelocityXHash;

    // Start is called before the first frame update
    void Start()
    {
        //search gameObject this script is attached to and get the animator component 
        animator = GetComponent<Animator>();

        //increase performance 
        VelocityZHash = Animator.StringToHash("VelocityZ");
        VelocityXHash = Animator.StringToHash("VelocityX");
    } 

    void changeVelocity(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVel)
    {
        //if forwardPressed,increase vel in z
        if (forwardPressed && velocityZ < currentMaxVel)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        //if backwardPressed,increase vel in z
        if (backwardPressed && velocityZ < currentMaxVel)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        //increase vel in left direction
        if (leftPressed && velocityX > -currentMaxVel)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        //increase vel in right direction
        if (rightPressed && velocityX < currentMaxVel)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //decrease velocityZ
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        //increase velocityX if left not pressed and velocityX < 0
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }
        //decrease velocityX if right not pressed and velocityX > 0
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
    }

    void lockOrResetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVel)
    {
        //reset velocityZ
        if (!forwardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }

        //reset velocityX
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }
        //lock forward
        if (forwardPressed && runPressed && velocityZ > currentMaxVel)
        {
            velocityZ = currentMaxVel;
        }
        //decelerate to max walk velocity
        else if (forwardPressed && velocityZ > currentMaxVel)
        {
            velocityZ -= Time.deltaTime * deceleration;
            //round to currentMaxVel if within offset
            if (velocityZ > currentMaxVel && velocityZ < (currentMaxVel + 0.05f))
            {
                velocityZ = currentMaxVel;
            }
        }
        //round to currentMaxVel if within offset
        else if (forwardPressed && velocityZ < currentMaxVel && velocityZ > (currentMaxVel - 0.05f))
        {

            velocityZ = currentMaxVel;
        }

        //lock left
        if (leftPressed && runPressed && velocityX < -currentMaxVel)
        {
            velocityX = -currentMaxVel;
        }
        //decelerate to max walk velocity
        else if (leftPressed && velocityX < -currentMaxVel)
        {
            velocityX += Time.deltaTime * deceleration;
            //round to currentMaxVel if within offset
            if (velocityX < -currentMaxVel && velocityX > (-currentMaxVel - 0.05f))
            {
                velocityX = -currentMaxVel;
            }
        }
        //round to currentMaxVel if within offset
        else if (leftPressed && velocityX > -currentMaxVel && velocityX < (-currentMaxVel + 0.05f))
        {

            velocityX = -currentMaxVel;
        }

        //lock right
        if (rightPressed && runPressed && velocityX > currentMaxVel)
        {
            velocityX = currentMaxVel;
        }
        //decelerate to max walk velocity
        else if (rightPressed && velocityX > currentMaxVel)
        {
            velocityX -= Time.deltaTime * deceleration;
            //round to currentMaxVel if within offset
            if (velocityX > currentMaxVel && velocityX < (currentMaxVel + 0.05f))
            {
                velocityX = currentMaxVel;
            }
        }
        //round to currentMaxVel if within offset
        else if (rightPressed && velocityX < currentMaxVel && velocityX > (currentMaxVel - 0.05f))
        {

            velocityX = currentMaxVel;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get key inputs 
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwardPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        //set current max Velocityif runPressed true: 1st param else 2nd param
        float currentMaxVel = runPressed ? maxRunVel : maxWalkVel;

        //handle Vel 
        changeVelocity(forwardPressed, backwardPressed, leftPressed, rightPressed, runPressed, currentMaxVel);
        lockOrResetVelocity(forwardPressed, leftPressed, rightPressed, runPressed, currentMaxVel);


        //set params to local variable values
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
    }
}
