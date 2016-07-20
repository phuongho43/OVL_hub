using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading;


public class LoadingBar : MonoBehaviour {

    public Transform Progress;
    int current;
    void setup()
    {
        current = 0;
        Progress.GetComponent<Image>().fillAmount = 0;
    }
	// Update is called once per frame
	void Update () {
        Progress.GetComponent<Image>().fillAmount = current / 100;
        current++;
	}
}
