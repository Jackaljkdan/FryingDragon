using JK.Injection;
using JK.Observables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JK.UI
{
    [DisallowMultipleComponent]
    public class ButtonIcon : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private ButtonType _buttonType;

        public CrossPlatformButtonIconSet iconSet;

        public Image image;

        [Injected]
        public InputTypeDetector inputTypeDetector;

        private void Reset()
        {
            image = GetComponent<Image>();
        }

        #endregion

        public ButtonType ButtonType
        {
            get => _buttonType;
            set
            {
                if (_buttonType == value)
                    return;

                _buttonType = value;
                Refresh();
            }
        }

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            inputTypeDetector = context.GetOptional<InputTypeDetector>();
        }

        private void Awake()
        {
            Inject();
        }


        private void OnEnable()
        {
            Refresh();

            if (inputTypeDetector != null)
                inputTypeDetector.current.onChange.AddListener(OnInputTypeChanged);
        }

        private void OnDisable()
        {
            if (inputTypeDetector != null)
                inputTypeDetector.current.onChange.RemoveListener(OnInputTypeChanged);
        }

        private void OnInputTypeChanged(ObservableProperty<InputType>.Changed arg0)
        {
            Refresh();
        }

        private ButtonIconSet GetPlatformIconSet()
        {
            if (inputTypeDetector != null)
                return iconSet.Get(inputTypeDetector.current.Value);

            return iconSet.keyboard;
        }

        public void Refresh()
        {
            image.sprite = GetPlatformIconSet().Get(ButtonType);
        }
    }
}