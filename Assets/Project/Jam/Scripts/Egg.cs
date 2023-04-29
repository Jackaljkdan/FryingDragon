using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Jam
{
    [DisallowMultipleComponent]
    public class Egg : MonoBehaviour
    {
        #region Inspector

        public float removeSeconds = 5f;
        public float scaleAnimationSeconds = 0.5f;

        #endregion

        private void Start()
        {
            Invoke(nameof(Remove), 5f);
        }

        private void Remove()
        {
            this.
            transform.DOScale(Vector3.zero, scaleAnimationSeconds).SetEase(Ease.Linear).OnComplete(() => Destroy(gameObject));
        }
    }
}