using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float sprintSpeed = 30f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y ; 
            //smoothing angles inside of unity so that turns smoothly
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f); 

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            

            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(moveDir * sprintSpeed * Time.deltaTime + new Vector3(0, -10, 0) * Time.deltaTime);
                Debug.Log("Srinting: " + sprintSpeed + "\n" +moveDir); //sprintSpeed is printed as 10 and not 30f
            }
            else
            {
                controller.Move(moveDir * speed * Time.deltaTime + new Vector3(0, -1, 0) * Time.deltaTime);
            }
        } 
        
    }
}
