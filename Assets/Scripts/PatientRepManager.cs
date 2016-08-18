using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class PatientRepManager : MonoBehaviour {

    // Attach this script to the PatientRep gameobject

    public GameObject marker;
    public GameObject sampleNameTextBox;
    public string sampleName;
    public int maxMarkerImageNumber;

        
    public void NextImage() {
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

    public void SetName(string name) {
        sampleNameTextBox.GetComponent<Text>().text = name;
        sampleName = name;
    }

    public void DeletePatientRep() {
        GameObject.Destroy(transform.gameObject);
    }
}
