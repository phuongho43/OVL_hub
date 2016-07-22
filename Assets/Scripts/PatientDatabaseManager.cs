using UnityEngine;
using System;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using System.Collections.Generic;


public class PatientDatabaseManager : MonoBehaviour {

	private string connectionString;

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
	public GameObject gender_input;
    public Text birthdate_input;
    public Text weight_input;
    public Text height_input;
    public Text phone_input;
    public Text latest_visit_doc_input;
    public Text medications_input;
    public Text conditions_input;

	// Use this for initialization
	void Start () {
        
	}

	// Update is called once per frame
	void Update () {

	}

	public string MostRecentPatient() {
        string last_patient_id = "";
		connectionString = "URI=file:" + Application.dataPath + "/hubDB.db";
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
				string sqlQuery = String.Format("SELECT max(patient_id) from patients");
				dbCmd.CommandText = sqlQuery;
				using (IDataReader reader = dbCmd.ExecuteReader()) {
					while (reader.Read()) {
                        last_patient_id = reader.GetInt64(0).ToString();
						Debug.Log("MostRecentPatient was called: " + last_patient_id);
					}
					dbConnection.Close();
					reader.Close();
				}
			}
		}
		return last_patient_id;
	}

	private List<string> GetPatientData(string patient_id) {
		List<string> patient_data = new List<string>();
		connectionString = "URI=file:" + Application.dataPath + "/hubDB.db";
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
				string sqlQuery = String.Format("SELECT * from patients where patient_id = {0}", patient_id);
				dbCmd.CommandText = sqlQuery;
				using (IDataReader reader = dbCmd.ExecuteReader()) {
					while (reader.Read()) {
						for (int i = 0; i < reader.FieldCount; i++) {
							patient_data.Add(reader.GetValue(i).ToString());
						}
						Debug.Log("GetPatientData was called: " + patient_data[1]);
					}
					dbConnection.Close();
					reader.Close();
				}
			}
		}
		return patient_data;
	}

	public void SetPatientData() {
        string patient_id = MostRecentPatient();
		List<string> data = GetPatientData(patient_id);
		Debug.Log("SetPatientData was called with patient_id: " + patient_id);
		this.id_textbox.GetComponent<Text>().text = "ID: " + data[0];
		this.fullname_textbox.GetComponent<Text>().text = data[1] + " " + data[2];
		this.gender_textbox.GetComponent<Text>().text = "Gender: " + data[3];
		this.birthdate_textbox.GetComponent<Text>().text = "DoB: " + data[4];
		this.weight_textbox.GetComponent<Text>().text = "Weight: " + data[5] + " Kg";
		this.height_textbox.GetComponent<Text>().text = "Height: " + data[6] + " m";
		this.phone_textbox.GetComponent<Text>().text = "Phone: " + data[7];
		this.appointment_textbox.GetComponent<Text>().text = "Appt: " + data[8];
		this.latest_visit_date_textbox.GetComponent<Text>().text = data[9];
		this.latest_visit_doc_textbox.GetComponent<Text>().text = data[10];
	}

    private void InsertPatientData(string first_name, string last_name, string gender,
        string birthdate, string weight, string height, string phone, string latest_visit_doc,
		string medications, string conditions) {
		connectionString = "URI=file:" + Application.dataPath + "/hubDB.db";
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
				string sqlQuery = String.Format(@"INSERT INTO patients(first_name,
                last_name,gender,date_of_birth,weight,height,phone_number,
				latest_visit_doctor,medications,medical_conditions) 
                VALUES('{0}','{1}','{2}','{3}',{4}, {5},'{6}','{7}','{8}','{9}')",
                first_name, last_name, gender, birthdate, weight, height, phone, latest_visit_doc, medications, conditions);
				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}
	}

    public void InsertPatientData() {
		string first_name = this.firstname_input.GetComponent<Text>().text;
		string last_name = this.lastname_input.GetComponent<Text>().text;
		bool genderbool = this.gender_input.GetComponent<Toggle>().isOn;
		string gender;
		if (genderbool == true) {
			gender = "M";
		} else {
			gender = "F";
		}
		string birth_date = this.birthdate_input.GetComponent<Text>().text;
		string weight = this.weight_input.GetComponent<Text>().text;
		string height = this.height_input.GetComponent<Text>().text;
		string phone = this.phone_input.GetComponent<Text>().text;
		string latest_visit_doc = this.latest_visit_doc_input.GetComponent<Text>().text;
		string medications = this.medications_input.GetComponent<Text>().text;
		string conditions = this.conditions_input.GetComponent<Text>().text;

        InsertPatientData(first_name,last_name,gender,birth_date,weight,height,phone,latest_visit_doc,medications,conditions);
    }

	private void DeletePatientData (int patient_id) {
		connectionString = "URI=file:" + Application.dataPath + "/hubDB.db";
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
				string sqlQuery = String.Format("DELETE FROM patients WHERE patient_id = {0}", patient_id);
				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}
	}

}
