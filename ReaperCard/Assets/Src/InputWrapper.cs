using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enums for user-friendly input handling
public enum EKey
{
    Confirm = 0,
    Cancel,
    Menu,
    Jump
}

public enum EAxis
{
    X = 0,
    Y
}


public enum EInputDirection
{
    None,
    Up,
    Down,
    Left,
    Right
}

public struct KeyBind
{
    public KeyCode Keyboard;
    public KeyCode Gamepad;

    public KeyBind(KeyCode KeyboardKey, KeyCode GamePadButton)
    {
        Keyboard = KeyboardKey;
        Gamepad = GamePadButton;
    }
}

[System.Serializable]
public class InputWrapper
{
    string AxisNameX { get; set; }
    string AxisNameY { get; set; }

    private List<KeyBind> Bindings;

    public InputWrapper()
    {

        AxisNameX = "MoveX";
        AxisNameY = "MoveY";



        Bindings = new List<KeyBind>();

        Bindings.Add(new KeyBind(KeyCode.E, KeyCode.Joystick1Button1)); //Confirm
        Bindings.Add(new KeyBind(KeyCode.Escape, KeyCode.Joystick1Button0)); //Cancel
        Bindings.Add(new KeyBind(KeyCode.Tab, KeyCode.Joystick1Button3)); //Menu
        Bindings.Add(new KeyBind(KeyCode.Space, KeyCode.Joystick1Button2)); //Jump

    }

    // Axis values

    // Get WASD or Left Stick Axis
    public Vector2 GetXY()
    {
        return new Vector2(Input.GetAxis(AxisNameX), Input.GetAxis(AxisNameY));
    }

    // Get the WASD or Left Stick value for a single axis
    public float GetAxis(EAxis Axis)
    {
        float a = (Axis == EAxis.X) ? Input.GetAxis(AxisNameX) : Input.GetAxis(AxisNameY);
        return a;
    }

    // Returns a cardinal direction (useful for menus)
    public EInputDirection GetDirection(EAxis Axis)
    {
        EInputDirection Direction = EInputDirection.None;
        float Dir = GetAxis(Axis);

        if (Axis == EAxis.X)
        {
            if (Direction < 0)
            {
                Direction = EInputDirection.Left;
            }
            else if (Direction > 0)
            {
                Direction = EInputDirection.Right;
            }
        }
        else
        {
            if (Direction < 0)
            {
                Direction = EInputDirection.Up;
            }
            else if (Direction > 0)
            {
                Direction = EInputDirection.Down;
            }
        }

        return Direction;
    }

    // Button values

    // True is pressed this frame
    public bool IsDown(EKey Key)
    {
        KeyBind Bind = Bindings[(int)Key];

        return Input.GetKeyDown(Bind.Keyboard) || Input.GetKeyDown(Bind.Gamepad);
    }

    // True if released this frame
    public bool IsUp(EKey Key)
    {
        KeyBind Bind = Bindings[(int)Key];

        return Input.GetKeyUp(Bind.Keyboard) || Input.GetKeyUp(Bind.Gamepad);
    }

    // True if button is currently pressed
    public bool IsHeld(EKey Key)
    {
        KeyBind Bind = Bindings[(int)Key];

        return Input.GetKey(Bind.Keyboard) || Input.GetKey(Bind.Gamepad);
    }
}

