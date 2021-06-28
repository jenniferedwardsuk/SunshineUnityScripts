using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class MarioFSM {
    
    private static void Awake()
    {
        // set up inputs for default state
        //MarioFSM.changeState(MarioFSM.MarioMovementState);
    }

    public enum MovementState { Walking, HoldingLedge, Midair, Balancing, WallSliding, GroundSliding, PoleClimbing, Swimming};

    public enum BodyState { Normal, Spraying, OnYoshi, OnYoshiSpraying, HoldingItem };

    public enum CameraState { Normal, CloseUp, LLock, RLock }; //todo: stop c stick from functioning in closeup mode

    public static MovementState MarioMovementState = MovementState.Walking;
    public static BodyState MarioBodyState = BodyState.Normal;
    public static CameraState MarioCameraState = CameraState.Normal;

    public static KeyValuePair<MovementState, InputSet> currentMarioInputs;

    public static void changeState(MovementState newState)
    {
        currentMarioInputs = InputManager.changeInputs(newState);
    }


    //todo: implement these events
    static void OnHitLedge()
    {
        changeState(MovementState.HoldingLedge);
    }
    static void OnUngrounded()
    {
        changeState(MovementState.Midair);
    }
    static void OnStartBalancing()
    {
        changeState(MovementState.Balancing);
    }
    static void OnHitWall()
    {
        changeState(MovementState.WallSliding);
    }
    static void OnStartSlide()
    {
        changeState(MovementState.GroundSliding);
    }
    static void OnHitPole()
    {
        changeState(MovementState.PoleClimbing);
    }
    static void OnEnterWater()
    {
        changeState(MovementState.Swimming);
    }
}

