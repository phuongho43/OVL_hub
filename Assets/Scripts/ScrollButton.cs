using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ScrollButton : MonoBehaviour {

    private float calcInterval() {
        int numChildren = transform.childCount;
        float interval = 2;
        if (numChildren < 3) {
            interval = 0;
        }
        else if (numChildren > 3) {
            int n = numChildren - 3;
            for (int i = 1; i <= n; i++) {
                int denom = i * (i + 1) / 2;
                interval -= (1.0f) / denom;
            }
        }
        return interval;
    }
        
    private float minScrollDown() {
        int numChildren = transform.childCount;
        int occurrence = (int)Math.Ceiling(numChildren / 2.0f) - 1;
        return 1 - (occurrence*calcInterval() - 0.0000001f);
    }

    public void ScrollUp() {
        if (transform.parent.parent.GetComponentInParent<ScrollRect>().verticalNormalizedPosition < 0.99) {
            transform.parent.parent.GetComponentInParent<ScrollRect>().verticalNormalizedPosition += (calcInterval());
            Debug.Log(transform.parent.parent.GetComponentInParent<ScrollRect>().verticalNormalizedPosition);
        }
    }

    public void ScrollDown() {
        Debug.Log(minScrollDown());
        if (transform.parent.parent.GetComponentInParent<ScrollRect>().verticalNormalizedPosition > minScrollDown()) {
            transform.parent.parent.GetComponentInParent<ScrollRect>().verticalNormalizedPosition -= (calcInterval());
            Debug.Log(transform.parent.parent.GetComponentInParent<ScrollRect>().verticalNormalizedPosition);
        }
    }
}
