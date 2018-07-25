using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLogLayoutRendererExtension
{
    [LayoutRenderer("requestmethod")]
    public class RequestMethodLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.Parameters[2].ToString());
        }
    }
}
