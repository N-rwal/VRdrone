using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/*
this is the main input manager, he collects the usefull inputs and other scripts just use these values
should save processing power, and just feels right
*/


public class InputManager : MonoBehaviour
{
    // singleton instance, whatever this means
    public static InputManager Instance;

    public Vector2 leftJoystickValue;
    public Vector2 rightJoystickValue;  //this guy's not used for now, but might as well have him

    void Awake()
    {
        // this ensures only one instance exists, wouldn't want to have n++ of input managers
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // reads joystick input once per frame
        InputDevice leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        leftHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftJoystickValue);

        InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        rightHandDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightJoystickValue);
    }
}
