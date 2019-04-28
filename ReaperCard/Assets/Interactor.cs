using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private InputWrapper controls = new InputWrapper();
    private GameObject interactable;

    public void setInteractable(GameObject obj) {
        if(interactable != null) {
            var dist = (transform.position - obj.transform.position).magnitude;
            var newDist = (transform.position - interactable.transform.position).magnitude;
            bool isCloser = dist < newDist;
            if(!isCloser)
                return;
        }
        interactable = obj;
    }

    public void clearInteractable(GameObject obj) {
        if(interactable == obj) interactable = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(interactable != null && controls.IsDown(EKey.Confirm)) {
            interactable.GetComponent<Interactible>().doInteraction(this.gameObject);
        }
    }
}
