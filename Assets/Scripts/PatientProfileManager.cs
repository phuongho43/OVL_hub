using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MaterialUI;

public class PatientProfileManager : MonoBehaviour {

    // Patient Profile outputs
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

    // Patient Profile inputs
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

	// Reference to DB manager script to use methods
    public PatientDatabaseManager manager = new PatientDatabaseManager();


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
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
            patient_id = manager.MostRecentPatient();
        }
        List<string> data = manager.GetPatientData(patient_id);
        Debug.Log("SetProfileData: " + patient_id);
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
        Debug.Log("AddPatientData: " + data["first_name"]);
        // Addresses when the user enters nothing in the form
        bool goAhead = false;
        foreach (KeyValuePair<string, string> pair in data) {
            if (!manager.ConsistsOfWhiteSpace(pair.Value)) {
                goAhead = true;
            }
        }
        if (goAhead) {
            manager.InsertPatientData("patients", data);
            Debug.Log("AddPatientData: at least one inputfield was filled out");
        }
    }

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
		};
        Debug.Log("GetCorrespondingColumn: " + TagToColumnName[objectTag]);
        updateColumn = TagToColumnName[objectTag];
	}

    public void GetAndAddDialogInput() {
        string patient_id = id_textbox.GetComponent<Text>().text;
        Dictionary<string,string> columnAndValue = new Dictionary<string,string>();
        string updateValue = GameObject.FindGameObjectWithTag("UpdateProfileDialog_Inputfield").GetComponent<InputField>().text.Trim();
        columnAndValue.Add(updateColumn, updateValue);
        manager.UpdatePatientData("patients", columnAndValue, patient_id);
        SetProfileData(patient_id);
        GameObject.FindGameObjectWithTag("UpdateProfileDialog_Inputfield").GetComponent<InputField>().text = string.Empty;
    }
}
