using UnityEngine;
using Microsoft.MixedReality.Toolkit;

namespace MRTKExtensions.QRCodes
{
	[MixedRealityServiceProfile(typeof(IQRCodeTrackingService))]
	[CreateAssetMenu(fileName = "QRCodeTrackingServiceProfile", menuName = "MixedRealityToolkit/QRCodeTrackingService Configuration Profile")]
	public class QRCodeTrackingServiceProfile : BaseMixedRealityProfile
	{
        [SerializeField] 
		[Tooltip("Number of seconds before retrying to get access to the camera")]
        private int accessRetryTime = 5000;
        public int AccessRetryTime => accessRetryTime;

        [SerializeField]
        [Tooltip("Expose progress and debug messages")]
        private bool exposedProgressMessages = true;
        public bool
        ExposedProgressMessages => exposedProgressMessages;

        [SerializeField]
        [Tooltip("Number of debug message lines")]
        private int debugMessages = 10;
        public int DebugMessages => debugMessages;
    }
}