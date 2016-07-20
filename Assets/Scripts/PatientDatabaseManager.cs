using UnityEngine;
using System;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;


public class PatientDatabaseManager : MonoBehaviour {

	private string connectionString;
	public GameObject full_name_textbox;
	//private List<PatientProfile> patientProfile = new List<PatientProfile>();

	// Use this for initialization
	void Start () {
		//connectionString = "URI=file:" + Application.dataPath + "/hubDB.db";
		//InsertPatientData("Charlie", "Mander");
		//DeletePatient(7);
		//GetPatientData(2);
	}

	// Update is called once per frame
	void Update () {

	}

	public void GetPatientData (int patient_id) {
		//patientProfile.Clear();
		connectionString = "URI=file:" + Application.dataPath + "/hubDB.db";
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
				string sqlQuery = String.Format("SELECT first_name, last_name from patients where patient_id = {0}", patient_id);
				dbCmd.CommandText = sqlQuery;
				using (IDataReader reader = dbCmd.ExecuteReader()) {
					while (reader.Read()) {
						string first_name = reader.GetString(0);
						string last_name = reader.GetString(1);
						string full_name = first_name + " " + last_name;
						Debug.Log("GetPatientData is called");
						this.full_name_textbox.GetComponent<Text> ().text = full_name;
						//patientProfile.Add(new PatientProfile(first_name));
					}
					dbConnection.Close();
					reader.Close();
				}
			}
		}
	}

	private void InsertPatientData (string first_name, string last_name) {
		connectionString = "URI=file:" + Application.dataPath + "/hubDB.db";
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
				string sqlQuery = String.Format("INSERT INTO patients(first_name,last_name) VALUES('{0}', '{1}')", first_name, last_name);
				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}
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
