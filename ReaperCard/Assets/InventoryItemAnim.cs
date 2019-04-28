using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemAnim : MonoBehaviour
{
    private InventoryItem item;
    private float startTime;

    private const float ANIM_DURATION = 3.0f;
    private const float ANIM_END_DURATION = 0.5f;
    private const float HEIGHT = 8.0f;
    private const float START_RADIUS = 5.0f;
    private const float ROT_SPEED = 3.0f; // rotations per second

    // Start is called before the first frame update
    void Start()
    {
    }

    void Init(InventoryItem _item) {
        item = _item;
        startTime = Time.time;
        GetComponent<MeshRenderer>().material.mainTexture = item.texture;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.time - startTime;
        float percent = Mathf.Clamp(dt / ANIM_DURATION, 0f, 1f);
        float height = HEIGHT * percent;
        float radius = Mathf.Lerp(START_RADIUS, 0f, percent);
        float angle = 2.0f * Mathf.PI * ROT_SPEED * dt;
        float x = Mathf.Cos(angle) * radius;
        float y = height;
        float z = Mathf.Sin(angle) * radius;
        transform.localPosition = new Vector3(x, y, z);

        if(dt > ANIM_DURATION) {
            // TODO: play sound
        }

        if(dt > ANIM_DURATION + ANIM_END_DURATION) {
            Destroy(gameObject);
        }
    }
}
