using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    public GameObject InteractObj;
    public CameraManager Camera;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Actor>().jumpHax = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrySetInteractObj(GameObject Obj)
    {
        if(InteractObj == null || (transform.position - Obj.transform.position).magnitude < ((transform.position - InteractObj.transform.position).magnitude))
        {
            InteractObj = Obj;
        }
        //Camera.FocusObject = (GameObject)Obj.gameObject;

    }

    public void TryClearInteractObj(GameObject Obj)
    {
        if(InteractObj && InteractObj == Obj)
        {
            InteractObj = null;
            //Camera.FocusObject = null;
        }
    }

    public GameObject prefab;
    public void TestFunc()
    {
        Vector3 pos = new Vector3(Random.Range(-32f, 32f), 100, Random.Range(-32f, 32f));
        Instantiate(prefab, pos, new Quaternion());
    }
}
