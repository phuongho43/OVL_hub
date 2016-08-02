using UnityEngine;
using System.Collections;

public class ResetForm : MonoBehaviour {

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
