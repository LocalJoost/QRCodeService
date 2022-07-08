using System;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using Microsoft.MixedReality.OpenXR;

namespace MRTKExtensions.QRCodes
{
    public class OpenXRSpatialGraphCoordinateSystemSetter : SpatialGraphCoordinateSystemSetter
    {
        protected override void UpdateLocation(Guid spatialGraphNodeId, float physicalSideLength)
        {
            var node = spatialGraphNodeId != Guid.Empty ? SpatialGraphNode.FromStaticNodeId(spatialGraphNodeId) : null;
            if (node != null && node.TryLocate(FrameTime.OnUpdate, out Pose pose))
            {
                if (CameraCache.Main.transform.parent != null)
                {
                    pose = pose.GetTransformedBy(CameraCache.Main.transform.parent);
                }

                MovePoseToCenter(pose,physicalSideLength);
            }
            else
            {
                PositionAcquisitionFailed?.Invoke(this, null);
            }
        }
    }
}