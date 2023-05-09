using JK.UI;
using JK.Utils;
using JK.Utils.Addressables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace JK.UI
{
    [DisallowMultipleComponent]
    public class InputList : MonoBehaviour
    {
        #region Inspector

        public AssetReferenceComponent<InputEntry> entryAsset;

        public RectTransform parent;

        [RuntimeField]
        public List<VirtualInputEntry> list;

        private void Reset()
        {
            parent = transform as RectTransform;
        }

        #endregion

        private void Awake()
        {
            transform.DestroyAndDetachChildren();
        }

        public void AddEntry(VirtualInputEntry virtualEntry)
        {
            list.Add(virtualEntry);

            entryAsset.InstantiateAsync(parent).Completed += handle =>
            {
                // in the meantime the entry may have been removed
                int index = list.FindIndex(item => item.buttonType == virtualEntry.buttonType);

                if (index < 0)
                {
                    Destroy(handle.Result.gameObject);
                    entryAsset.ReleaseInstance(handle.Result.gameObject);
                }
                else
                {
                    handle.Result.icon.ButtonType = virtualEntry.buttonType;
                    handle.Result.text.text = virtualEntry.text;
                    handle.Result.name = "InputEntry-" + virtualEntry.buttonType;
                    list[index] = VirtualInputEntry.From(virtualEntry, handle.Result);
                }
            };
        }

        public void RemoveEntry(ButtonType buttonType)
        {
            int index = list.FindIndex(item => item.buttonType == buttonType);

            if (index < 0)
                return;

            var entry = list[index].realEntry;

            if (entry != null)
            {
                Destroy(entry.gameObject);
                entryAsset.ReleaseInstance(entry.gameObject);
            }

            list.RemoveAt(index);
        }
    }
}