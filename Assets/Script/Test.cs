using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private float x, y;
    private void Awake()
    {
        x = UnityEngine.Random.Range(300, 600);
        y = UnityEngine.Random.Range(300, 600);
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(x, y, 300, 300), "ok");
    }
}
