using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SpaceNavigatorDriver;
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

        // YOUR CODE - END    
    }

    void IsotonicAcceleration(float X, float Y)
    {
        // YOUR CODE - BEGIN

        // YOUR CODE - END
    }

    void ElasticPosition(float X, float Y)
    {
        // YOUR CODE - BEGIN

        // YOUR CODE - END
    }

    void ElasticRate(float X, float Y)
    {
        // YOUR CODE - BEGIN

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