using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputActions {
    
    public static void EmptyAction(PlayerController Mario)
    {
        // does nothing - is called when button is disabled for current state
    }

    #region multibutton
    public static void ExitCloseup(PlayerController Mario)
    {
        Debug.Log("input detected: exit closeup");
        //TODO
    }

    public static void HeadbuttWall(PlayerController Mario)
    {
        Debug.Log("input detected: headbutt wall");
        //TODO
    }

    public static void ClimbLedge(PlayerController Mario)
    {
        Debug.Log("input detected: climb ledge");
        //TODO
    }

    public static void ReleaseLedge(PlayerController Mario)
    {
        Debug.Log("input detected: release ledge");
        //TODO
    }
    #endregion multibutton

    #region A Button
    public static void WalkingJumpDecider(PlayerController Mario)
    {
        Debug.Log("input detected: walking jump (deciding)");
        //TODO
        BasicJump(Mario);
        //nb: also used while balancing
    }
    static void BasicJump(PlayerController Mario)
    {
        Debug.Log("input detected: jump");
        //TODO
    }
    static void SecondJump(PlayerController Mario)
    {
        Debug.Log("input detected: jump 2");
        //TODO
    }
    static void TripleJump(PlayerController Mario)
    {
        Debug.Log("input detected: jump 3");
        //TODO
    }
    static void TurnJump(PlayerController Mario)
    {
        Debug.Log("input detected: turn jump");
        //TODO
    }
    static void SpinJump(PlayerController Mario)
    {
        Debug.Log("input detected: spin jump");
        //TODO
    }
    static void SprayBackflipJump(PlayerController Mario)
    {
        Debug.Log("input detected: backflip spray jump");
        //TODO
    }
    static void FlutterJump(PlayerController Mario)
    {
        Debug.Log("input detected: flutter jump");
        //TODO
    }

    public static void SwimUpOrForward(PlayerController Mario)
    {
        Debug.Log("input detected: swim up/forward");
        //TODO
    }

    public static void JumpOffPole(PlayerController Mario)
    {
        Debug.Log("input detected: pole jump");
        //TODO
    }

    public static void JumpOffWall(PlayerController Mario)
    {
        Debug.Log("input detected: wall jump");
        //TODO
    }

    public static void SlideJump(PlayerController Mario)
    {
        Debug.Log("input detected: slide jump");
        //TODO
    }
    #endregion A Button

    #region B Button
    public static void WalkingBDecider(PlayerController Mario)
    {
        Debug.Log("input detected: walking B (deciding)");
        //TODO
        Pickup(Mario);
    }
    static void StartSlide(PlayerController Mario)
    {
        Debug.Log("input detected: start slide");
        //TODO
    }
    static void Pickup(PlayerController Mario)
    {
        Debug.Log("input detected: pick up");
        //TODO
    }
    public static void DropItem(PlayerController Mario)
    {
        Debug.Log("input detected: drop item");
        //TODO
    }

    public static void SwimDown(PlayerController Mario)
    {
        Debug.Log("input detected: swim down");
        //TODO
    }

    public static void FromBalanceToHang(PlayerController Mario)
    {
        Debug.Log("input detected: balance to hang");
        //TODO
    }

    public static void UseTongue(PlayerController Mario)
    {
        Debug.Log("input detected: use tongue");
        //TODO
    }

    public static void ReleasePole(PlayerController Mario)
    {
        Debug.Log("input detected: release pole");
        //TODO
    }

    public static void SlideHop(PlayerController Mario)
    {
        Debug.Log("input detected: slide hop");
        //TODO
    }
    #endregion B Button

    #region YButton
    public static void EnterCloseup(PlayerController Mario)
    {
        //nb: turn mario unless balancing
        Debug.Log("input detected: enter closeup");
        //TODO
    }
    #endregion YButton

    #region XButton
    public static void ChangeFluddMode(PlayerController Mario)
    {
        //go to dismountyoshi if riding
        Debug.Log("input detected: change fludd mode");
        //TODO
    }

    public static void DismountYoshi(PlayerController Mario)
    {
        Debug.Log("input detected: dismount yoshi");
        //TODO
    }
    #endregion XButton

    #region CStick
    public static void RotateCamera(PlayerController Mario)
    {
        //TODO
    }
    #endregion CStick

    #region Joystick
    public static void Move(PlayerController Mario)
    {
        //TODO
        //nb: also used while midair
        //nb: don't move if camera state is close up or locked, call rotate camera instead for closeup
        //nb: limit if sliding
    }

    public static void MoveAlongLedge(PlayerController Mario)
    {
        Debug.Log("input detected: ledge movement");
        //TODO
    }

    public static void MoveAroundPole(PlayerController Mario)
    {
        Debug.Log("input detected: pole movement");
        //TODO
    }

    public static void Strafe(PlayerController Mario)
    {
        Debug.Log("input detected: strafe");
        //TODO
    }

    public static void TurnOnSpot(PlayerController Mario)
    {
        Debug.Log("input detected: fixed movement / turn on spot");
        //TODO
    }

    //nb: also RotateCamera
    #endregion Joystick

    #region ZButton
    public static void ShowMap(PlayerController Mario)
    {
        Debug.Log("input detected: show map");
        //TODO
    }
    #endregion ZButton

    #region LButton
    public static void LockXAndCenter(PlayerController Mario)
    {
        CenterCamera(Mario);
        LockPositionAlongX(Mario);
    }

    public static void LockPositionAlongX(PlayerController Mario)
    {
        Debug.Log("input detected: lock X position");
        //TODO
    }
    public static void CenterCamera(PlayerController Mario)
    {
        Debug.Log("input detected: center camera");
    }
    public static void GroundPound(PlayerController Mario)
    {
        Debug.Log("input detected: ground pound");
        //TODO
    }
    #endregion LButton

    #region RButton
    public static void SprayWater(PlayerController Mario)
    {
        //TODO
        //nb: also used by balancing (turn limited)
    }
    public static void SprayWaterFromLedge(PlayerController Mario)
    {
        Debug.Log("input detected: spray water from ledge");
        //TODO
    }
    public static void SprayWaterFromHang(PlayerController Mario)
    {
        Debug.Log("input detected: spray water from hanging");
        //TODO
    }
    public static void LockPosition(PlayerController Mario)
    {
        Debug.Log("input detected: lock position");
    }
    public static void Refill(PlayerController Mario)
    {
        Debug.Log("input detected: refill water");
        //TODO
    }
    #endregion RButton
}
