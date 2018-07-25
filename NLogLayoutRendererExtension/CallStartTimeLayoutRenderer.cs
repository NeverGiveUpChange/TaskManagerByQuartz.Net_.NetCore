using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLogLayoutRendererExtension
{
    [LayoutRenderer("callstarttime")]
    public class CallStartTimeLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.Parameters[4].ToString());
        }
    }
}
