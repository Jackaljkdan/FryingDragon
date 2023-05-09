using JK.Injection;
using JK.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.Interaction
{
    [DisallowMultipleComponent]
    public class MultipleInputEntryAffordance : AbstractAffordance
    {
        #region Inspector

        public MultipleInteractionForwarder interaction;

        public string primaryText;
        public string secondaryText;
        public string tertiaryText;
        public string quaternaryText;

        [Injected]
        public InputList inputList;

        private void Reset()
        {
            interaction = GetComponentInParent<MultipleInteractionForwarder>();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            inputList = context.GetOptional<InputList>();
        }

        private void Awake()
        {
            Inject();
        }

        protected override void StartHighlightProtected(RaycastHit hit)
        {
            if (inputList == null)
                return;

            if (interaction.primaryTarget != null)
            {
                inputList.AddEntry(new VirtualInputEntry()
                {
                    buttonType = ButtonType.Primary,
                    text = primaryText,
                });
            }

            if (interaction.secondaryTarget != null)
            {
                inputList.AddEntry(new VirtualInputEntry()
                {
                    buttonType = ButtonType.Secondary,
                    text = secondaryText,
                });
            }

            if (interaction.tertiaryTarget != null)
            {
                inputList.AddEntry(new VirtualInputEntry()
                {
                    buttonType = ButtonType.Tertiary,
                    text = tertiaryText,
                });
            }

            if (interaction.quaternaryTarget != null)
            {
                inputList.AddEntry(new VirtualInputEntry()
                {
                    buttonType = ButtonType.Quaternary,
                    text = quaternaryText,
                });
            }
        }

        protected override void StopHighlightProtected()
        {
            if (inputList == null)
                return;

            if (interaction.primaryTarget != null)
                inputList.RemoveEntry(ButtonType.Primary);

            if (interaction.secondaryTarget != null)
                inputList.RemoveEntry(ButtonType.Secondary);

            if (interaction.tertiaryTarget != null)
                inputList.RemoveEntry(ButtonType.Tertiary);

            if (interaction.quaternaryTarget != null)
                inputList.RemoveEntry(ButtonType.Quaternary);
        }
    }
}