using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MaterialUI;

public class PatientProfileManager : MonoBehaviour {

    // Patient Profile Outputs
    public Text id_textbox;
    public Text firstname_textbox;
    public Text lastname_textbox;
    public Text gender_textbox;
    public Text birthdate_textbox;
    public Text weight_textbox;
    public Text height_textbox;
    public Text phone_textbox;
    public Text appointment_textbox;
    public Text latest_visit_date_textbox;
    public Text latest_visit_doc_textbox;
    public Transform meds_ScrollContent;
    public GameObject meds_ItemPrefab;
    public Transform conds_ScrollContent;
    public GameObject conds_ItemPrefab;

    // Main Page Search for Patients
    public GameObject mp_searchSelector;
    public GameObject mp_searchInputfield;
    public Transform results_ScrollContent;
    public GameObject results_ItemPrefab;

    // Patient Profile Inputs
    private string firstname_input;
    private string lastname_input;
    private string gender_input;
    private string birthdate_input;
    private string weight_input;
    private string height_input;
    private string phone_input;
    private string latest_visit_doc_input;
    private string medications_input;
    private string conditions_input;

    // Passing List of Params to Dialog for Updating profiles
    private string updateColumn;

	// Reference to DB dbManager script to use methods
    public PatientDatabaseManager dbManager = new PatientDatabaseManager();

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
//        if (EventSystem.current.currentSelectedGameObject.tag == "SearchPatients" && Input.GetKeyDown(KeyCode.Return)) {
//            RefreshSearchResultsPanel();
//        }
	}
    // Empties the textboxes after inserting the data; Having issues with MaterialUI's text inputs...    
//    public void ClearForm() {
//        firstname_input.GetComponent<InputField>().text = string.Empty;
//        lastname_input.GetComponent<InputField>().text = string.Empty;
//        gender_input.GetComponent<InputField>().text = string.Empty;
//        birthdate_input.GetComponent<InputField>().text = string.Empty;
//        weight_input.GetComponent<InputField>().text = string.Empty;
//        height_input.GetComponent<InputField>().text = string.Empty;
//        phone_input.GetComponent<InputField>().text = string.Empty;
//        latest_visit_doc_input.GetComponent<InputField>().text = string.Empty;
//        medications_input.GetComponent<InputField>().text = string.Empty;
//        conditions_input.GetComponent<InputField>().text = string.Empty;
//        Debug.Log("ClearForm: " + firstname_input.GetComponent<InputField>().text);
//    }

    // Changes text on patient profile according to data from GetPatientData
    public void SetProfileData(string patient_id) {
        if (patient_id == "new") {
            patient_id = dbManager.MostRecentPatient();
        }
        string cond = "patient_id = " + "'" + patient_id + "'";
        List<string> data = dbManager.GetPatientData("*", "patients", cond);
        //Debug.Log("SetProfileData: " + patient_id);
        // Targetting very specific objects on the profile page
        id_textbox.GetComponent<Text>().text = data[0];
        firstname_textbox.GetComponent<Text>().text = data[1];
        lastname_textbox.GetComponent<Text>().text = data[2];
        gender_textbox.GetComponent<Text>().text = data[3];
        birthdate_textbox.GetComponent<Text>().text = data[4];
        weight_textbox.GetComponent<Text>().text = data[5];
        height_textbox.GetComponent<Text>().text = data[6];
        phone_textbox.GetComponent<Text>().text = data[7];
        appointment_textbox.GetComponent<Text>().text = data[8];
        latest_visit_date_textbox.GetComponent<Text>().text = data[9];
        latest_visit_doc_textbox.GetComponent<Text>().text = data[10];
        char[] delimiters = new char[] { ',' };
        List<string> listOfMeds = data[11].Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
        List<string> listOfConds = data[12].Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
        foreach (Transform child in meds_ScrollContent.transform) {
            GameObject.Destroy(child.gameObject);
        }
        foreach (Transform child in conds_ScrollContent.transform) {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < listOfMeds.Count; i++) {
            GameObject medItem = Instantiate(meds_ItemPrefab) as GameObject;
            medItem.GetComponentInChildren<Text>().text = listOfMeds[i];
            medItem.transform.SetParent(meds_ScrollContent.transform, false);
        }
        for (int i = 0; i < listOfConds.Count; i++) {
            GameObject condItem = Instantiate(conds_ItemPrefab) as GameObject;
            condItem.GetComponentInChildren<Text>().text = listOfConds[i];
            condItem.transform.SetParent(conds_ScrollContent.transform, false);
        }
    }

    public void GetAndAddFormInput() {
        Dictionary<string, string> data = new Dictionary<string, string>();
        // Targetting very specific input field objects on the form page
        // Maybe use FindGameObjectsWithTag instead to get a list
        firstname_input = GameObject.FindGameObjectWithTag("Firstname_Inputfield").GetComponent<InputField>().text.Trim();
        lastname_input = GameObject.FindGameObjectWithTag("Lastname_Inputfield").GetComponent<InputField>().text.Trim();
        gender_input = GameObject.FindGameObjectWithTag("Gender_Inputfield").GetComponent<InputField>().text.Trim();
        birthdate_input = GameObject.FindGameObjectWithTag("DoB_Inputfield").GetComponent<InputField>().text.Trim();
        weight_input = GameObject.FindGameObjectWithTag("Weight_Inputfield").GetComponent<InputField>().text.Trim();
        height_input = GameObject.FindGameObjectWithTag("Height_Inputfield").GetComponent<InputField>().text.Trim();
        phone_input = GameObject.FindGameObjectWithTag("Phone_Inputfield").GetComponent<InputField>().text.Trim();
        medications_input = GameObject.FindGameObjectWithTag("Meds_Inputfield").GetComponent<InputField>().text.Trim();
        conditions_input = GameObject.FindGameObjectWithTag("Conditions_Inputfield").GetComponent<InputField>().text.Trim();
        latest_visit_doc_input = GameObject.FindGameObjectWithTag("Doc_Inputfield").GetComponent<InputField>().text.Trim();
        data.Add("first_name", firstname_input);
        data.Add("last_name", lastname_input);
        data.Add("gender", gender_input);
		// If using radio buttons
//        bool malebool = this.gender_m_input.GetComponent<Toggle>().isOn;
//        bool femalebool = this.gender_f_input.GetComponent<Toggle>().isOn;
//        string gender;
//        if (malebool == true) {
//            gender = "M";
//        } else if (femalebool == true) {
//            gender = "F";
//        } else {
//            gender = "";
//        }
//        data.Add("gender", gender);
        data.Add("date_of_birth", birthdate_input);
        data.Add("weight", weight_input);
        data.Add("height", height_input);
        data.Add("phone_number", phone_input);
        data.Add("latest_visit_doctor", latest_visit_doc_input);
        data.Add("medications", medications_input);
        data.Add("medical_conditions", conditions_input);
        //Debug.Log("AddPatientData: " + data["first_name"]);
        // Addresses when the user enters nothing in the form
        bool goAhead = false;
        foreach (KeyValuePair<string, string> pair in data) {
            if (!dbManager.ConsistsOfWhiteSpace(pair.Value)) {
                goAhead = true;
            }
        }
        if (goAhead) {
            dbManager.InsertPatientData("patients", data);
            //Debug.Log("AddPatientData: at least one inputfield was filled out");
        }
    }

    // Retrieves the specific database column for GetAndAddDialogInput
    public void GetCorrespondingColumn() {
        // Get tag of game object that was just clicked on
        string objectTag = EventSystem.current.currentSelectedGameObject.tag;
		Dictionary<string, string> TagToColumnName = new Dictionary<string, string>()
		{
			{"Firstname_ProfileText","first_name"},
            {"Lastname_ProfileText","last_name"},
			{"Gender_ProfileLabel","gender"},
			{"DoB_ProfileLabel","date_of_birth"},
			{"Weight_ProfileLabel","weight"},
			{"Height_ProfileLabel","height"},
			{"Phone_ProfileLabel","phone_number"},
			{"Appt_ProfileLabel","appointment"},
            {"lvDate_ProfileText","latest_visit_date"},
            {"lvDoc_ProfileText","latest_visit_doctor"},
            {"EditMeds_Icon","medications"},
            {"EditConds_Icon","medical_conditions"},
		};
        //Debug.Log("GetCorrespondingColumn: " + TagToColumnName[objectTag]);
        updateColumn = TagToColumnName[objectTag];
	}

    // Used by the updating dialog box in the profile page for meds and conds panels
    public void preFillInputField() {
        string patient_id = id_textbox.GetComponent<Text>().text;
        string cond = "patient_id = " + "'" + patient_id + "'";
        string currentValue = dbManager.GetPatientData(updateColumn, "patients", cond)[0];
        GameObject.FindGameObjectWithTag("UpdateProfileDialog_Inputfield").GetComponent<InputField>().text = currentValue;
    }

    // Updating profile page info by clicking the specific fields and filling in new data through a dialog box
    public void GetAndAddDialogInput() {
        string patient_id = id_textbox.GetComponent<Text>().text;
        Dictionary<string,string> columnAndValue = new Dictionary<string,string>();
        string updateValue = GameObject.FindGameObjectWithTag("UpdateProfileDialog_Inputfield").GetComponent<InputField>().text.Trim();
        columnAndValue.Add(updateColumn, updateValue);
        dbManager.UpdatePatientData("patients", columnAndValue, patient_id);
        SetProfileData(patient_id);
        GameObject.FindGameObjectWithTag("UpdateProfileDialog_Inputfield").GetComponent<InputField>().text = string.Empty;
    }

    // Clears all item in the results panel on the main page
    public void ClearSearchResults() {
        foreach (Transform child in results_ScrollContent.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void SearchForPatients() {
        List<string> data = new List<string>();
        string cond;
        int selectedIndex = mp_searchSelector.GetComponent<SelectionBoxConfig>().currentSelection;
        string selectedItem = mp_searchSelector.GetComponent<SelectionBoxConfig>().listItems[selectedIndex];
        //Debug.Log(selectedItem);
        string searchWord = mp_searchInputfield.GetComponent<InputField>().text.Trim();
        mp_searchInputfield.GetComponent<InputField>().text = string.Empty;
        if (selectedItem == "ID") {
            cond = "patient_id = " + "'" + searchWord + "'";
            data = dbManager.GetPatientData("patient_id, first_name, last_name", "patients", cond);
        } else if (selectedItem == "Lastname") {
            cond = "last_name LIKE " + "'%" + searchWord + "%'";
            data = dbManager.GetPatientData("patient_id, first_name, last_name", "patients", cond);
        } else if (selectedItem == "Phone") {
            cond = "phone_number = " + "'" + searchWord + "'";
            data = dbManager.GetPatientData("patient_id, first_name, last_name", "patients", cond);
        }
        //Debug.Log(data[0]);
        ClearSearchResults();
        ScreenManager screenManager = GameObject.FindGameObjectWithTag("ScreenManager").GetComponent<ScreenManager>();
        GameObject startTestButton = GameObject.FindGameObjectWithTag("StartTest_Button").transform.GetChild(0).gameObject;
        GameObject deleteProfileButton = GameObject.FindGameObjectWithTag("DeleteProfile_Button").transform.GetChild(0).gameObject;
        //Debug.Log(deleteProfileButton);
        for (int i = 0; i < data.Count/3; i++) {
            GameObject instance = Instantiate(results_ItemPrefab) as GameObject;
            instance.GetComponentInChildren<Text>().text = "[" + data[i*3] + "]" + " " + data[i*3 + 1] + " " + data[i*3 + 2];
            instance.transform.SetParent(results_ScrollContent.transform, false);
            string current_id = data[i * 3];
            instance.GetComponent<Button>().onClick.AddListener(() => {
                SetProfileData(current_id);
                screenManager.Set("Dashboard");
                startTestButton.SetActive(true);
                deleteProfileButton.SetActive(true);
                ClearSearchResults();
            });
        }
    }

    public void DeleteThisPatient() {
        string patient_id = id_textbox.GetComponent<Text>().text;
        dbManager.DeletePatientData(patient_id);
        GameObject.FindGameObjectWithTag("DeleteProfileDialog_Bg").GetComponent<CanvasGroup>().alpha = 0;
    }
}
