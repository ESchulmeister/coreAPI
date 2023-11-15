using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using coreAPI.Services;
using CoreAPI.Utilities;


namespace coreAPI.Utilities
{

    /// <summary>
    /// /Common api error handler
    /// Gets invoked on any application error  - ref. Startup.cs.  
    /// </summary>
    public class ErrorHandlingMiddleware
    {

        private readonly AppSettings _appSettings;

        private readonly RequestDelegate _next;

        private const string  General_Error = "An Unexpected  error has occurred. Please contact the system administrator.";



        #region constructor
        public ErrorHandlingMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }
        #endregion


        #region Methods


        public async Task Invoke(HttpContext context, IUserRepository repo,  ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    await this.AttachUserToContext(context, repo, token, logger);
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

   
        /// <summary>
        /// Process - handle errors @ each request
        /// </summary>
        /// <param name="oHttpContext"></param>
        /// <param name="oException">application error</param>
        /// <param name="logger">Dependency Injection - logger injected</param>
        /// <returns></returns>
        private async Task HandleExceptionAsync(HttpContext oHttpContext, Exception oException , ILogger<ErrorHandlingMiddleware> logger)
        {
            var sErrorMsg = string.Empty;

            if (oException.InnerException == null)    //Message specified @ request,unhandled exeption being thrown 
            {
                sErrorMsg = oException.ToString();
            }
            else
            {
                sErrorMsg = oException.InnerException.ToString();
            }

            //log error @ log4net
            logger.LogError(sErrorMsg);

            int iStatusCode = (int)HttpStatusCode.InternalServerError;
            string sMessage = oException.Message.Replace("\r\n", " ");

            sMessage = General_Error;

            //type of  exption thrown:
            switch (oException)
            {
                case SqlException     //any database specific sql ex, e.g. network error connecting
                AmbiguousMatchException:    
                    break;
                case DataValidationException 
                     BadRequestException:
                    iStatusCode = (int)HttpStatusCode.BadRequest;
                    sMessage = oException.Message;
                    break;
                case NotFoundException:
                    iStatusCode = (int)HttpStatusCode.NotFound;
                    sMessage = oException.Message;
                    break;
                case DbUpdateException:
                    break;
            }

            var oErrorMessageObject = new { Message = sMessage};
            var oResponse = oHttpContext.Response;

            oResponse.ContentType = "application/json";
            oResponse.StatusCode = iStatusCode;

            //write  out error message
            sMessage = JsonConvert.SerializeObject(oErrorMessageObject);
            await oResponse.WriteAsync(sMessage);
        }



        /// <summary>
        /// Upon successfull login, JWT 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="repo"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task AttachUserToContext(HttpContext context, IUserRepository repo, string token,  ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                var user = await repo.GetUserAsync( userId);

                // attach user to context on successful jwt validation
                context.Items["User"] = user; 
            }
            catch  
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
                //throw possible db error to display general error
                throw;

            }
        }

     
        #endregion 
    }
}
