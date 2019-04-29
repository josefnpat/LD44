using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class Interactor : MonoBehaviour
{
    [HideInInspector]
    private InputWrapper controls = new InputWrapper();
    private HashSet<GameObject> interactables = new HashSet<GameObject>();

    public void setInteractable(GameObject obj)
    {
        interactables.Add(obj);
    }

    public void clearInteractable(GameObject obj)
    {
        interactables.Remove(obj);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Actor>().GetState() != EActorState.Walking)
            return;

        if (interactables.Count > 0 && controls.IsDown(EKey.Confirm))
        {
            GameObject bestMatch = null;
            var bestDistance = Mathf.Infinity;
            foreach (var interactable in interactables)
            {
                if (interactable == null) continue;
                var dist = (transform.position - interactable.transform.position).magnitude;
                if (dist < bestDistance)
                {
                    bestMatch = interactable;
                    bestDistance = dist;
                }
            }

            if (bestMatch != null)
            {
                bestMatch.GetComponent<Interactible>().doInteraction(this.gameObject);
            }
            interactables.RemoveWhere(obj => obj == null);
        }
    }
}
