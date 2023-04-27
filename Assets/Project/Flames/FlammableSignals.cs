using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [Serializable]
    public struct FireStartSignal
    {
        public Flammable flammable;

        public FireStartSignal(Flammable flammable)
        {
            this.flammable = flammable;
        }
    }

    [Serializable]
    public struct FireStopSignal
    {
        public Flammable flammable;

        public FireStopSignal(Flammable flammable)
        {
            this.flammable = flammable;
        }
    }
}