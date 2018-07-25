using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLogLayoutRendererExtension
{
    [LayoutRenderer("exception_msg")]
    public class ExceptionMessageLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.Parameters[7].ToString());
        }
    }
}
