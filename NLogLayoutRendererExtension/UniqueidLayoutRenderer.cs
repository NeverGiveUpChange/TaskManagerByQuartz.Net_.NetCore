using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NLogLayoutRendererExtension
{
    [LayoutRenderer("uniqueid")]
    public class UniqueidLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {

            builder.Append(logEvent.Parameters[3].ToString());
        }
    }
}
