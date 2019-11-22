using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{

    private float height;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float getHeight()
    {

        return this.transform.localScale.y;
    }

    public void setHeight(float newHeight)
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x, newHeight, this.transform.localScale.z);
        this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
    }
}
