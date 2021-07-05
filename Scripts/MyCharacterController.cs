using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    private Transform cam;
    Vector2 mousePos;
    [SerializeField] float mouseSensor = 3.0f;
    [SerializeField] float lookUpLimit = 90.0f;
    [SerializeField] float walkSpeed = 5.0f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float moveSmoothTime = 0.3f;
    [SerializeField] float cameraSmoothTime = 0.1f;
    private CharacterController character;
    private float camRot = 0;
    private float realCamRot = 0;
    private float camSpeed = 0;
    Vector2 targetDir;
    Vector2 movDir;
    Vector2 curVel;
    // Start is called before the first frame update
    void Start()
    {
        movDir = Vector3.zero;
        curVel = Vector2.zero;
        cam = transform.GetChild(0);
        character = GetComponent<CharacterController>();
        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked; // lock mouse to center
            Cursor.visible = false; // dont show cursor in screen 
        }
    }

    // Update is called once per frame
    void Update()
    {
        mouseMovment();
        playerMovment();
    }

    void mouseMovment() //// look of player
    {
        mousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); /// store the postion of mouse in mousePos Vector Variable
        camRot -= mousePos.y * mouseSensor; // camRot initilay is 0 subtract from it how y is moved in frame subtraction because
        //we will rotate around the x axis of camera look donw => mousePos.y < 0 but and if yoy want to look down  that means x rot of camera should be > 0 thats why -= not +=
        camRot = Mathf.Clamp(camRot, -lookUpLimit, lookUpLimit); // limit the variable camRot between lookUpLimit 
        realCamRot = Mathf.SmoothDamp(realCamRot, camRot, ref camSpeed, cameraSmoothTime);
        cam.localEulerAngles = Vector3.right * realCamRot; //// see gambl look is way of rotation
        transform.Rotate(Vector3.up * mousePos.x * mouseSensor); /// rotate player and camrera will rotate with since it is child
    }

    void playerMovment() /// movment of the player
    {
        targetDir = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();
        movDir = Vector2.SmoothDamp(movDir, targetDir, ref curVel,  moveSmoothTime);
        Vector3 vel = (transform.forward * movDir.y + transform.right * movDir.x) * walkSpeed;
        character.Move(vel * Time.deltaTime);
    }
}
