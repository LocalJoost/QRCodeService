using System;
using System.Threading.Tasks;
using RealityCollective.ServiceFramework.Services;
using UnityEngine;

namespace MRTKExtensions.QRCodes
{
    public class ContinuousQrTrackerController : BaseTrackerController
    {
        [SerializeField]
        private SpatialGraphCoordinateSystemSetter spatialGraphCoordinateSystemSetter;

        [SerializeField]
        private string locationQrValue = string.Empty;

        private Transform markerHolder;
        private GameObject markerDisplay;
        private QRInfo lastMessage;
        private float resetTime = 0;

        private IQRCodeTrackingService qrCodeTrackingService;

        private IQRCodeTrackingService QrCodeTrackingService =>
            qrCodeTrackingService ??= ServiceManager.Instance.GetService<IQRCodeTrackingService>();

        private async Task Start()
        {
            markerHolder = spatialGraphCoordinateSystemSetter.gameObject.transform;
            markerDisplay = markerHolder.GetChild(0).gameObject;
            markerDisplay.SetActive(false);
            ResetTracking(false);
            // Give service time to start;
            await Task.Delay(250);
            if (!QrCodeTrackingService.IsSupported)
            {
                return;
            }   

            QrCodeTrackingService.QRCodeFound += ProcessTrackingFound;
            spatialGraphCoordinateSystemSetter.PositionAcquired += SetPosition;
        }

        private void ProcessTrackingFound(object sender, QRInfo msg)
        {
            if (msg == null || markerDisplay.activeSelf || resetTime > Time.time)
            {
                return;
            }

            lastMessage = msg;

            if (msg.Data == locationQrValue &&
                Math.Abs((DateTimeOffset.UtcNow - msg.LastDetectedTime.UtcDateTime).TotalMilliseconds) < 200)
            {
                spatialGraphCoordinateSystemSetter.SetLocationIdSize(msg.SpatialGraphNodeId,
                    msg.PhysicalSideLength);
            }
        }

        public override void ResetTracking()
        {
            ResetTracking(true);
        }
        
        private void ResetTracking(bool delayed = true)
        {
            if (delayed)
            {
                resetTime = Time.time + 2;
            }

            markerDisplay.SetActive(false);
        }

        private void SetPosition(object sender, Pose pose)
        {
            markerHolder.localScale = Vector3.one * lastMessage.PhysicalSideLength;
            markerDisplay.SetActive(true);
            positionSet?.Invoke(pose);
        }
    }
}