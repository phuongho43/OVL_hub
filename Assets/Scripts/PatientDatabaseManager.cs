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
	public GameObject gender_m_input;
	public GameObject gender_f_input;
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

    // Retrieves id of latest patient: used for grabbing data for the profile page right after submitting the form
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

    // Changes text on patient profile according to data from GetPatientData
	public void SetPatientData() {
        string patient_id = MostRecentPatient();
		List<string> data = GetPatientData(patient_id);
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

    private bool ConsistsOfWhiteSpace(string s){
        foreach(char c in s){
            if(c != ' ') return false;
        }
        return true;
    }

    private void InsertPatientData(Dictionary<string, string> data, string tableName) {
        List<string> columns = new List<string>();
        List<string> values = new List<string>();
        // Exclude empty/whitespace strings where user didn't fill in the field
        foreach (KeyValuePair<string, string> pair in data) {
            if (ConsistsOfWhiteSpace(pair.Value)) {
                
            }
            else {
                columns.Add(pair.Key);
                values.Add(pair.Value);
            }
        }
		connectionString = "URI=file:" + Application.dataPath + "/hubDB.db";
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
                string sqlQuery = "INSERT INTO " + tableName + " " + "(" + columns[0];
                for (int i = 1; i < columns.Count; i++) {
                    sqlQuery += ", " + columns[i];
                }
                sqlQuery += ") VALUES (" + "'" + values[0] + "'";
                for(int i = 1; i < values.Count; i++) {
                    sqlQuery += ", " + "'" + values[i] + "'";
                }
                sqlQuery += ")";
                Debug.Log(sqlQuery);
				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}
	}

	public void InsertPatientData() {
		Dictionary<string, string> data = new Dictionary<string, string>();
		data.Add("first_name", this.firstname_input.GetComponent<Text>().text);
		data.Add("last_name", this.lastname_input.GetComponent<Text>().text);
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
		InsertPatientData(data, "patients");
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
