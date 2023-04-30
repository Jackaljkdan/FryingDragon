using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Flames
{
    [Serializable]
    public struct FirefighterSpawnSignal
    {
        public FirefighterExit firefighter;
    }

    [Serializable]
    public struct FirefighterExitSignal
    {
    }
}