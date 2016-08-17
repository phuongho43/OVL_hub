using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class FirstDevice : MonoBehaviour {

    private GameObject deviceTitle;
    private GameObject initPanel;
    private Transform repListScrollContent;
    public GameObject patientRepPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetDeviceTitle(string deviceType, string number) {
        string newTitle = deviceType + " #" + number;
        deviceTitle = transform.GetChild(1).gameObject;
        deviceTitle.GetComponent<Text>().text = newTitle;
    }

    public void SetInitPanel(string sampleName) {
        initPanel = transform.GetChild(5).gameObject;
        initPanel.SetActive(true);
        initPanel.GetComponent<FirstInitPanel>().SetSampleName(sampleName);
    }

    public void InstantiatePatientRep(string sampleName) {
        GameObject patientRepInstance = Instantiate(patientRepPrefab) as GameObject;
        repListScrollContent = transform.GetChild(4).GetChild(0);
        patientRepInstance.transform.SetParent(repListScrollContent.transform, false);
        patientRepInstance.transform.GetChild(1).GetComponent<Text>().text = sampleName;
    }
}
