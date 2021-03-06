﻿// <copyright file="OktaMvcOptions.cs" company="Okta, Inc">
// Copyright (c) 2018-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Generic;

namespace Okta.AspNetCore
{
    public class OktaMvcOptions : AspNet.Abstractions.OktaWebOptions
    {
        public string ClientSecret { get; set; }

        public string ClientId { get; set; }

        public string CallbackPath { get; set; } = OktaDefaults.CallbackPath;

        public string PostLogoutRedirectUri { get; set; }

        public IList<string> Scope { get; set; } = OktaDefaults.Scope;

        public bool GetClaimsFromUserInfoEndpoint { get; set; } = false;
    }
}
