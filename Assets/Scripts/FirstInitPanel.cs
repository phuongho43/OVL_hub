using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FirstInitPanel : MonoBehaviour {

    private GameObject sampleNameBox;
    private string currentSampleName = "";
    private GameObject firstDevice;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetSampleName(string name) {
        sampleNameBox = transform.GetChild(1).GetChild(1).gameObject;
        sampleNameBox.GetComponent<Text>().text = name;
        currentSampleName = name;
    }

    public void Cancel() {
        transform.gameObject.SetActive(false);
    }

    public void Confirm() {
        firstDevice = transform.parent.gameObject;
        firstDevice.GetComponent<FirstDevice>().InstantiatePatientRep(currentSampleName);
        transform.gameObject.SetActive(false);
    }
}
