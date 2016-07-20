using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PatientProfileDataScript : MonoBehaviour {

	public GameObject full_name_textbox;

	public void SetInfo() {
		this.full_name_textbox.GetComponent<Text>().text = "hello";
		Debug.Log ("Setting the full name");
	}

}
