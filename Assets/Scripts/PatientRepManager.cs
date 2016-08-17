using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PatientRepManager : MonoBehaviour {

    // Attach this script to the PatientRep gameobject

    private GameObject marker;
    public int maxMarkerImageNumber;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
        
    public void NextImage() {
        marker = transform.GetChild(0).gameObject;
        Image markerImage = marker.GetComponent<Image>();
        int currentImageName = Int32.Parse(markerImage.sprite.name);
        if (currentImageName < maxMarkerImageNumber) {
            string nextImageName = (currentImageName + 1).ToString();
            markerImage.sprite = Resources.Load<Sprite>("Markers/" + nextImageName);
        }
        else if (currentImageName >= maxMarkerImageNumber) {
            markerImage.sprite = Resources.Load<Sprite>("Markers/0");
        }
    }

    public void SetName(string id, string lastname) {
        GameObject textbox = transform.GetChild(1).gameObject;
        textbox.GetComponent<Text>().text = "[" + id + "] " + lastname;
    }

    public void DeletePatientRep() {
        GameObject patientRep = transform.gameObject;
        GameObject.Destroy(patientRep);
    }
}
