﻿// <copyright file="OpenIdConnectOptionsHelper.cs" company="Okta, Inc">
// Copyright (c) 2018-present Okta, Inc. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Okta.AspNet.Abstractions;

namespace Okta.AspNetCore
{
    public class OpenIdConnectOptionsHelper
    {
        /// <summary>
        /// Configure an OpenIdConnectOptions object based on user's configuration.
        /// </summary>
        /// <param name="oktaMvcOptions">The <see cref="OktaMvcOptions"/> options.</param>
        /// <param name="events">The OpenIdConnect events.</param>
        /// <param name="oidcOptions">The OpenIdConnectOptions to configure.</param>
        public static void ConfigureOpenIdConnectOptions(OktaMvcOptions oktaMvcOptions, OpenIdConnectEvents events, OpenIdConnectOptions oidcOptions)
        {
            var issuer = UrlHelper.CreateIssuerUrl(oktaMvcOptions.OktaDomain, oktaMvcOptions.AuthorizationServerId);

            oidcOptions.ClientId = oktaMvcOptions.ClientId;
            oidcOptions.ClientSecret = oktaMvcOptions.ClientSecret;
            oidcOptions.Authority = issuer;
            oidcOptions.CallbackPath = new PathString(oktaMvcOptions.CallbackPath);
            oidcOptions.SignedOutCallbackPath = new PathString(OktaDefaults.SignOutCallbackPath);
            oidcOptions.SignedOutRedirectUri = oktaMvcOptions.PostLogoutRedirectUri;
            oidcOptions.ResponseType = OpenIdConnectResponseType.Code;
            oidcOptions.GetClaimsFromUserInfoEndpoint = oktaMvcOptions.GetClaimsFromUserInfoEndpoint;
            oidcOptions.SecurityTokenValidator = new StrictSecurityTokenValidator();
            oidcOptions.SaveTokens = true;
            oidcOptions.UseTokenLifetime = false;
            oidcOptions.BackchannelHttpHandler = new UserAgentHandler(
                "okta-aspnetcore",
                typeof(OktaAuthenticationOptionsExtensions).Assembly.GetName().Version);

            var hasDefinedScopes = oktaMvcOptions.Scope?.Any() ?? false;
            if (hasDefinedScopes)
            {
                oidcOptions.Scope.Clear();
                foreach (var scope in oktaMvcOptions.Scope)
                {
                    oidcOptions.Scope.Add(scope);
                }
            }

            oidcOptions.TokenValidationParameters = new DefaultTokenValidationParameters(oktaMvcOptions, issuer)
            {
                ValidAudience = oktaMvcOptions.ClientId,
                NameClaimType = "name",
            };

            oidcOptions.Events.OnRedirectToIdentityProvider = events.OnRedirectToIdentityProvider;
        }
    }
}
