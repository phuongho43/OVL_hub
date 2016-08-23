using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;

public class Device : MonoBehaviour {

    public bool deviceStandby = false;
    public GameObject patientRepPrefab;
    public GameObject initPanelSelectionPrefab;
    public PatientDatabaseManager dbManager = new PatientDatabaseManager();
    private GameObject devicesManager;
    private GameObject deviceTitle;
    private GameObject initPanel;
    private GameObject initPanelSampleName;
    private GameObject portPanel;
    private GameObject donePanel;
    private Transform repListScrollContent;
    private GameObject toggleSwitch;
    private GameObject portTextInput;
    private GameObject progressCircle;
    private GameObject percentCompletedText;
    private SerialPort serialPort = new SerialPort();
	private KeyCode testButtonKey;
	private int trackingCase;
    private bool startTrackingBool = false;
    private int progress = 0;
    private string currentSampleName = "";


	// Use this for initialization
	void Awake () {
        devicesManager = GameObject.FindGameObjectWithTag("DevicesManager");
        deviceTitle = transform.GetChild(1).gameObject;
        initPanel = transform.GetChild(6).gameObject;
        portPanel = transform.GetChild(8).gameObject;
        donePanel = transform.GetChild(7).gameObject;
        repListScrollContent = transform.GetChild(5).GetChild(0);
        toggleSwitch = transform.GetChild(3).gameObject;
        portTextInput = transform.GetChild(8).GetChild(1).GetChild(1).GetChild(0).gameObject;
        progressCircle = transform.GetChild(4).GetChild(0).gameObject;
        percentCompletedText = transform.GetChild(4).GetChild(1).GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (toggleSwitch.GetComponent<Toggle>().isOn && startTrackingBool == false) {
            portPanel.SetActive(true);
        }

		if (startTrackingBool == true) {
			if (trackingCase == 0) {
				StartTracking();
			} else if (trackingCase == 1) {
				StartTracking_Test(testButtonKey);
			} else {
				// setActive button that sets progress = 100
			}
        }
        else {
            StopTracking();
        }

        if (progress >= 100) {
            donePanel.SetActive(true);
            toggleSwitch.GetComponent<Toggle>().isOn = false;
            startTrackingBool = false;
            progress = 0;
        }
	}

    public void SetDeviceTitle(string deviceType, string number) {
        string newTitle = deviceType + " #" + number;
        deviceTitle.GetComponent<Text>().text = newTitle;
    }

    public void SetFirstInitPanel(string sampleName) {
        if (initPanel.activeSelf == false) {
            initPanel.SetActive(true);
            initPanelSampleName = transform.GetChild(6).GetChild(1).GetChild(1).gameObject;
            initPanelSampleName.GetComponent<Text>().text = sampleName;
            currentSampleName = sampleName;
        }
    }

    public void SetLaterInitPanel(List<string> inputSamples) {
        initPanel.SetActive(true);
        Transform selectedSamplesScrollContent = transform.GetChild(6).GetChild(1).GetChild(1).GetChild(0);
        List<string> namesInSelectionList = new List<string>();
        foreach (Transform child in selectedSamplesScrollContent.transform) {
            namesInSelectionList.Add(child.GetComponentInChildren<Text>().text);
        }            
        foreach (string sampleName in inputSamples) {
            if (!namesInSelectionList.Contains(sampleName)) {
                GameObject selectionInstance = Instantiate(initPanelSelectionPrefab) as GameObject;
                selectionInstance.transform.SetParent(selectedSamplesScrollContent.transform, false);
                selectionInstance.GetComponentInChildren<Text>().text = sampleName;
            }
        }        
    }

    public void InstantiatePatientRep() {
        GameObject patientRepInstance = Instantiate(patientRepPrefab) as GameObject;
        patientRepInstance.transform.SetParent(repListScrollContent.transform, false);
        patientRepInstance.GetComponent<PatientRepManager>().SetName(currentSampleName);
    }

    public void InstantiateSelectedPatientReps(int finishedSamplesList) {
        Transform selectedSamplesScrollContent = transform.GetChild(6).GetChild(1).GetChild(1).GetChild(0);
        foreach (Transform selection in selectedSamplesScrollContent.transform) {
            if (selection.GetComponentInChildren<Toggle>().isOn) {
                selection.GetComponentInChildren<Toggle>().isOn = false;
                string sampleName = selection.GetComponentInChildren<Text>().text;
                GameObject patientRepInstance = Instantiate(patientRepPrefab) as GameObject;
                patientRepInstance.transform.SetParent(repListScrollContent.transform, false);
                patientRepInstance.GetComponent<PatientRepManager>().SetName(sampleName);
                if (finishedSamplesList == 0) {
                    devicesManager.GetComponent<DevicesManager>().finishedExtraction.Remove(sampleName);
                }
                else if (finishedSamplesList == 1) {
                    devicesManager.GetComponent<DevicesManager>().finishedCentrifugation.Remove(sampleName);
                }
                else if (finishedSamplesList == 2) {
                    devicesManager.GetComponent<DevicesManager>().finishedPCR.Remove(sampleName);
                }
            }
        }
        foreach (Transform child in selectedSamplesScrollContent) {
            GameObject.Destroy(child.gameObject);
        }
        deviceStandby = true;
    }

    public void SelectMoreSamples() {
        deviceStandby = false;
    }

	private bool ConsistsOfWhiteSpace(string s){
		foreach(char c in s){
			if(c != ' ') return false;
		}
		return true;
	}

    public void SetPort() {
        string port = portTextInput.GetComponent<InputField>().text;
//		if (!ConsistsOfWhiteSpace(port)) {
//			try {
//                serialPort.BaudRate = 9600;
//                serialPort.PortName = port;
//				trackingCase = 0;
//            } catch (Exception) {
//				try {
//					testButtonKey = (KeyCode)Enum.Parse(typeof(KeyCode), port);
//					trackingCase = 1;
//                } catch (Exception) {
//				
//				}
//			}
//		} else {
//			trackingCase = 2;
//		}
        testButtonKey = (KeyCode)Enum.Parse(typeof(KeyCode), port);
        trackingCase = 1;
		startTrackingBool = true;
    }

    private void StartTracking() {        
        progress = serialPort.ReadByte();
        progressCircle.GetComponent<Image>().fillAmount = progress / 100.0f;
        percentCompletedText.GetComponent<Text>().text = progress.ToString() + "%";
    }

	private void StartTracking_Test(KeyCode testButtonKey) {
		if (Input.GetKeyDown(testButtonKey)) {
			progress += 5;
			progressCircle.GetComponent<Image>().fillAmount = progress / 100.0f;
			percentCompletedText.GetComponent<Text>().text = progress.ToString() + "%";
		}
	}

    private void StopTracking() {
        progress = 0;
        progressCircle.GetComponent<Image>().fillAmount = 0f;
        percentCompletedText.GetComponent<Text>().text = "0%";
        //DONE 5b. Switch OFF: resets progress circle back to standby
    }

    public void StoreFinishedSamples(int finishedSamplesList) {// Place this function on the "OK" button of the "DONE!" panel
        foreach (Transform sample in repListScrollContent) {
            string sampleName = sample.gameObject.GetComponent<PatientRepManager>().sampleName;
            List<string> finishedSamples = new List<string>();
            if (finishedSamplesList == 0) {
                finishedSamples = devicesManager.GetComponent<DevicesManager>().finishedExtraction;
            }
            else if (finishedSamplesList == 1) {
                finishedSamples = devicesManager.GetComponent<DevicesManager>().finishedCentrifugation;
            }
            else if (finishedSamplesList == 2) {
                finishedSamples = devicesManager.GetComponent<DevicesManager>().finishedPCR;
            }
            finishedSamples.Add(sampleName);
            GameObject.Destroy(sample.gameObject);
        }
        donePanel.SetActive(false);
        if (finishedSamplesList == 0) {
            devicesManager.GetComponent<DevicesManager>().activeExtractors.Remove(transform.gameObject);
        }
        else if (finishedSamplesList == 1) {
            devicesManager.GetComponent<DevicesManager>().activeCentrifuges.Remove(transform.gameObject);
        }
        else if (finishedSamplesList == 2) {
            devicesManager.GetComponent<DevicesManager>().activeThermos.Remove(transform.gameObject);
        }
        GameObject.Destroy(transform.gameObject);
        //DONE 7. store all patientRepPrefab in List<> extractionFinished
        //DONE 8. Destroy all children in extractorSamplesScrollContent
        //DONE 8b. Destroy entire device main panel
    }

    public string GetViralLoadValue() {
        // interface with imager's python program
        // Test
        float loadValue = 252.0f;
        return loadValue.ToString();
        // Test
    }

    public void RecordViralLoad() {
        foreach (Transform sample in repListScrollContent) {
            string sampleName = sample.gameObject.GetComponent<PatientRepManager>().sampleName;
            sampleName = sampleName.Trim();
            char[] trimBrackets = {'[',']'};
            string patientID = sampleName.Split(' ')[0].Trim(trimBrackets);
            string viralLoad = GetViralLoadValue();
            Debug.Log(patientID);
            Dictionary<string,string> data = new Dictionary<string,string>();
            data.Add("patient_id", patientID);
            data.Add("load", viralLoad);
            dbManager.InsertPatientData("hiv_load", data);
            GameObject.Destroy(sample.gameObject);
        }
        GameObject.Destroy(transform.gameObject);
    }
}
