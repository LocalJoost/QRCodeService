using UnityEngine;
using UnityEngine.Events;

namespace MRTKExtensions.QRCodes
{
    public abstract class BaseTrackerController : MonoBehaviour
    {
        public abstract void ResetTracking();
        
        protected readonly UnityEvent<Pose> positionSet = new();
        public UnityEvent<Pose> PositionSet => positionSet;
    }
}