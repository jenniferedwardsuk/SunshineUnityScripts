using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public float MarioHealth = 8;
    public float MarioMaxHealth = 8;
    public Vector3 startPoint;
    public float speed;
    public float jumpPower;
    bool _moving;
    bool _jumping;
    public bool moving
    {
        get { return _moving; }
        set { }
    }
    public bool jumping
    {
        get { return _jumping; }
        set { }
    }
    Vector3 movement;
    Animator anim;
    Rigidbody rb;
    GameObject dummyObject;
    Transform mainCamera;
    public GameObject Fludd;

    public UnityEvent OnHitLedgeEvent; //invoked in OnCollisionEnter below
    public UnityEvent OnUngroundedEvent; //invoked below
    public UnityEvent OnGroundedEvent; //invoked below
    public UnityEvent OnStartBalancingEvent; //invoked in OnCollisionEnter below
    public UnityEvent OnHitWallEvent; //invoked in OnCollisionEnter below
    public UnityEvent OnStartSlideEvent; //todo: invoke in StartSlide action
    public UnityEvent OnHitPoleEvent; //invoked in OnCollisionEnter below
    public UnityEvent OnEnterWaterEvent; //invoked in OnCollisionEnter below

    //todo: tag meshes
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            bool angle = false;
            bool edge = false;
            //todo: check angle to decide
            if (edge)
            {
                OnHitLedgeEvent.Invoke();
            }
            else if (angle)
            {
                OnHitWallEvent.Invoke();
            }
        }
        if (collision.gameObject.tag == "Pole")
        {
            OnHitPoleEvent.Invoke();
        }
        if (collision.gameObject.tag == "Balance")
        {
            OnStartBalancingEvent.Invoke();
        }
        if (collision.gameObject.tag == "Water")
        {
            OnEnterWaterEvent.Invoke();
        }
    }

    private void Awake()
    {
        
    }

    void Start ()
    {
        dummyObject = new GameObject();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        rb = this.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // set up inputs for default state
        MarioFSM.changeState(MarioFSM.MarioMovementState);
    }

    bool groundedLastFrame;
    bool groundedThisFrame;
	void Update ()
    {
        //check for entering midair
        groundedThisFrame = false;
        if (isGrounded(this.gameObject, 0f))
        {
            groundedThisFrame = true;
        }

        if (!groundedThisFrame && groundedLastFrame)
        {
            OnUngroundedEvent.Invoke();
        }
        else if (groundedThisFrame && !groundedLastFrame)
        {
            OnGroundedEvent.Invoke();
        }

        //todo: move this to a fludd controller
        //Fludd.transform.position = this.gameObject.transform.position + new Vector3(0f, 0.03f, 1.5f);

        InputManager.handleInput(this);

        float h = 0;
        float v = 0;

        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            h = Input.GetAxisRaw("Horizontal");
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            v = Input.GetAxisRaw("Vertical");

        //if (rb)
        //    rb.velocity = new Vector3(0, 0, 0); // prevent collision interference with player-controlled movement

        if (h != 0 || v != 0)
        {
            _moving = true;
        }
        else
        {
            _moving = false;
        }

        Move(h, v);

        Animating(h, v);
    }

    private void LateUpdate()
    {
        groundedLastFrame = groundedThisFrame;
    }

    private void FixedUpdate()
    {
        if (Input.GetAxisRaw("Jump") != 0 && isGrounded(this.gameObject, 0))
        {
            _jumping = true;
            rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WarpPlane")
        {
            this.transform.position = startPoint;
        }
    }

    public bool isGrounded(GameObject gameObj, float objHeightToGround)
    {
        int floorMask = LayerMask.GetMask("Floor");
        return Physics.Raycast(gameObj.transform.position, -Vector3.up, objHeightToGround + 0.1f, floorMask);
    }

    void Move(float h, float v)
    {
        Transform flatCameraTransform = dummyObject.transform;
        Quaternion flatCameraRotation = flatCameraTransform.rotation;
        flatCameraRotation.eulerAngles = new Vector3(0, mainCamera.transform.rotation.eulerAngles.y, 0);
        flatCameraTransform.rotation = flatCameraRotation;

        movement = flatCameraTransform.forward * v + flatCameraTransform.right * h;
        movement = movement.normalized * speed * Time.deltaTime;
        this.transform.localPosition += movement;

        if (movement.x == 0 && movement.y == 0 && movement.z == 0)
        {
            h = 0;
            v = 0;
        }
        
        // rotate to face movement direction if moving
        if ((v != 0 || h != 0))
        {
            if (v > 0)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movement), 0.15f);
            }
            else // without this, rotation is faster when moving backwards
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(movement), 0.05f);
            }
        }

    }

    bool jumpinprogress = false;
    void Animating(float h, float v)
    {
        if (isGrounded(this.gameObject, 0))
        {
            bool walking = h != 0f || v != 0f;
            anim.SetBool("IsMoving", walking);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        if (!isGrounded(this.gameObject, 0))
        {
            if (_jumping)
            {
                anim.SetTrigger("jump");
                _jumping = false;
                jumpinprogress = true;
            }
        }
        else if (jumpinprogress)
        {
            anim.SetTrigger("jumplanding");
            jumpinprogress = false;
        }

    }
}
