using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatSpeed = 8f;
    public float floatRadius = 1.5f;
    private float floatTimeOffset = 0f;
    private float startY;

    void Start() {
      floatTimeOffset = Random.Range(0, 100);
      startY = transform.localPosition.y;
    }
    void Update()
    {
      Vector3 offset = transform.localPosition;
      offset.y = startY + Mathf.Sin(Time.time / floatSpeed + floatTimeOffset) * floatRadius;
      //print(y);
      transform.localPosition = offset;
    }
}
