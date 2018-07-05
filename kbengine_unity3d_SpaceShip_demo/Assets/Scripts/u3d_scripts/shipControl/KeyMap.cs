using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMap{


    private KeyCode __moveForward;
    private KeyCode __moveBack;
    private KeyCode __rollLeft;
    private KeyCode __rollRight;
    private KeyCode __moveJump;
    private KeyCode __shoot;
    private KeyCode __accelerate;
    private KeyCode __shotting;

    public KeyCode MoveForward
    {
        get
        {
            return __moveForward;
        }

        set
        {
            __moveForward = value;
        }
    }

    public KeyCode MoveBack
    {
        get
        {
            return __moveBack;
        }

        set
        {
            __moveBack = value;
        }
    }

    public KeyCode RollLeft
    {
        get
        {
            return __rollLeft;
        }

        set
        {
            __rollLeft = value;
        }
    }

    public KeyCode RollRight
    {
        get
        {
            return __rollRight;
        }

        set
        {
            __rollRight = value;
        }
    }

    public KeyCode MoveJump
    {
        get
        {
            return __moveJump;
        }

        set
        {
            __moveJump = value;
        }
    }

    public KeyCode Shoot
    {
        get
        {
            return __shoot;
        }

        set
        {
            __shoot = value;
        }
    }

    public KeyCode Accelerate
    {
        get
        {
            return __accelerate;
        }

        set
        {
            __accelerate = value;
        }
    }

    public KeyCode Shotting
    {
        get
        {
            return __shotting;
        }

        set
        {
            __shotting = value;
        }
    }
}
