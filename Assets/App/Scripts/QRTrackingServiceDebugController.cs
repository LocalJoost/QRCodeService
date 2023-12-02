using System.Threading.Tasks;
using MRTKExtensions.QRCodes;
using RealityCollective.ServiceFramework.Services;
using TMPro;
using UnityEngine;

public class QRTrackingServiceDebugController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro displayText;

    private IQRCodeTrackingService qrCodeTrackingService;

    private IQRCodeTrackingService QRCodeTrackingService =>
        qrCodeTrackingService ??= ServiceManager.Instance.GetService<IQRCodeTrackingService>();

    private async Task Start()
    {
        // Give service time to start;
        await Task.Delay(250);
        DisplayMessage(QRCodeTrackingService.ProgressMessages);
        QRCodeTrackingService.ProgressMessageSent += QRCodeTrackingService_ProgressMessageSent;
        if (!QRCodeTrackingService.IsSupported)
        {
            return;
        }

        if (QRCodeTrackingService.IsInitialized)
        {
            StartTracking();
        }
        else
        {
            QRCodeTrackingService.Initialized += QRCodeTrackingService_Initialized;
        }
    }

    private void QRCodeTrackingService_Initialized(object sender, System.EventArgs e)
    {
        StartTracking();
    }

    private void StartTracking()
    {
        QRCodeTrackingService.Enable();
    }

    private void QRCodeTrackingService_ProgressMessageSent(object sender, string e)
    {
        DisplayMessage(e);
    }

    private void DisplayMessage(string msg)
    {
        displayText.text = msg;
    }
}
