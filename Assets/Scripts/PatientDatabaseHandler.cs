using UnityEngine;
using System;
using System.Collections;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;

public class PatientDatabaseHandler : MonoBehaviour {

	private string connectionString;
	
	// Use this for initialization
	void Start () {
		Debug.Log("Hello World");
		connectionString = "URI=file:" + Application.dataPath + "/hubDB.db";
		//InsertPatientData("Charlie", "Mander");
		//DeletePatient(7);
		GetPatientData();

	}

	// Update is called once per frame
	void Update () {

	}

	private void GetPatientData () {
		using (IDbConnection dbConnection = new SqliteConnection(connectionString)) {
			dbConnection.Open();
			using (IDbCommand dbCmd = dbConnection.CreateCommand()) {
				string sqlQuery = "SELECT first_name, last_name from patients";
				dbCmd.CommandText = sqlQuery;
				using (IDataReader reader = dbCmd.ExecuteReader()) {
					while (reader.Read()) {
						string first_name = reader.GetString(0);
						string last_name = reader.GetString(1);
						Debug.Log(first_name + " " + last_name);
					}
					dbConnection.Close();
					reader.Close();
				}
			}
		}
	}

	private void InsertPatientData (string first_name, string last_name) {
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

	private void DeletePatient (int patient_id) {
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
