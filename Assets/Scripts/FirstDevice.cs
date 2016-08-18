using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class FirstDevice : MonoBehaviour {

    public GameObject deviceTitle;
    public GameObject initPanel;
    public GameObject initPanelSampleName;
    private string currentSampleName = "";
    public GameObject donePanel;
    public Transform repListScrollContent;
    public GameObject toggleSwitch;
    public GameObject patientRepPrefab;
    public GameObject progressCircle;
    public GameObject percentCompletedText;
    private int progress = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (toggleSwitch.GetComponent<Toggle>().isOn) {
            StartTracking();
        }
        else {
            StopTracking();
        }
        if (progress >= 100) {
            donePanel.SetActive(true);
            progress = 0;
        }
	}

    public void SetDeviceTitle(string deviceType, string number) {
        string newTitle = deviceType + " #" + number;
        deviceTitle.GetComponent<Text>().text = newTitle;
    }

    public void SetInitPanel(string sampleName) {
        initPanel.SetActive(true);
        initPanelSampleName.GetComponent<Text>().text = sampleName;
        currentSampleName = sampleName;
    }

    public void InstantiatePatientRep() {
        GameObject patientRepInstance = Instantiate(patientRepPrefab) as GameObject;
        patientRepInstance.transform.SetParent(repListScrollContent.transform, false);
        patientRepInstance.GetComponent<PatientRepManager>().SetName(currentSampleName);
    }

    public void StartTracking() {// Place this function on the toggle switch: --> "DONE!" panel pops up when progress is at 100%
        if (Input.GetKeyDown(KeyCode.Alpha1)) { // Change this when arduino integration finished
            progress += 5;
            progressCircle.GetComponent<Image>().fillAmount = progress / 100.0f;
            percentCompletedText.GetComponent<Text>().text = progress.ToString() + "%";
        }
        //DONE 5. Switch ON: adjust progress circle based on potentiometer reading
    }

    public void StopTracking() {
        progress = 0;
        progressCircle.GetComponent<Image>().fillAmount = 0f;
        percentCompletedText.GetComponent<Text>().text = "0%";
        //DONE 5b. Switch OFF: resets progress circle back to standby
    }

    public void StoreFinishedSamples() {// Place this function on the "OK" button of the "DONE!" panel
        GameObject devicesManager = GameObject.FindGameObjectWithTag("DevicesManager");
        foreach (Transform sample in repListScrollContent) {
            string sampleName = sample.gameObject.GetComponent<PatientRepManager>().sampleName;
            List<string> finishedSamples = devicesManager.GetComponent<DevicesManager>().finishedExtraction;
            finishedSamples.Add(sampleName);
            GameObject.Destroy(sample.gameObject);
        }
        donePanel.SetActive(false);
        devicesManager.GetComponent<DevicesManager>().activeExtractors.Remove(transform.gameObject);
        GameObject.Destroy(transform.gameObject);
        //DONE 7. store all patientRepPrefab in List<> extractionFinished
        //DONE 8. Destroy all children in extractorSamplesScrollContent
        //DONE 8b. Destroy entire device main panel
    }
}
