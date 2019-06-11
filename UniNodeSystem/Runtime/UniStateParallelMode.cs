﻿namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;
    using UniTools.UniRoutine.Runtime;
    using UnityEngine.Serialization;

    [Serializable]
    public class UniStateParallelMode
    {
        [FormerlySerializedAs("State")] 
        public UniStateTransition Transition;
        public RoutineType RoutineType = RoutineType.UpdateStep;
        public bool RestartOnComplete = true;
    }
}
