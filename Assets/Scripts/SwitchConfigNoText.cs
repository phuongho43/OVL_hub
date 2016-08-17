using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    [ExecuteInEditMode]
    public class SwitchConfigNoText : MonoBehaviour
    {
        // Variant of SwitchConfig.cs but without the text (just the switch)

        public float animationDuration = 0.5f;

        public Color onColor;
        public Color offColor;
        public Color disabledColor;

        public Color backOffColor;
        public Color backDisabledColor;

        public bool changeRippleColor;


        [SerializeField] private Image switchImage;
        [SerializeField] private Image backImage;

        private RectTransform switchRectTransform;
        private RippleConfig rippleConfig;
        private CheckBoxToggler checkBoxToggler = null;

        Toggle toggle;

        private bool lastToggleInteractableState;
        private bool lastToggleState;

        private float currentSwitchPosition;
        private Color currentColor;
        private Color currentBackColor;

        private int state;
        private float animStartTime;
        private float animDeltaTime;

        void OnEnable()
        {
            //  Set references
            toggle = gameObject.GetComponent<Toggle>();
            switchRectTransform = switchImage.GetComponent<RectTransform>();
            rippleConfig = gameObject.GetComponent<RippleConfig>();
        }

        void Start()
        {
            lastToggleInteractableState = toggle.interactable;

            if (lastToggleInteractableState)
            {
                if (lastToggleState != toggle.isOn)
                {
                    lastToggleState = toggle.isOn;

                    if (lastToggleState)
                        TurnOnSilent();
                    else
                        TurnOffSilent();
                }
            }

            if (changeRippleColor)
                rippleConfig.rippleColor = backImage.color;
        }

        public void ToggleSwitch ()
        {
            if (toggle.isOn)
                TurnOn ();
            else
                TurnOff ();
        }

        public void TurnOn()
        {
            currentSwitchPosition = switchRectTransform.anchoredPosition.x;
            currentColor = switchImage.color;
            currentBackColor = backImage.color;

            animStartTime = Time.realtimeSinceStartup;
            state = 1;
        }

        private void TurnOnSilent()
        {
            if (switchRectTransform.anchoredPosition != new Vector2(8f, 0f))
                switchRectTransform.anchoredPosition = new Vector2(8f, 0f);

            if (lastToggleInteractableState)
            {
                switchImage.color = onColor;
                backImage.color = onColor;

                if (changeRippleColor)
                    rippleConfig.rippleColor = onColor;
            }
        }

        public void TurnOff()
        {
            currentSwitchPosition = switchRectTransform.anchoredPosition.x;
            currentColor = switchImage.color;
            currentBackColor = backImage.color;

            animStartTime = Time.realtimeSinceStartup;
            state = 2;
        }

        private void TurnOffSilent()
        {
            backImage.enabled = true;
            if (switchRectTransform.anchoredPosition != new Vector2(-8f, 0f))
                switchRectTransform.anchoredPosition = new Vector2(-8f, 0f);

            if (lastToggleInteractableState)
            {
                switchImage.color = offColor;
                backImage.color = backOffColor;


                if (changeRippleColor)
                    rippleConfig.rippleColor = backOffColor;
            }
        }

        private void EnableSwitch()
        {
            if (toggle.isOn)
            {
                switchImage.color = onColor;
                backImage.color = onColor;

            }
            else
            {
                switchImage.color = offColor;
                backImage.color = backOffColor;               
            }

            checkBoxToggler.enabled = true;
            rippleConfig.enabled = true;
        }

        private void DisableSwitch()
        {
            switchImage.color = disabledColor;
            backImage.color = backDisabledColor;

            checkBoxToggler.enabled = false;
            rippleConfig.enabled = false;
        }

        void Update()
        {
            animDeltaTime = Time.realtimeSinceStartup - animStartTime;

            if (state == 1)
            {
                if (animDeltaTime <= animationDuration)
                {
                    switchRectTransform.anchoredPosition = Anim.Quint.SoftOut(new Vector2(currentSwitchPosition, 0f), new Vector2(8f, 0f), animDeltaTime, animationDuration);
                    switchImage.color = Anim.Quint.SoftOut(currentColor, onColor, animDeltaTime, animationDuration);
                    backImage.color = Anim.Quint.SoftOut(currentBackColor, onColor, animDeltaTime, animationDuration);


                    if (changeRippleColor)
                        rippleConfig.rippleColor = switchImage.color;
                }
                else
                {
                    switchRectTransform.anchoredPosition = new Vector2(8f, 0f);
                    switchImage.color = onColor;
                    backImage.color = onColor;


                    if (changeRippleColor)
                        rippleConfig.rippleColor = onColor;
                    state = 0;
                }
            }
            else if (state == 2)
            {
                if (animDeltaTime <= animationDuration * 0.75f)
                {
                    switchRectTransform.anchoredPosition = Anim.Quint.SoftOut(new Vector2(currentSwitchPosition, 0f), new Vector2(-8f, 0f), animDeltaTime, animationDuration);
                    switchImage.color = Anim.Sept.InOut(currentColor, offColor, animDeltaTime, animationDuration * 0.75f);
                    backImage.color = Anim.Sept.InOut(currentBackColor, backOffColor, animDeltaTime, animationDuration * 0.75f);
                    

                    if (changeRippleColor)
                        rippleConfig.rippleColor = switchImage.color;
                }
                else
                {
                    switchRectTransform.anchoredPosition = new Vector2(-8f, 0f);

                    switchImage.color = offColor;
                    backImage.color = backOffColor;


                    if (changeRippleColor)
                        rippleConfig.rippleColor = backOffColor;
                    state = 0;
                }
            }

            if (lastToggleInteractableState != toggle.interactable)
            {
                lastToggleInteractableState = toggle.interactable;

                if (lastToggleInteractableState)
                    EnableSwitch();
                else
                    DisableSwitch();
            }

            if (!Application.isPlaying)
            {
                if (lastToggleState != toggle.isOn)
                {
                    lastToggleState = toggle.isOn;

                    if (lastToggleState)
                        TurnOnSilent();
                    else
                        TurnOffSilent();
                }

                if (changeRippleColor)
                    rippleConfig.rippleColor = switchImage.color;
            }
        }
    }
}
