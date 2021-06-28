using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {

    public static Dictionary<MarioFSM.MovementState, InputSet> MarioStateInputs =
            new Dictionary<MarioFSM.MovementState, InputSet> {
            { MarioFSM.MovementState.Walking,
            new InputSet(pAButtonAction:InputActions.WalkingJumpDecider, pBButtonAction:InputActions.WalkingBDecider,
                                pYButtonAction:InputActions.EnterCloseup, pXButtonAction:InputActions.ChangeFluddMode, pCStickAction:InputActions.RotateCamera, pJoystickAction:InputActions.Move,
                                pZButtonAction:InputActions.ShowMap, pLButtonAction:InputActions.LockXAndCenter, pRButtonAction:InputActions.SprayWater)},
            { MarioFSM.MovementState.HoldingLedge,
            new InputSet(pAButtonAction:InputActions.ClimbLedge, pBButtonAction:InputActions.ReleaseLedge,
                                pYButtonAction:InputActions.EnterCloseup, pXButtonAction:InputActions.ChangeFluddMode, pCStickAction:InputActions.RotateCamera, pJoystickAction:InputActions.MoveAlongLedge,
                                pZButtonAction:InputActions.ShowMap, pLButtonAction:InputActions.CenterCamera, pRButtonAction:InputActions.SprayWaterFromLedge)},
            { MarioFSM.MovementState.Midair,
            new InputSet(pAButtonAction:null, pBButtonAction:InputActions.WalkingBDecider,
                                pYButtonAction:InputActions.EnterCloseup, pXButtonAction:InputActions.ChangeFluddMode, pCStickAction:InputActions.RotateCamera, pJoystickAction:InputActions.Move,
                                pZButtonAction:InputActions.ShowMap, pLButtonAction:InputActions.GroundPound, pRButtonAction:InputActions.SprayWater)},
            { MarioFSM.MovementState.Balancing,
            new InputSet(pAButtonAction:InputActions.WalkingJumpDecider, pBButtonAction:InputActions.FromBalanceToHang,
                                pYButtonAction:InputActions.EnterCloseup, pXButtonAction:InputActions.ChangeFluddMode, pCStickAction:InputActions.RotateCamera, pJoystickAction:InputActions.Move,
                                pZButtonAction:InputActions.ShowMap, pLButtonAction:InputActions.CenterCamera, pRButtonAction:InputActions.SprayWater)},
            { MarioFSM.MovementState.WallSliding,
            new InputSet(pAButtonAction:InputActions.JumpOffWall, pBButtonAction:InputActions.HeadbuttWall,
                                pYButtonAction:InputActions.EnterCloseup, pXButtonAction:InputActions.ChangeFluddMode, pCStickAction:InputActions.RotateCamera, pJoystickAction:null,
                                pZButtonAction:InputActions.ShowMap, pLButtonAction:InputActions.HeadbuttWall, pRButtonAction:null)},
            { MarioFSM.MovementState.GroundSliding,
            new InputSet(pAButtonAction:InputActions.SlideJump, pBButtonAction:InputActions.SlideHop,
                                pYButtonAction:InputActions.EnterCloseup, pXButtonAction:InputActions.ChangeFluddMode, pCStickAction:InputActions.RotateCamera, pJoystickAction:InputActions.Move,
                                pZButtonAction:InputActions.ShowMap, pLButtonAction:InputActions.CenterCamera, pRButtonAction:null)},
            { MarioFSM.MovementState.PoleClimbing,
            new InputSet(pAButtonAction:InputActions.JumpOffPole, pBButtonAction:InputActions.ReleasePole,
                                pYButtonAction:null, pXButtonAction:InputActions.ChangeFluddMode, pCStickAction:InputActions.RotateCamera, pJoystickAction:InputActions.MoveAroundPole,
                                pZButtonAction:InputActions.ShowMap, pLButtonAction:InputActions.CenterCamera, pRButtonAction:null)},
            { MarioFSM.MovementState.Swimming,
            new InputSet(pAButtonAction:InputActions.SwimUpOrForward, pBButtonAction:InputActions.SwimDown,
                                pYButtonAction:InputActions.EnterCloseup, pXButtonAction:InputActions.ChangeFluddMode, pCStickAction:InputActions.RotateCamera, pJoystickAction:InputActions.Move,
                                pZButtonAction:InputActions.ShowMap, pLButtonAction:InputActions.CenterCamera, pRButtonAction:InputActions.SprayWater)}
            };

    public static KeyValuePair<MarioFSM.MovementState, InputSet> changeInputs(MarioFSM.MovementState newState)
    {
        KeyValuePair<MarioFSM.MovementState, InputSet> newInputs;
        newInputs = new KeyValuePair<MarioFSM.MovementState, InputSet>(newState, MarioStateInputs[newState]);
        if (MarioStateInputs[newState] == null)
        {
            Debug.LogError("Error: state inputs not found for " + newState);
        }
        return newInputs;
    }

    static KeyValuePair<MarioFSM.MovementState, InputSet> currentMarioInputs;
    public static void handleInput(PlayerController Mario)
    {
        //MarioFSM.MovementState startingState = MarioFSM.MarioMovementState;
        currentMarioInputs = MarioFSM.currentMarioInputs;
        if (Input.GetKey(KeyCode.Mouse0)) //left mouse = spraying
        {
            currentMarioInputs.Value.RButtonAction.ButtonAction(Mario);
        }
        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse2)) //right mouse = camera control, middle mouse = camera zoom
        {
            currentMarioInputs.Value.CStickAction.ButtonAction(Mario);
        }
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(ButtonMapper.AButton))
            {
                currentMarioInputs.Value.AButtonAction.ButtonAction(Mario);
            }
            if (Input.GetKeyDown(ButtonMapper.BButton)) // need to check startingState == MarioFSM.MarioMovementState ?
            {
                currentMarioInputs.Value.BButtonAction.ButtonAction(Mario);
            }
            if (Input.GetKeyDown(ButtonMapper.XButton))
            {
                currentMarioInputs.Value.XButtonAction.ButtonAction(Mario);
            }
            if (Input.GetKeyDown(ButtonMapper.YButton))
            {
                currentMarioInputs.Value.YButtonAction.ButtonAction(Mario);
            }
            if (Input.GetKeyDown(ButtonMapper.ZButton))
            {
                currentMarioInputs.Value.ZButtonAction.ButtonAction(Mario);
            }
            if (Input.GetKeyDown(ButtonMapper.LButton))
            {
                currentMarioInputs.Value.LButtonAction.ButtonAction(Mario);
            }

            bool moving = false;
            for (int n = 0; n < ButtonMapper.Joystick.Count; n++)
            {
                if (Input.GetKey(ButtonMapper.Joystick[n]))
                {
                    moving = true;
                }
            }
            if (moving)
            {
                currentMarioInputs.Value.JoystickAction.ButtonAction(Mario);
            }
        }

        
    }


}

/// <summary>
/// Determines which keyboard key represents each button.
/// </summary>
public static class ButtonMapper
{
    public static KeyCode AButton = KeyCode.Space;
    public static KeyCode BButton = KeyCode.Q;
    public static KeyCode XButton = KeyCode.E;
    public static KeyCode YButton = KeyCode.R;
    public static KeyCode ZButton = KeyCode.Keypad1;
    public static KeyCode LButton = KeyCode.LeftShift;
    public static KeyCode RButton = KeyCode.Mouse0;
    public static List<KeyCode> Joystick = new List<KeyCode> { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow};
    public static KeyCode Cstick = KeyCode.Mouse2;
}

/// <summary>
/// Holds a set of delegates for all buttons.
/// </summary>
public class InputSet
{
    public ButtonDelegate AButtonAction;
    public ButtonDelegate BButtonAction;
    public ButtonDelegate YButtonAction;
    public ButtonDelegate XButtonAction;
    public ButtonDelegate CStickAction;
    public ButtonDelegate JoystickAction;
    public ButtonDelegate ZButtonAction;
    public ButtonDelegate LButtonAction;
    public ButtonDelegate RButtonAction;

    public InputSet(ButtonDelegate.Button pAButtonAction, ButtonDelegate.Button pBButtonAction, ButtonDelegate.Button pYButtonAction, ButtonDelegate.Button pXButtonAction,
        ButtonDelegate.Button pCStickAction, ButtonDelegate.Button pJoystickAction,
        ButtonDelegate.Button pZButtonAction, ButtonDelegate.Button pLButtonAction, ButtonDelegate.Button pRButtonAction)
    {
        AButtonAction = new ButtonDelegate(pAButtonAction);
        BButtonAction = new ButtonDelegate(pBButtonAction);
        YButtonAction = new ButtonDelegate(pYButtonAction);
        XButtonAction = new ButtonDelegate(pXButtonAction);
        CStickAction = new ButtonDelegate(pCStickAction);
        JoystickAction = new ButtonDelegate(pJoystickAction);
        ZButtonAction = new ButtonDelegate(pZButtonAction);
        LButtonAction = new ButtonDelegate(pLButtonAction);
        RButtonAction = new ButtonDelegate(pRButtonAction);
    }
}

/// <summary>
/// Holds a method delegate for a button.
/// </summary>
public class ButtonDelegate
{
    public delegate void Button(PlayerController Mario);
    public Button ButtonAction;

    public ButtonDelegate(Button pButtonAction)
    {
        if (pButtonAction != null)
        {
            ButtonAction = pButtonAction;
        }
        else
        {
            ButtonAction = InputActions.EmptyAction;
        }
    }
}
