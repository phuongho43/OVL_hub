using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.UI;


public class PatientDatabaseManager {

	private string connectionString;

    public bool ConsistsOfWhiteSpace(string s){
        foreach(char c in s){
            if(c != ' ') return false;
        }
        return true;
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
						Debug.Log("MostRecentPatient: " + last_patient_id);
					}
					dbConnection.Close();
					reader.Close();
				}
			}
		}
		return last_patient_id;
	}

	public List<string> GetPatientData(string patient_id) {
        // Dictionary might be better if order of DB columns get changed
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
						Debug.Log("GetPatientData: " + patient_data[1]);
					}
					dbConnection.Close();
					reader.Close();
				}
			}
		}
		return patient_data;
	}

    public void InsertPatientData(string tableName, Dictionary<string, string> data) {
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
                string sqlQuery = "INSERT INTO " + tableName + " (" + columns[0];
                for (int i = 1; i < columns.Count; i++) {
                    sqlQuery += ", " + columns[i];
                }
                sqlQuery += ") VALUES (" + "'" + values[0] + "'";
                for(int i = 1; i < values.Count; i++) {
                    sqlQuery += ", " + "'" + values[i] + "'";
                }
                sqlQuery += ")";
                Debug.Log("InsertPatientData: " + sqlQuery);
				dbCmd.CommandText = sqlQuery;
				dbCmd.ExecuteScalar();
				dbConnection.Close();
			}
		}
	}

    public void UpdatePatientData(string tableName, Dictionary<string, string> data, string patient_id) {
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
                string sqlQuery = "UPDATE " + tableName + " SET " + columns[0] + "='" + values[0] + "'";
                for (int i = 1; i < columns.Count; i++) {
                    sqlQuery += ", " + columns[i] + "='" + values[i] + "'";
                }
                sqlQuery += " WHERE patient_id='" + patient_id + "'";
                Debug.Log("UpdatePatientData: " + sqlQuery);
                dbCmd.CommandText = sqlQuery;
                dbCmd.ExecuteScalar();
                dbConnection.Close();
            }
        }
    }
        
	public void DeletePatientData (int patient_id) {
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
