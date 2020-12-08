using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceNavigatorDriver;
using UnityEngine.UI;


public class Mover : MonoBehaviour
{
    private int mode = 0;    
    public string modeString = string.Empty;

    private Vector3 posLF; // cursor position last frame
    public float cursorVel; // measured cursor velocity

    private GameObject birdie;
    private GameObject mainCamera;

    private bool spacemouse_flag = false;


    // YOUR CODE - BEGIN
    
    //Exercies3.2:initiate two variables to store the bird’s velocity over frames
    
    float currentAxisX = 0.0f;
    float currentAxisY = 0.0f;

    //Exercise3.3
    float currentAccelerationX = 0.0f;
    float currentAccelerationY = 0.0f;

    float currentSpeedX = 0.0f;
    float currentSpeedY = 0.0f;



    // YOUR CODE - END

    // Start is called before the first frame update
    void Start()
    {
        posLF = transform.position;

        birdie = GameObject.Find("Birdie");
        mainCamera = GameObject.Find("Main Camera");

        // initial states
        SetMode(1); // isotonic position control
    }

    // Update is called once per frame
    void Update()
    {
      
        UpdateMode();
        InputMapping();

        // check for birdie visibility
        bool flag = IsRenderedFrom(birdie.GetComponent<Renderer>(), mainCamera.GetComponent<Camera>());
        if (flag == false)
        {
            transform.position = posLF;
        }

        // calc actual velocity if birdie (displayed in HUD)
        float dist = Vector3.Distance(posLF, transform.position); // moved distance from last to actual frame
        posLF = transform.position;
        cursorVel = dist / Time.deltaTime;
        cursorVel = (float)System.Math.Round(cursorVel, 2);
        //Debug.Log(cursorVel);

    }


    void InputMapping()
    {
        // isotonic input
        float isotonicX = Input.GetAxis("Mouse X");
        float isotonicY = Input.GetAxis("Mouse Y");
        //Debug.Log(isotonicX + " " + isotonicY);

        // elastic input
        float elasticX = 0.0f;
        float elasticY = 0.0f;

        // select elastic input device
        if (spacemouse_flag == true) // elastic input from spacemouse
        {
            elasticX = SpaceNavigator.Translation.x;
            elasticY = SpaceNavigator.Translation.y;
        }
        else // elastic input from joystick/gamepad
        {

            elasticX = Input.GetAxis("Horizontal") * 0.15f;
            elasticY = Input.GetAxis("Vertical") * -0.15f;
        }

        //Debug.Log(elasticX + " " + elasticY);

        switch (mode)
        {
            case 1:
                IsotonicPosition(isotonicX, isotonicY);
                break;
            case 2:
                IsotonicRate(isotonicX, isotonicY);
                break;
            case 3:
                IsotonicAcceleration(isotonicX, isotonicY);
                break;
            case 4:
                ElasticPosition(elasticX, elasticY);
                break;
            case 5:
                ElasticRate(elasticX, elasticY);
                break;
            case 6:
                ElasticAcceleration(elasticX, elasticY);
                break;
            default:
                break;
        }
    }

    void IsotonicPosition(float X, float Y)
    {
        float factor = 0.05f;
        transform.Translate(X * factor, Y * factor, 0.0f);
    }

    void IsotonicRate(float X, float Y)
    {
        // YOUR CODE - BEGIN
        Debug.Log("Value of isotonicX: " + X + " Value of isotonocY: " + Y);

        currentAxisX += X;
        currentAxisY += Y;

        Debug.Log("Value of currentAxisX: " + currentAxisX + " Value of currentAxisY: " + currentAxisY);


        //Set the scaling factor: speed
        //Refrence: https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
        float horizontalSpeed = 1.0f;
        float verticalSpeed = 1.0f;

        /*
        Time.deltaTime: The completion time in seconds since the last frame (Read Only).
        This property provides the time between the current and previous frame.
         */

        //Make it move (meters per second) instead of (meters per frame)
        horizontalSpeed *= Time.deltaTime;
        verticalSpeed *= Time.deltaTime; 

        // Get the mouse delta. This is not in the range -1...1
        float h = horizontalSpeed * currentAxisX;
        float v = verticalSpeed * currentAxisY;

        transform.Translate(h, v, 0.0f);

        // YOUR CODE - END    
    }

    void IsotonicAcceleration(float X, float Y)
    {
        // YOUR CODE - BEGIN
        Debug.Log("Value of isotonicX: " + X + " Value of isotonocY: " + Y);

        currentAccelerationX += X;
        currentAccelerationY += Y;

        currentSpeedX += currentAccelerationX;
        currentSpeedY += currentAccelerationY;

        Debug.Log("Value of currentSpeedX: " + currentSpeedX + " Value of currentSpeedY: " + currentSpeedY);


        //Set the scaling factor: speed
        //Refrence: https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
        float horizontalSpeed = 0.001f;
        float verticalSpeed = 0.001f;

        /*
        Time.deltaTime: The completion time in seconds since the last frame (Read Only).
        This property provides the time between the current and previous frame.
         */

        //Make it move (meters per second) instead of (meters per frame)
        horizontalSpeed *= Time.deltaTime;
        verticalSpeed *= Time.deltaTime;

        // Get the mouse delta. This is not in the range -1...1
        float h = horizontalSpeed * currentSpeedX;
        float v = verticalSpeed * currentSpeedY;

        transform.Translate(h, v, 0.0f);

        // YOUR CODE - END
    }

    void ElasticPosition(float X, float Y)
    {
        // YOUR CODE - BEGIN
        
        //Debug.Log(Input.GetJoystickNames());
        Debug.Log("Value of Input.GetAxis Horizontal: " + Input.GetAxis("Horizontal") + " Value of Input.GetAxis Vertical: " + Input.GetAxis("Vertical"));

        float factor = 2.0f;
        factor *= Time.deltaTime;
        

        //Vector3 originLocation = transform.localPosition;

        if (Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Abs(0.19f) && Mathf.Abs(Input.GetAxis("Vertical")) < Mathf.Abs(0.19f)) {

            transform.localPosition = new Vector3(0, 0, 0);
            X = 0;
            Y = 0;
           
            
        }
        else
        {
            transform.Translate(X * factor, Y * factor, 0.0f);

        };

        

        

        // YOUR CODE - END
    }

    void ElasticRate(float X, float Y)
    {
        // YOUR CODE - BEGIN
        Debug.Log("Value of Input.GetAxis Horizontal: " + Input.GetAxis("Horizontal") + " Value of Input.GetAxis Vertical: " + Input.GetAxis("Vertical"));
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Abs(0.19f) && Mathf.Abs(Input.GetAxis("Vertical")) < Mathf.Abs(0.19f))
        {

            //transform.localPosition = new Vector3(0, 0, 0);
            X = 0;
            Y = 0;


        }
        else
        {
            float speed = 2.0f;

            speed *= Time.deltaTime;

            transform.Translate(speed * X, speed * Y, 0.0f);

        };
       



        // YOUR CODE - END
    }
    void ElasticAcceleration(float X, float Y)
    {
        // YOUR CODE - BEGIN

        // YOUR CODE - END
    }


    void Recenter()
    {
        transform.localPosition = Vector3.zero;
    }

    void SetMode(int MODE)
    {
        mode = MODE;

        switch (mode)
        {
            case 1:
                modeString = "isotonic position";                
                break;
            case 2:
                modeString = "isotonic rate";                
                break;
            case 3:
                modeString = "isotonic acceleration";                
                break;
            case 4:
                modeString = "elastic position";                
                break;
            case 5:
                modeString = "elastic rate";                
                break;
            case 6:
                modeString = "elastic acceleration";                
                break;
        }

        // print to console
        Debug.Log(string.Format("New Mode: {0}", modeString));

    }

    void UpdateMode()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetMode(1); // isotonic position
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetMode(2); // isotonic rate
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetMode(3); // isotonic acceleration
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetMode(4); // elastic position
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SetMode(5); // elastic rate
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SetMode(6); // elastic acceleration
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Recenter();
        }
    }

    bool IsRenderedFrom(Renderer renderer, Camera camera)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }


    // Excercise 3.7:
    // Which combinations were suitable for this task, which combinations were less suitable? Why? 
    //Come up with potential use cases for the six different combinations.Think in the context of object manipulation and viewpoint navigation.
    
    // YOUR EXPLANATION - BEGIN

    // YOUR EXPLANATION - END

}