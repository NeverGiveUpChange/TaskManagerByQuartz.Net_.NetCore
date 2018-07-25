using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLogLayoutRendererExtension
{
    [LayoutRenderer("elapsedtime")]
    public class ElapsedTimeLayoutRenderer : LayoutRenderer
    {
    
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.Parameters[6].ToString());
        }
    }
}
