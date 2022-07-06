using System;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit;
using MRTKExtensions.QRCodes;
using UnityEngine;

public class MultiQRTrackerController : MonoBehaviour
{
    [SerializeField] private ReferenceQRTextLibrary _referenceQrTextLibrary;

    public EventHandler<Pose> PositionSet;

    private SpatialGraphCoordinateSystemSetter _spatialGraphCoordinateSystemSetter;
    private Transform markerHolder;
    private AudioSource audioSource;
    private GameObject markerDisplay;
    private QRInfo lastMessage;
    private GameObject placeObject;
    private List<GameObject> placedGameObjects = new List<GameObject>();

    public bool IsTrackingActive { get; private set; } = true;

    private IQRCodeTrackingService qrCodeTrackingService;

    private IQRCodeTrackingService QRCodeTrackingService
    {
        get
        {
            while (!MixedRealityToolkit.IsInitialized && Time.time < 5) ;
            return qrCodeTrackingService ??
                   (qrCodeTrackingService = MixedRealityToolkit.Instance.GetService<IQRCodeTrackingService>());
        }
    }

    void Awake()
    {
        _spatialGraphCoordinateSystemSetter = GetComponentInChildren<SpatialGraphCoordinateSystemSetter>();
    }

    private void Start()
    {
        if (!QRCodeTrackingService.IsSupported)
        {
            return;
        }

        markerHolder = _spatialGraphCoordinateSystemSetter.gameObject.transform;
        markerDisplay = markerHolder.GetChild(0).gameObject;
        markerDisplay.SetActive(false);

        audioSource = markerHolder.gameObject.GetComponent<AudioSource>();

        QRCodeTrackingService.QRCodeFound += ProcessTrackingFound;
        _spatialGraphCoordinateSystemSetter.PositionAcquired += SetPosition;
        _spatialGraphCoordinateSystemSetter.PositionAcquisitionFailed +=
            (s, e) => ResetTracking();


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
        QRCodeTrackingService.Enable();
    }

    public void ResetTracking()
    {
        if (QRCodeTrackingService.IsInitialized)
        {
            markerDisplay.SetActive(false);
            IsTrackingActive = true;
        }
    }

    private void ProcessTrackingFound(object sender, QRInfo msg)
    {
        if (msg == null || !IsTrackingActive)
        {
            return;
        }

        lastMessage = msg;

        foreach (var item in _referenceQrTextLibrary.QRContentPrefabs)
        {
            if (msg.Data == item.QRText &&
                Math.Abs((DateTimeOffset.UtcNow - msg.LastDetectedTime.UtcDateTime).TotalMilliseconds) < 200)
            {
                _spatialGraphCoordinateSystemSetter.SetLocationIdSize(msg.SpatialGraphNodeId,
                    msg.PhysicalSideLength);
                placeObject = item.Prefab;
            }
        }
    }

    private void SetPosition(object sender, Pose pose)
    {
        IsTrackingActive = false;
        markerHolder.localScale = Vector3.one * lastMessage.PhysicalSideLength;

        if (placeObject != null)
        {
            if (GetPlacedObject(placeObject.name) == null)
            {
                GameObject go = Instantiate(placeObject, markerHolder.position, markerHolder.rotation);
                go.name = placeObject.name;
                placedGameObjects.Add(go);
            }
            else
            {
                GetPlacedObject(placeObject.name).transform.SetPositionAndRotation(markerHolder.position, markerHolder.rotation);
            }
        }
        
        markerDisplay.SetActive(true);
        PositionSet?.Invoke(this, pose);
        audioSource.Play();
    }

    GameObject GetPlacedObject(string name)
    {
        return placedGameObjects.Find(x => x.name.Equals(name));
    }
}
