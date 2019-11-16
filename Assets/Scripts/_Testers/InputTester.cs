using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTester : MonoBehaviour
{
    private void Start()
    {
        TInputManager im = TInputManager.SharedInstance;

        im.OnMovementAxis += AxisTest;
        im.OnJumpDown += JumpDownTest;
        im.OnJumpUp += JumpUpTest;
        im.OnLeftClickDown += LeftClickDownTest;
        im.OnRightClickDown += RightClickDownTest;
        im.OnLeftClickUp += LeftClickUpTest;
        im.OnRightClickUp += RightClickUpTest;
    }

    private void OnEnable()
    {
        TInputManager im = TInputManager.SharedInstance;

        if (im)
        {
            im.OnMovementAxis += AxisTest;
            im.OnJumpDown += JumpDownTest;
            im.OnJumpUp += JumpUpTest;
            im.OnLeftClickDown += LeftClickDownTest;
            im.OnRightClickDown += RightClickDownTest;
            im.OnLeftClickUp += LeftClickUpTest;
            im.OnRightClickUp += RightClickUpTest;
        }
    }

    private void OnDisable()
    {
        TInputManager im = TInputManager.SharedInstance;

        im.OnMovementAxis -= AxisTest;
        im.OnJumpDown -= JumpDownTest;
        im.OnJumpUp -= JumpUpTest;
        im.OnLeftClickDown -= LeftClickDownTest;
        im.OnRightClickDown -= RightClickDownTest;
        im.OnLeftClickUp -= LeftClickUpTest;
        im.OnRightClickUp -= RightClickUpTest;
    }

    private void JumpDownTest()
    {
        Debug.Log("Jump DOWN");
    }

    private void JumpUpTest()
    {
        Debug.Log("Jump UP");
    }

    private void AxisTest(float inAxis)
    {
        Debug.Log("Axis value: " + inAxis);
    }

    private void LeftClickDownTest(TPointerData inData)
    {
        Debug.Log("Left click DOWN at " + inData);
    }

    private void RightClickDownTest(TPointerData inData)
    {
        Debug.Log("Right click DOWN at " + inData);
    }

    private void LeftClickUpTest(TPointerData inData)
    {
        Debug.Log("Left click UP at " + inData);
    }

    private void RightClickUpTest(TPointerData inData)
    {
        Debug.Log("Right click UP at " + inData);
    }
}
