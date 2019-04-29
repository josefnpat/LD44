using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemAnim : MonoBehaviour
{
    public float animDuration = 3.0f;
    public float animEndDuration = 0.5f;
    public float height = 8.0f;
    public float startRadius = 5.0f;
    public float rotSpeed = 3.0f; // rotations per second

    private float startTime;
    private bool doneSoundPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init(InventoryItem item)
    {
        startTime = Time.time;
        GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", item.texture);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.time - startTime;
        float percent = Mathf.Clamp(dt / animDuration, 0f, 1f);
        float fullHeight = height * percent;
        float radius = Mathf.Lerp(startRadius, 0f, percent);
        float angle = 2.0f * Mathf.PI * rotSpeed * dt;
        float x = Mathf.Cos(angle) * radius;
        float y = fullHeight;
        float z = Mathf.Sin(angle) * radius;
        transform.localPosition = new Vector3(x, y, z);

        if (dt > animDuration)
        {
            if(!doneSoundPlayed) {
                SoundManager.instance.Play("itempickupdone");
                doneSoundPlayed = true;
            }
        }

        if (dt > animDuration + animEndDuration)
        {
            Destroy(gameObject);
        }
    }
}
