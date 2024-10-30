﻿using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OIdentNetLib.Application.Common;
using OIdentNetLib.Application.OAuth.Contracts;
using OIdentNetLib.Application.OAuth.DataTransferObjects;
using OIdentNetLib.Application.OAuth.Models;
using OIdentNetLib.Application.Options;
using OIdentNetLib.Infrastructure.Database;
using OIdentNetLib.Infrastructure.Database.Contracts;

namespace OIdentNetLib.Application.OAuth;

public class TokenSessionProcessor(
    ILogger<TokenSessionProcessor> logger,
    IOptions<OIdentOptions> oidentOptions,
    ITokenSessionReader tokenSessionReader,
    ITokenSessionWriter tokenSessionWriter
) : ITokenSessionProcessor
{
    public async Task<GenericHttpResponse<ProcessTokenSessionResponse>> ProcessAsync(
        ProcessTokenSessionRequest processTokenSessionRequest)
    {
        // Validate SessionId
        if (!processTokenSessionRequest.SessionId.HasValue)
        {
            return GenericHttpResponse<ProcessTokenSessionResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OAuthErrorTypes.InvalidRequest,
                "SessionId is required.");
        }

        // Validate ClientId
        if (!processTokenSessionRequest.ClientId.HasValue)
        {
            return GenericHttpResponse<ProcessTokenSessionResponse>.CreateErrorResponse(
                HttpStatusCode.BadRequest,
                OAuthErrorTypes.InvalidRequest,
                "ClientId is required.");
        }
        
        // Get or create the token session
        var tokenSession = await tokenSessionReader.ReadByIdAsync(processTokenSessionRequest.SessionId.Value);
        if (tokenSession == null)
        {
            var now = DateTime.UtcNow;
            tokenSession = new TokenSession
            {
                TokenSessionId = processTokenSessionRequest.SessionId,
                SessionCreatedAt = now,
                SessionExpiresAt = now.AddMinutes(oidentOptions.Value.TokenSessionExpirationInMinutes),
                ClientId = processTokenSessionRequest.ClientId,
                UserId = processTokenSessionRequest.UserId                
            };
        }
        
        // Create the access token
        

        await Task.CompletedTask;
        throw new NotImplementedException();
    }
}