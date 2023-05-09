using JK.Utils.Addressables;
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

        private void Start()
        {
            Refresh();
        }

        public void Refresh()
        {
            // TODO: usare il set di icone corretto

            iconSet.controller.Get(ButtonType).LoadAssetAsyncIfNecessary().Completed += handle =>
            {
                image.sprite = handle.Result as Sprite;
            };
        }
    }
}