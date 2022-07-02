
using System;

namespace MRTKExtensions.Visualization
{
    [Flags]
    public enum VisualizationEnvironment
    {
        Editor = 1 << 1,
        RunTime = 1 << 2,
    }
}