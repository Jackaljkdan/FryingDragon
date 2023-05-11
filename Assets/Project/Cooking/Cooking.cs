using CartoonFX;
using DG.Tweening;
using JK.Injection;
using JK.Utils.DGTweening;
using Project.Cooking.Recipes;
using Project.Items;
using Project.Items.Ingredients;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Project.Cooking
{
    [DisallowMultipleComponent]
    public class Cooking : MonoBehaviour
    {
        #region Inspector

        public float cookingTime = 10f;

        public string burnedText = "Burned";

        public Brazier brazier;

        public ParticleSystem particles;
        public ParticleSystem smokeParticles;
        public ParticleSystem readyParticles;
        public ParticleSystem textParticles;

        public CFXR_ParticleText dynamicParticleText;

        public UICookingSlider cookingSlider;

        [Injected]
        public OrderFulfiller fulfiller;

        [Injected]
        private SignalBus signalBus;

        [ContextMenu("Stop")]
        private void StopInEditMode()
        {
            if (Application.isPlaying)
                particles.Stop();
        }

        [ContextMenu("Start smoke")]
        private void StartSmokeInEditMode()
        {
            if (Application.isPlaying)
            {
                smokeParticles.gameObject.SetActive(true);
                smokeParticles.Play();
            }
        }

        [ContextMenu("Stop smoke")]
        private void StopSmokeInEditMode()
        {
            if (Application.isPlaying)
                smokeParticles.Stop();
        }

        #endregion

        [InjectMethod]
        public void Inject()
        {
            Context context = Context.Find(this);
            signalBus = context.Get<SignalBus>(this);
            fulfiller = context.Get<OrderFulfiller>(this);
        }

        private void Awake()
        {
            Inject();
        }

        public bool IsCooking => cookingTween.IsActiveAndPlaying();

        private Tween cookingTween;

        public void StartCooking()
        {
            readyParticles.gameObject.SetActive(false);
            particles.gameObject.SetActive(true);

            cookingTween = cookingSlider.DOFillWithScale(cookingTime);

            signalBus.Invoke(new CookingStartedSignal() { cookingTime = cookingTime, bowl = brazier.bowl });

            cookingTween.OnComplete(StartOvercooking);
        }

        public void StartOvercooking()
        {
            readyParticles.gameObject.SetActive(true);
            readyParticles.Play();
            signalBus.Invoke(new CookingFinishedSignal() { bowl = brazier.bowl });
            cookingTween = cookingSlider.DOOverfill(cookingTime);

            cookingTween.OnComplete(BurnedRecipe);
        }

        public void BurnedRecipe()
        {
            signalBus.Invoke(new CookingBurnedSignal() { bowl = brazier.bowl });
            cookingSlider.DOScaledown();
            dynamicParticleText.UpdateText(burnedText);
            textParticles.Play(true);
        }

        public void StopCooking()
        {
            if (!IsCooking)
                return;
            signalBus.Invoke(new CookingInterruptedSignal() { bowl = brazier.bowl });
            particles.Stop();
            cookingTween?.Kill(false);
            cookingSlider.DOScaledown();
        }
    }
}