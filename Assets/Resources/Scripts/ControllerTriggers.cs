using UnityEngine;
using System.Collections;

public class ControllerTriggers : MonoBehaviour {

	public bool RightTriggerDown { get; set; }
    public bool LeftTriggerDown { get; set; }
    public bool LeftTriggerUp { get; set; }

    private bool wasRightTriggerReleased = false;
    private bool wasLeftTriggerReleased = false;
    private bool wasLeftTriggerPressed = true;

    // Update is called once per frame
    void Update()
    {
        RightTriggerDown = false;
        if (Input.GetAxis("ShootAxis") == 1 && wasRightTriggerReleased)
        {
            RightTriggerDown = true;
            wasRightTriggerReleased = false;
        }
        if (Input.GetAxis("ShootAxis") == 0) wasRightTriggerReleased = true;


        LeftTriggerDown = false;
        LeftTriggerUp = false;
        if (Input.GetAxis("JumpAxis") == 1 && wasLeftTriggerReleased)
        {
            LeftTriggerDown = true;
            wasLeftTriggerReleased = false;
            wasLeftTriggerPressed = true;
        }
        if (Input.GetAxis("JumpAxis") == 0 && wasLeftTriggerPressed)
        {
            wasLeftTriggerReleased = true;
            LeftTriggerUp = true;
            wasLeftTriggerPressed = false;
        }
    }
}
