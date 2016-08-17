﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tabBehaviour : MonoBehaviour
{
    // Tabbing between inputfields on the new patient form page
    // Attach this script to the form container

    private EventSystem system;

    private void Start()
    {
        system = EventSystem.current;
    }

    private void Update()
    {
        if (system.currentSelectedGameObject == null || !Input.GetKeyDown (KeyCode.Tab))
            return;

        Selectable current = system.currentSelectedGameObject.GetComponent<Selectable>();
        if (current == null)
            return;

        bool up = Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift);
        Selectable next = up ? current.FindSelectableOnUp() : current.FindSelectableOnDown();

        if (next == null)
        {
            next = current;

            Selectable pnext;
            if(up) while((pnext = next.FindSelectableOnDown()) != null) next = pnext;
            else while((pnext = next.FindSelectableOnUp()) != null) next = pnext;
        }
            
        InputField inputfield = next.GetComponent<InputField>();
        if (inputfield != null) inputfield.OnPointerClick(new PointerEventData(system));


        system.SetSelectedGameObject(next.gameObject);
    }
}
