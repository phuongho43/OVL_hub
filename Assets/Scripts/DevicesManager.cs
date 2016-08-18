using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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


    public void PrepFirstDevice() {
        string patientID = idTextbox.GetComponent<Text>().text.Trim();
        string lastName = lastnameTextbox.GetComponent<Text>().text.Trim();
        string newSampleName = "[" + patientID + "] " + lastName;
        int currentNumberOfExtractors = extractorListScrollContent.childCount;
        string numberForNewExtractor = (currentNumberOfExtractors + 1).ToString();
        bool allExtractorsBusy = true;
        foreach (GameObject extractor in activeExtractors) {
            allExtractorsBusy = extractor.transform.GetChild(2).GetComponent<Toggle>().isOn;
            if (allExtractorsBusy == false) {
                availableExtractor = extractor;
                break;
            }
        }
        if (currentNumberOfExtractors == 0 || allExtractorsBusy) {
            GameObject extractorInstance = Instantiate(extractorPrefab) as GameObject;
            extractorInstance.transform.SetParent(extractorListScrollContent.transform, false);
            extractorInstance.GetComponent<FirstDevice>().SetDeviceTitle("Extractor", numberForNewExtractor);
            activeExtractors.Add(extractorInstance);
            extractorInstance.GetComponent<FirstDevice>().SetInitPanel(newSampleName);
        }
        else if (!allExtractorsBusy) {
            availableExtractor.GetComponent<FirstDevice>().SetInitPanel(newSampleName);
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

    public void PrepMiddleDevice(string deviceType) {// Place this function in the Update() function: use an if statement to check whether there are any samples in finishedSamples        
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
        // 15. Do same for thermocycler and imager
        // 16. After 100% for imager -->
        // 17. get hiv load reading for corresponding samples
        // 18. INSERT hiv load where patient_id = xxx for each sample in imager
        // 19. "Test completed for these patients:" (at top of the imager panel)
        // 20. OK: Destroy all children in imagerSamplesScrollContent
    }
}
