using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public Text firstname_input;
    public Text lastname_input;
    public GameObject gender_m_input;
    public GameObject gender_f_input;
    public Text birthdate_input;
    public Text weight_input;
    public Text height_input;
    public Text phone_input;
    public Text latest_visit_doc_input;
    public Text medications_input;
    public Text conditions_input;

    public PatientDatabaseManager manager = new PatientDatabaseManager();

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Changes text on patient profile according to data from GetPatientData
    public void SetPatientData() {
        string patient_id = manager.MostRecentPatient();
        List<string> data = manager.GetPatientData(patient_id);
        Debug.Log("SetPatientData was called with patient_id: " + patient_id);
        this.id_textbox.GetComponent<Text>().text = "ID: " + data[0];
        this.fullname_textbox.GetComponent<Text>().text = data[1] + " " + data[2];
        this.gender_textbox.GetComponent<Text>().text = "Gender: " + data[3];
        this.birthdate_textbox.GetComponent<Text>().text = "DoB: " + data[4];
        this.weight_textbox.GetComponent<Text>().text = "Weight(Kg): " + data[5];
        this.height_textbox.GetComponent<Text>().text = "Height(m): " + data[6];
        this.phone_textbox.GetComponent<Text>().text = "Phone: " + data[7];
        this.appointment_textbox.GetComponent<Text>().text = "Appt: " + data[8];
        this.latest_visit_date_textbox.GetComponent<Text>().text = data[9];
        this.latest_visit_doc_textbox.GetComponent<Text>().text = data[10];
    }

    public void AddPatientData() {
        Dictionary<string, string> data = new Dictionary<string, string>();
        string firstname = this.firstname_input.GetComponent<Text>().text;
        string lastname = this.lastname_input.GetComponent<Text>().text;
        if (manager.ConsistsOfWhiteSpace(firstname)) {
            firstname = "firstname";
        }
        if (manager.ConsistsOfWhiteSpace(lastname)) {
            lastname = "lastname";
        }
        data.Add("first_name", firstname);
        data.Add("last_name", lastname);
        bool malebool = this.gender_m_input.GetComponent<Toggle>().isOn;
        bool femalebool = this.gender_f_input.GetComponent<Toggle>().isOn;
        string gender;
        if (malebool == true) {
            gender = "M";
        } else if (femalebool == true) {
            gender = "F";
        } else {
            gender = "";
        }
        data.Add("gender", gender);
        data.Add("date_of_birth", this.birthdate_input.GetComponent<Text>().text);
        data.Add("weight", this.weight_input.GetComponent<Text>().text);
        data.Add("height", this.height_input.GetComponent<Text>().text);
        data.Add("phone_number", this.phone_input.GetComponent<Text>().text);
        data.Add("latest_visit_doctor", this.latest_visit_doc_input.GetComponent<Text>().text);
        data.Add("medications", this.medications_input.GetComponent<Text>().text);
        data.Add("medical_conditions", this.conditions_input.GetComponent<Text>().text);
        manager.InsertPatientData(data, "patients");
    }
}
