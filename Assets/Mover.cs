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

    //Exercise3.5
    float speedX = 0.0f;
    float speedY = 0.0f;



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

        /* moving the mouse in a certain direction results in a change of the bird’s velocity, which is in turn applied to a change of the
         bird’s position every frame. When the mouse stops moving, the bird keeps moving with the previously defined velocity.
         To stop the bird, the mouse needs to be moved back to its start position. */

        Debug.Log("Value of isotonicX: " + X + " Value of isotonocY: " + Y);

        //To store the velocity value 
        currentAxisX += X;
        currentAxisY += Y;

        Debug.Log("Value of currentAxisX: " + currentAxisX + " Value of currentAxisY: " + currentAxisY);


        //Set the scaling factor: speed
        //Refrence: https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
        float speedFactor = 1.0f;
        

        /*
        Time.deltaTime: The completion time in seconds since the last frame (Read Only).
        This property provides the time between the current and previous frame.
         */

        //Make it move (meters per second) instead of (meters per frame)
        speedFactor *= Time.deltaTime;

        // Get translate value along x axis(h) and y axis(v) 
        float h = speedFactor * currentAxisX;
        float v = speedFactor * currentAxisY;

        transform.Translate(h, v, 0.0f);

        // YOUR CODE - END    
    }

    void IsotonicAcceleration(float X, float Y)
    {
        // YOUR CODE - BEGIN
        /*
         * moving the mouse in a certain direction results in a change of the bird’s acceleration, which is applied to a change of the bird’s velocity every frame, 
         * which is in turn applied to a change of the bird’s position every frame. 
         * When the mouse stops moving, the bird keeps getting faster with respect to the previously defined acceleration. 
         * To stop the bird, the mouse needs to be moved in the inverse direction for the same amount of time.
         */
        Debug.Log("Value of isotonicX: " + X + " Value of isotonocY: " + Y);

        //To store the acceleration value in each frame
        currentAccelerationX += X;
        currentAccelerationY += Y;

        //To store the speed value in each frame
        currentSpeedX += currentAccelerationX;
        currentSpeedY += currentAccelerationY;

        Debug.Log("Value of currentSpeedX: " + currentSpeedX + " Value of currentSpeedY: " + currentSpeedY);


        //Set the scaling factor: speed
        float speedFactor = 0.0001f;
        
        
        //Make it move (meters per second) instead of (meters per frame)
        speedFactor *= Time.deltaTime;


        // Get translate value along x axis(h) and y axis(v) 
        float h = speedFactor * currentSpeedX;
        float v = speedFactor * currentSpeedY;

        transform.Translate(h, v, 0.0f);

        // YOUR CODE - END
    }

    void ElasticPosition(float X, float Y)
    {
        // YOUR CODE - BEGIN
        /* Moving a joystick in one direction with position control and then releasing it leads to snap back to starting position
         */
        Debug.Log("Value of Input.GetAxis Horizontal: " + Input.GetAxis("Horizontal") + " Value of Input.GetAxis Vertical: " + Input.GetAxis("Vertical"));

        float speedFactor = 2.0f;
        speedFactor *= Time.deltaTime;
        
        //detect if the joystick snap, values less than the range are mapped to neutral(indicate the joystick is snapped) 
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Abs(0.19f) && Mathf.Abs(Input.GetAxis("Vertical")) < Mathf.Abs(0.19f)) {

            //set the bird back to starting position
            transform.localPosition = new Vector3(0, 0, 0);
            X = 0;
            Y = 0;

            /* 
            // Moves bird smoothly toward target (starting position)
            float speed = 3.0f;
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, 0), step);
            */


        }
        else
        {
            transform.Translate(X * speedFactor, Y * speedFactor, 0.0f);

        };

            

        // YOUR CODE - END
    }

    void ElasticRate(float X, float Y)
    {
        // YOUR CODE - BEGIN
        Debug.Log("Value of Input.GetAxis Horizontal: " + Input.GetAxis("Horizontal") + " Value of Input.GetAxis Vertical: " + Input.GetAxis("Vertical"));
        /*
         * Moving a joystick in one direction with rate control and then releasing it leads to cursor stop
         */

        //when joystick snaps
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Abs(0.19f) && Mathf.Abs(Input.GetAxis("Vertical")) < Mathf.Abs(0.19f))
        {

            //stop translate the bird
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
        /* Moving a joystick in one direction with acceleration control and then releasing it leads to
         * the cursor move continuously with the adjusted velocity
         */
        Debug.Log("Value of Input.GetAxis Horizontal: " + Input.GetAxis("Horizontal") + " Value of Input.GetAxis Vertical: " + Input.GetAxis("Vertical"));
        Debug.Log("Value of X: " + X + " Value of Y: " + Y);

        float speedFactor = 0.05f;
        speedFactor *= Time.deltaTime;

        if (Mathf.Abs(Input.GetAxis("Horizontal")) < Mathf.Abs(0.19f) && Mathf.Abs(Input.GetAxis("Vertical")) < Mathf.Abs(0.19f))
        {

            
            //cursor move with constant velocity after joystick snaps
            transform.Translate(speedFactor * speedX, speedFactor * speedY, 0.0f);


        }
        else
        {
            //Acceleration 
            speedX += X;
            speedY += Y;

            transform.Translate(speedFactor * speedX, speedFactor * speedY, 0.0f);

        };

        

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

    /* 
    
    For controlling the position of the bird, the use of an isotonic device (i.e. mouse) and the IsotonicPosition function were more compatible. 
   An elastic device (i.e. Space Mouse) and the ElasticPosition function were less suitable for this task. 
   This is based on our own experience controlling the position of the bird, but primarily Zhai's findings (1995) on human performance with input control that demonstrate the difficulty of operating isometric position control.

    For controlling the velocity of the bird, the use of an elastic device and the ElasticRate function were more compatible. 
   An isotonic device and the IsotonicRate function were less suitable for this task. 
   Again we discovered this through our own experiences, but also the Zhai's findings that rate control is slightly easier with isometric devices.
   For controlling the acceleration of the bird, we made the same observations as controlling the velocity of the bird.

   */

    /*
    Isotonic position use case:

    In a virtual environment, a user takes one teapot and move it from top left corner of cupboard to bottom right. 
     */

    /*
    Isotonic rate use case:

    In a virtual environment, a space women in her capsule grabs a paper airplane and launch it. The paper airplane would keep moving with the previously defined velocity.  
     */

    /*
    Isotonic acceleration use case:

    Simulation in a virtual environment, user use virtual slider to set the object’s acceleration for experiment purpose.
     */

    /*
    Elastic position use case:

    Editing 360 video in immersive environment, editor would like to have a quick preview of fast-forwarding the video, and then either snapping back to the original location or set a new desired location. 
     */

    /*
    Elastic rate use case:

    In virtual environment, user use joystick controller to walk around.
    */

    /*
    Elastic acceleration use case:

    In virtual environment, user is a spaceman with astronaut propulsion unit and use joystick controller to navigate, once the joystick snap back, user would just drift into space
    */





    // YOUR EXPLANATION - END

}