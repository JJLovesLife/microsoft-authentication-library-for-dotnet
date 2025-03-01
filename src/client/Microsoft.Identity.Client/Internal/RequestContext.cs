﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Internal.Logger;
using Microsoft.Identity.Client.TelemetryCore;
using Microsoft.Identity.Client.TelemetryCore.Internal.Events;
using Microsoft.Identity.Client.TelemetryCore.TelemetryClient;
using Microsoft.IdentityModel.Abstractions;

namespace Microsoft.Identity.Client.Internal
{
    internal class RequestContext
    {
        public Guid CorrelationId { get; } /* commonParameters.CorrelationId */
        public ILoggerAdapter Logger { get; }
        public IServiceBundle ServiceBundle { get; } /* _this.ServiceBundle */

        /// <summary>
        /// One and only one ApiEvent is associated with each request.
        /// </summary>
        public ApiEvent ApiEvent { get; set; }

        public CancellationToken UserCancellationToken { get; }

        public RequestContext(IServiceBundle serviceBundle, Guid correlationId, CancellationToken cancellationToken = default)
        {
            ServiceBundle = serviceBundle ?? throw new ArgumentNullException(nameof(serviceBundle));
            Logger = LoggerHelper.CreateLogger(correlationId, ServiceBundle.Config);
            CorrelationId = correlationId;
            UserCancellationToken = cancellationToken;
        }

        public TelemetryHelper CreateTelemetryHelper(ApiEvent eventToStart)
        {
            return new TelemetryHelper(
                ServiceBundle.HttpTelemetryManager,
                eventToStart);
        }
    }
}
