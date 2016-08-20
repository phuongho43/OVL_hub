using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
//refactor the 4 functions into 1
public class DevicesManager : MonoBehaviour {

    // Labels for samples
    public Text idTextbox;
    public Text lastnameTextbox;
    // Extractor
    public Transform extractorListScrollContent;
    public GameObject extractorPrefab;
    public List<GameObject> activeExtractors = new List<GameObject>();
    private GameObject availableExtractor;
    public List<string> finishedExtraction = new List<string>();
    // Centrifuge
    public Transform centriListScrollContent;
    public GameObject centrifugePrefab;
    public List<GameObject> activeCentrifuges = new List<GameObject>();
    private GameObject availableCentrifuge;
    public List<string> finishedCentrifugation = new List<string>();
    // ThermoCycler
    public Transform thermoListScrollContent;
    public GameObject thermoPrefab;
    public List<GameObject> activeThermos = new List<GameObject>();
    private GameObject availableThermo;
    public List<string> finishedPCR = new List<string>();
    // Imager
    public Transform imagerListScrollContent;
    public GameObject imagerPrefab;
    public List<GameObject> activeImagers = new List<GameObject>();
    private GameObject availableImager;


    void Update () {
        if (finishedExtraction.Count > 0) {
            PrepCentrifuge();
        }
        if (finishedCentrifugation.Count > 0) {
            PrepThermoCycler();
        }
        if (finishedPCR.Count > 0) {
            PrepImager();
        }
    }

    public void PrepFirstDevice() {
        string patientID = idTextbox.GetComponent<Text>().text.Trim();
        string lastName = lastnameTextbox.GetComponent<Text>().text.Trim();
        string newSampleName = "[" + patientID + "] " + lastName;
        int currentNumberOfExtractors = extractorListScrollContent.childCount;
        string numberForNewExtractor = (currentNumberOfExtractors + 1).ToString();
        bool allExtractorsBusy = true;
        foreach (GameObject extractor in activeExtractors) {
            allExtractorsBusy = extractor.transform.GetChild(3).GetComponent<Toggle>().isOn;
            if (allExtractorsBusy == false) {
                availableExtractor = extractor;
                break;
            }
        }
        if (currentNumberOfExtractors == 0 || allExtractorsBusy) {
            GameObject extractorInstance = Instantiate(extractorPrefab) as GameObject;
            extractorInstance.transform.SetParent(extractorListScrollContent.transform, false);
            extractorInstance.GetComponent<Device>().SetDeviceTitle("Extractor", numberForNewExtractor);
            activeExtractors.Add(extractorInstance);
            extractorInstance.GetComponent<Device>().SetFirstInitPanel(newSampleName);
        }
        else if (!allExtractorsBusy) {
            availableExtractor.GetComponent<Device>().SetFirstInitPanel(newSampleName);
        }
        //DONE 1. get patient id and last name from profile page
        //DONE 2. instantiate extractorPrefab as child of extractorListScrollContent if (zero extractors in scrollcontent || all of them are in middle of tests)
        //DONE 3. set initExtPanel active for available extractor (just instantiated or previously done so) with sampleName
        //DONE 4a. Cancel: sets initExtPanel inactive
        //DONE 4b. Confirm:
        //DONE      1. instantiate patientRepPrefab as child of extractorSamplesScrollContent (using the id and last name)
        //DONE      2. sets initExtPanel inactive
        //*      ** can go back to main page to start test for other profiles or click patientRep on the samples list to remove it       
    }

    public void PrepCentrifuge() {// Place this function in the Update() function: use an if statement to check whether there are any samples in finishedSamples        
        int currentNumberOfCentrifuges = centriListScrollContent.childCount;
        string numberForNewCentrifuge = (currentNumberOfCentrifuges + 1).ToString();
        bool allCentrifugesBusy = true;
        foreach (GameObject centrifuge in activeCentrifuges) {
            allCentrifugesBusy = centrifuge.transform.GetChild(3).GetComponent<Toggle>().isOn;
            if (allCentrifugesBusy == false) {
                availableCentrifuge = centrifuge;
                break;
            }
        }
        if (currentNumberOfCentrifuges == 0 || allCentrifugesBusy) {
            GameObject centrifugeInstance = Instantiate(centrifugePrefab) as GameObject;
            centrifugeInstance.transform.SetParent(centriListScrollContent.transform, false);
            centrifugeInstance.GetComponent<Device>().SetDeviceTitle("Centrifuge", numberForNewCentrifuge);
            activeCentrifuges.Add(centrifugeInstance);
            centrifugeInstance.GetComponent<Device>().SetLaterInitPanel(finishedExtraction);
        }
        else if (!allCentrifugesBusy && !availableCentrifuge.GetComponent<Device>().deviceStandby) {
            availableCentrifuge.GetComponent<Device>().SetLaterInitPanel(finishedExtraction);
        }
        // 9. Update(): if (extractionFinished is not empty && (zero centrifuges in scrollcontent || all of them are in middle of tests))
        //      1. instantiate centriPrefab as child of centriListScrollContent
        //      2. set initCentPanel active for available centrifuge (go by index: first sample(List) goes to first device(List));
        // 11. "Select the samples to allocate to this device" (checkboxes)
        //      1. fill proposedSamples scrollable list with samples from extractionFinished
        // 12. Start Test:
        //      1. instantiate patientRepPrefab as child of extractorSamplesScrollContent (using the id and last name)
        //      2. sets initCentPanel inactive
        //      3. start reading from potentiometer
        // 13. adjust progress circle based on potentiometer reading
        // 13b. Abort:resets progress circle back to standby
        // 14. if (progress = 100%) --> 
    }
    public void PrepThermoCycler() {
        int currentNumberOfThermos = thermoListScrollContent.childCount;
        string numberForNewThermo = (currentNumberOfThermos + 1).ToString();
        bool allThermosBusy = true;
        foreach (GameObject thermo in activeThermos) {
            allThermosBusy = thermo.transform.GetChild(3).GetComponent<Toggle>().isOn;
            if (allThermosBusy == false) {
                availableThermo = thermo;
                break;
            }
        }
        if (currentNumberOfThermos == 0 || allThermosBusy) {
            GameObject thermoInstance = Instantiate(thermoPrefab) as GameObject;
            thermoInstance.transform.SetParent(thermoListScrollContent.transform, false);
            thermoInstance.GetComponent<Device>().SetDeviceTitle("Thermocyler", numberForNewThermo);
            activeThermos.Add(thermoInstance);
            thermoInstance.GetComponent<Device>().SetLaterInitPanel(finishedCentrifugation);
        }
        else if (!allThermosBusy && !availableThermo.GetComponent<Device>().deviceStandby) {
            availableThermo.GetComponent<Device>().SetLaterInitPanel(finishedCentrifugation);
        }
        // 15. Do same for thermocycler and imager
    }
    public void PrepImager() {
        int currentNumberOfImagers = imagerListScrollContent.childCount;
        string numberForNewImager = (currentNumberOfImagers + 1).ToString();
        bool allImagersBusy = true;
        foreach (GameObject imager in activeImagers) {
            allImagersBusy = imager.transform.GetChild(3).GetComponent<Toggle>().isOn;
            if (allImagersBusy == false) {
                availableImager = imager;
                break;
            }
        }
        if (currentNumberOfImagers == 0 || allImagersBusy) {
            GameObject imagerInstance = Instantiate(imagerPrefab) as GameObject;
            imagerInstance.transform.SetParent(imagerListScrollContent.transform, false);
            imagerInstance.GetComponent<Device>().SetDeviceTitle("Imager", numberForNewImager);
            activeImagers.Add(imagerInstance);
            imagerInstance.GetComponent<Device>().SetLaterInitPanel(finishedPCR);
        }
        else if (!allImagersBusy && !availableImager.GetComponent<Device>().deviceStandby) {
            availableImager.GetComponent<Device>().SetLaterInitPanel(finishedPCR);
        }
        // 16. After 100% for imager -->
        // 17. get hiv load reading for corresponding samples
        // 18. INSERT hiv load where patient_id = xxx for each sample in imager
        // 19. "Test completed for these patients:" (at top of the imager panel)
        // 20. OK: Destroy all children in imagerSamplesScrollContent
    }
}
