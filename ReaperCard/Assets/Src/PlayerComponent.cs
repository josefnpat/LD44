using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    public GameObject InteractObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrySetInteractObj(GameObject Obj)
    {
        InteractObj = Obj;
    }

    public void TryClearInteractObj(GameObject Obj)
    {
        if(InteractObj && InteractObj == Obj)
        {
            InteractObj = null;
        }
    }
}
