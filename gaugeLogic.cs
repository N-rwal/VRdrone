using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeDisplay : MonoBehaviour
{
    //these are for a potential laser turret
    /*
    public Slider leftJoystickXSlider;
    public Slider leftJoystickYSlider;
    */
    public Slider rightJoystickXSlider;
    public Slider rightJoystickYSlider;

    void Update()
    {
        // get joystick values from InputManager
        //Vector2 leftJoystick = InputManager.Instance.leftJoystickValue;
        Vector2 rightJoystick = InputManager.Instance.rightJoystickValue;

        // update the gauges
        /*
        leftJoystickXSlider.value = leftJoystick.x;
        leftJoystickYSlider.value = leftJoystick.y;
        */
        rightJoystickXSlider.value = rightJoystick.x;
        rightJoystickYSlider.value = rightJoystick.y;
    }
}
