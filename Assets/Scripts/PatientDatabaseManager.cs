﻿using UnityEngine;
using System;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;
using System.Collections.Generic;


public class PatientDatabaseManager : MonoBehaviour {

	private string connectionString;
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

    public Text firstname_input;
    public Text lastname_input;
    public Text gender_input;
    public Text birthdate_input;
    public Text weight_input;
    public Text height_input;
    public Text phone_input;
    public Text appointment_input;
    public Text latest_visit_date_input;
    public Text latest_visit_doc_input;
    public Text medications_input;
    public Text conditions_input;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	}

	private List<string> GetPatientData (int patient_id) {
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

	public void SetPatientData(int patient_id) {
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

    private void InsertPatientData (string first_name, string last_name, string gender,
        string birthdate, string weight, string height, string phone, string appointment,
        string latest_visit_date, string latest_visit_doc, string medications, string conditions) {
		connectionString = "URI=file:" + Application.dataPath + "/hubDB.db";
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
				string sqlQuery = String.Format(@"INSERT INTO patients(first_name,
                last_name,gender,date_of_birth,weight,height,phone_number,appointments,
                latest_visit_date,latest_visit_doctor,medications,medical_conditions) 
                VALUES('{0}','{1}','{2}','{3}',{4}, {5},'{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
                first_name, last_name, gender, birthdate, weight, height, phone, appointment,
                latest_visit_date, latest_visit_doc, medications, conditions);
				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}
	}

    public void InsertPatientData() {
        string first_name = this.firstname_input.GetComponent<InputField>().text;
        string last_name = this.lastname_input.GetComponent<InputField>().text;
        string gender = this.gender_input.GetComponent<InputField>().text;
        string birth_date = this.birthdate_input.GetComponent<InputField>().text;
        string weight = this.weight_input.GetComponent<InputField>().text;
        string height = this.height_input.GetComponent<InputField>().text;
        string phone = this.phone_input.GetComponent<InputField>().text;
        string appointment = this.appointment_input.GetComponent<InputField>().text;
        string latest_visit_date = this.latest_visit_date_input.GetComponent<InputField>().text;
        string latest_visit_doc = this.latest_visit_doc_input.GetComponent<InputField>().text;
        string medications = this.medications_input.GetComponent<InputField>().text;
        string conditions = this.conditions_input.GetComponent<InputField>().text;
        InsertPatientData(first_name,last_name,gender,birth_date,weight,height,phone,appointment,latest_visit_date,latest_visit_doc,medications,conditions);
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
