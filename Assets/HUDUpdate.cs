//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdate : MonoBehaviour
{

    Text m_Text;
    const float fpsMeasurePeriod = 0.5f;
    private int m_FpsAccumulator = 0;
    private float m_FpsNextPeriod = 0;
    private int m_CurrentFps;

    private GameObject cursor;

    void Start()
    {
        m_Text = GetComponent<Text>();
        cursor = GameObject.Find("Cursor");

        m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;        
    }

    // Update is called once per frame
    void Update()
    {
        // measure average frames per second
        m_FpsAccumulator++;
        if (Time.realtimeSinceStartup > m_FpsNextPeriod)
        {
            m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
            m_FpsAccumulator = 0;
            m_FpsNextPeriod += fpsMeasurePeriod;                            
        }

        m_Text.text = string.Format("FPS: {0}", m_CurrentFps)
            + "\n" + string.Format("VEL: {0} m/s", cursor.GetComponent<Mover>().cursorVel)
            + "\n" + string.Format("MODE: {0}", cursor.GetComponent<Mover>().modeString);
    }
}
