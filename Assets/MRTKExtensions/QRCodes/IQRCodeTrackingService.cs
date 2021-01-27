using System;
using Microsoft.MixedReality.Toolkit;

namespace MRTKExtensions.QRCodes
{
    public interface IQRCodeTrackingService : IMixedRealityExtensionService
    {
        event EventHandler Initialized;
        event EventHandler<string> ProgressMessageSent;
        event EventHandler<QRInfo> QRCodeFound;
        bool InitializationFailed { get;}
        string ErrorMessage { get; }
        string ProgressMessages { get; }
        bool IsSupported { get; }
        bool IsTracking { get; }
        bool IsInitialized { get; }
    }
}