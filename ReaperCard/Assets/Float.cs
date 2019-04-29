using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatOffsetY = 1.53f;
    public float floatSpeed = 8f;
    public float floatRadius = 1.5f;
    private float floatTimeOffset = 0f;

    void Start() {
      floatTimeOffset = Random.Range(0, 100);
    }
    void Update()
    {
      Vector3 offset = transform.localPosition;
      offset.y = Mathf.Sin(Time.time / floatSpeed + floatTimeOffset) * floatRadius + floatOffsetY;
      //print(y);
      transform.localPosition = offset;
    }
}
