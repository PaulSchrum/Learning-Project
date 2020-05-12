using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Camera myCamera = null;
    private Dictionary<string, bool> keyStates = new Dictionary<string, bool>();
    private float xMove, zMove, zoom, speed;
    private Vector3 moveDelta;
    private float verticalSpeed = 0.0f, fallSpeed = -1f;
    private int debugCounter = 0;
    private float heightAdjust;

    // Start is called before the first frame update
    void Start()
    {
        heightAdjust = this.transform.localScale.y / 2f;
        speed = 1.1f;
        myCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        var letters = new List<string> { "a", "s", "d", "w", "z", "x", "j" };
        foreach (var letter in letters)
            keyStates[letter] = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        SetKeyStates();

        if (keyStates["j"] && aboveGroundHeight <= 0f)
        {
            verticalSpeed = 0f;
            moveDelta = new Vector3(0f, 0.99f, 0f);
            this.transform.Translate(moveDelta);
            myCamera.transform.Translate(moveDelta);
        }

        xMove = Convert.ToSingle(keyStates["d"]) - Convert.ToSingle(keyStates["a"]);
        zMove = Convert.ToSingle(keyStates["w"]) - Convert.ToSingle(keyStates["s"]);
        zoom = Convert.ToSingle(keyStates["x"]) - Convert.ToSingle(keyStates["z"]);
        zoom *= 1.1f;

        moveDelta = new Vector3(xMove * speed * Time.deltaTime, 
            verticalSpeed * Time.deltaTime,
            zMove * speed * Time.deltaTime);
        this.transform.Translate(moveDelta);
        myCamera.transform.Translate(moveDelta);
        myCamera.fieldOfView += zoom;
        myCamera.fieldOfView = validateRange(myCamera.fieldOfView, 10f, 100f);

        verticalSpeed = 0f;
        if (aboveGroundHeight > 0f)
            verticalSpeed = fallSpeed;
    }

    private float aboveGroundHeight
    {
        get { return this.transform.position.y - heightAdjust; }
    }

    private void SetKeyStates()
    {
        keyStates["a"] = Input.GetKey(KeyCode.A);
        keyStates["s"] = Input.GetKey(KeyCode.S);
        keyStates["d"] = Input.GetKey(KeyCode.D);
        keyStates["w"] = Input.GetKey(KeyCode.W);
        keyStates["z"] = Input.GetKey(KeyCode.Z);
        keyStates["x"] = Input.GetKey(KeyCode.X);
        keyStates["j"] = Input.GetKey(KeyCode.J);
    }

    float validateRange(float inValue, float minVal, float maxVal)
    {
        if (inValue < minVal) return minVal;
        if (inValue > maxVal) return maxVal;
        return inValue;
    }
}
