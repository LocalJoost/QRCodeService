using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.XR.WSA;
#if WINDOWS_UWP
using Windows.Perception.Spatial;
#endif
namespace MRTKExtensions.QRCodes
{
    public class LegacySpatialGraphCoordinateSystemSetter : SpatialGraphCoordinateSystemSetter
    {
#if !UNITY_2020_1_OR_NEWER

        private PositionalLocatorState CurrentState { get; set; }

        void Awake()
        {
            CurrentState = PositionalLocatorState.Unavailable;
        }

        void Start()
        {
            WorldManager.OnPositionalLocatorStateChanged += WorldManager_OnPositionalLocatorStateChanged;
            CurrentState = WorldManager.state;
        }

        private void WorldManager_OnPositionalLocatorStateChanged(PositionalLocatorState oldState, PositionalLocatorState newState)
        {
            CurrentState = newState;
            gameObject.SetActive(newState == PositionalLocatorState.Active);
        }


        protected override void UpdateLocation(Guid spatialGraphNodeId, float physicalSideLength)
        {
            if (CurrentState != PositionalLocatorState.Active)
            {
                PositionAcquisitionFailed?.Invoke(this, null);
                return;
            }

            System.Numerics.Matrix4x4? relativePose = System.Numerics.Matrix4x4.Identity;
#if WINDOWS_UWP
            SpatialCoordinateSystem coordinateSystem = Windows.Perception.Spatial.Preview.SpatialGraphInteropPreview.CreateCoordinateSystemForNode(spatialGraphNodeId);

            if (coordinateSystem == null)
            {
                PositionAcquisitionFailed?.Invoke(this, null);
                return;
            }

            SpatialCoordinateSystem rootSpatialCoordinateSystem = (SpatialCoordinateSystem)System.Runtime.InteropServices.Marshal.GetObjectForIUnknown(WorldManager.GetNativeISpatialCoordinateSystemPtr());

            // Get the relative transform from the unity origin
            relativePose = coordinateSystem.TryGetTransformTo(rootSpatialCoordinateSystem);
#endif
            CalculatePosition(relativePose, physicalSideLength);
        }
#else
        protected override void UpdateLocation(Guid spatialGraphNodeId, float physicalSideLength)
        {
            throw new NotSupportedException($"{GetType().Name} is not supported in Unity 2020 and beyond");
        }
#endif

    }
}