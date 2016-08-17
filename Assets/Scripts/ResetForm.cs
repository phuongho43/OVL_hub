using UnityEngine;
using System.Collections;

public class ResetForm : MonoBehaviour {

    // Resets the new patient form after submit is pressed so that currently filled inputfields don't carry over to the next time

    public GameObject Form;
    public Transform Container;

    public void Reset() {
        foreach (Transform child in Container.transform) {
            GameObject.Destroy(child.gameObject);
        }

        GameObject instance = Instantiate(Form) as GameObject;
        instance.transform.SetParent(Container.transform, false);
    }
}
