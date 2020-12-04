using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetSpawn : MonoBehaviour
{

    private List<GameObject> targets;
    
    private GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        targets = new List<GameObject>();

        foreach (int value in Enumerable.Range(1, 10))
        {            
            GameObject target = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            target.transform.parent = this.gameObject.transform;
            target.name = "Target" + value;
            target.transform.localPosition = new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-1.0f, 1.0f), 0.0f);
            target.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            target.GetComponent<MeshRenderer>().material.color = Color.blue;

            targets.Add(target);
        }

        cursor = GameObject.Find("Cursor");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject target in targets)
        {
            float dist = Vector3.Distance(target.transform.position, cursor.transform.position);
            //Debug.Log(target.name + " " + dist);
            if (dist < 0.02)
            {
                target.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
