using System;
using MRTKExtensions.QRCodes;
using RealityCollective.ServiceFramework.Services;
using TMPro;
using UnityEngine;

public class QRCodeDisplayController : MonoBehaviour
{
    [SerializeField]
    private float qrObservationTimeOut = 3500;

    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private TextMeshPro displayText;

    [SerializeField]
    private AudioSource confirmSound;

    private IQRCodeTrackingService qrCodeTrackingService;

    private QRInfo lastSeenCode;

    private IQRCodeTrackingService QRCodeTrackingService
    {
        get
        {
            while (!ServiceManager.Instance.IsInitialized && Time.time < 5) ;
            return qrCodeTrackingService ??
                   (qrCodeTrackingService = ServiceManager.Instance.GetService<IQRCodeTrackingService>());
        }
    }

    private void Start()
    {
        menu.SetActive(false);
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


    private void QRCodeTrackingService_Initialized(object sender, EventArgs e)
    {
        StartTracking();
    }

    private void StartTracking()
    {
        menu.SetActive(true);
        QRCodeTrackingService.QRCodeFound += QRCodeTrackingService_QRCodeFound;
        QRCodeTrackingService.Enable();
    }

    private void QRCodeTrackingService_QRCodeFound(object sender, QRInfo codeReceived)
    {
        if (lastSeenCode?.Data != codeReceived.Data)
        {
            displayText.text = $"code observed: {codeReceived.Data}";
            if (confirmSound.clip != null)
            {
                confirmSound.Play();
            }
        }
        lastSeenCode = codeReceived;
    }

    private void Update()
    {
        if (lastSeenCode == null)
        {
            return;
        }
        if (Math.Abs(
            (lastSeenCode.LastDetectedTime.UtcDateTime - DateTimeOffset.UtcNow).TotalMilliseconds) >
              qrObservationTimeOut)
        {
            lastSeenCode = null;
            displayText.text = string.Empty;
        }
    }
}
