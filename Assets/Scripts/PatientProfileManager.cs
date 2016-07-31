using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using MaterialUI;
using UnityEngine.EventSystems;

public class PatientProfileManager : MonoBehaviour {

    //Patient Profile outputs
    public Text id_textbox;
    public Text fullname_textbox;
    public Text gender_textbox;
    public Text birthdate_textbox;
    public Text weight_textbox;
    public Text height_textbox;
    public Text phone_textbox;
    public Text appointment_textbox;
    public Text latest_visit_date_textbox;
    public Text latest_visit_doc_textbox;

    //Patient Profile inputs
    public GameObject firstname_input;
    public GameObject lastname_input;
    public GameObject gender_input;
    public GameObject birthdate_input;
    public GameObject weight_input;
    public GameObject height_input;
    public GameObject phone_input;
    public GameObject latest_visit_doc_input;
    public GameObject medications_input;
    public GameObject conditions_input;

    public PatientDatabaseManager manager = new PatientDatabaseManager();


	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    // Empties the textboxes after inserting the data; Having issues with MaterialUI's text inputs...    
    public void ClearForm() {
        firstname_input.GetComponent<InputField>().text = string.Empty;
        lastname_input.GetComponent<InputField>().text = string.Empty;
        gender_input.GetComponent<InputField>().text = string.Empty;
        birthdate_input.GetComponent<InputField>().text = string.Empty;
        weight_input.GetComponent<InputField>().text = string.Empty;
        height_input.GetComponent<InputField>().text = string.Empty;
        phone_input.GetComponent<InputField>().text = string.Empty;
        latest_visit_doc_input.GetComponent<InputField>().text = string.Empty;
        medications_input.GetComponent<InputField>().text = string.Empty;
        conditions_input.GetComponent<InputField>().text = string.Empty;
        Debug.Log("ClearForm: " + firstname_input.GetComponent<InputField>().text);
    }

    // Changes text on patient profile according to data from GetPatientData
    public void SetProfileData(string patient_id) {
        if (patient_id == "new") {
            patient_id = manager.MostRecentPatient();
        }
        List<string> data = manager.GetPatientData(patient_id);
        Debug.Log("SetProfileData: " + patient_id);
        // Targetting very specific objects on the profile page
        this.id_textbox.GetComponent<Text>().text = data[0];
        this.fullname_textbox.GetComponent<Text>().text = data[1] + " " + data[2];
        this.gender_textbox.GetComponent<Text>().text = "<b>Gender</b>: " + data[3];
        this.birthdate_textbox.GetComponent<Text>().text = "<b>DoB</b>: " + data[4];
        this.weight_textbox.GetComponent<Text>().text = "<b>Weight(Kg)</b>: " + data[5];
        this.height_textbox.GetComponent<Text>().text = "<b>Height(m)</b>: " + data[6];
        this.phone_textbox.GetComponent<Text>().text = "<b>Phone</b>: " + data[7];
        this.appointment_textbox.GetComponent<Text>().text = "<b>Appt</b>: " + data[8];
        this.latest_visit_date_textbox.GetComponent<Text>().text = data[9];
        this.latest_visit_doc_textbox.GetComponent<Text>().text = data[10];
    }

    public void AddPatientData() {
        Dictionary<string, string> data = new Dictionary<string, string>();
        // Targetting very specific input field objects on the form page
        data.Add("first_name", this.firstname_input.GetComponent<InputField>().text);
        data.Add("last_name", this.lastname_input.GetComponent<InputField>().text);
        data.Add("gender", this.gender_input.GetComponent<InputField>().text);
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
        data.Add("date_of_birth", this.birthdate_input.GetComponent<InputField>().text);
        data.Add("weight", this.weight_input.GetComponent<InputField>().text);
        data.Add("height", this.height_input.GetComponent<InputField>().text);
        data.Add("phone_number", this.phone_input.GetComponent<InputField>().text);
        data.Add("latest_visit_doctor", this.latest_visit_doc_input.GetComponent<InputField>().text);
        data.Add("medications", this.medications_input.GetComponent<InputField>().text);
        data.Add("medical_conditions", this.conditions_input.GetComponent<InputField>().text);
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
}
