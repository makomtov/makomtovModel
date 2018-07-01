using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using WebApiMTModel.Models;
using WebApiMTModel.Models.Models.View;

namespace WebApiMTModel
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        public OAuthProvider()
        { }
        #region[GrantResourceOwnerCredentials]
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var db = new Entities())
            {
                if (db != null)
                {
                   
                    var user = db.UsersTbl.ToList();
                    if (user != null)
                    {
                        var usersTbl = user.Where(u => u.UserEmail == context.UserName && u.UserPaswrd == context.Password).FirstOrDefault();
                       if (!string.IsNullOrEmpty(user.Where(u => u.UserEmail == context.UserName && u.UserPaswrd == context.Password).FirstOrDefault().UserEmail))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, usersTbl.UserRole));
                            
                            var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                {
                                    "userdisplayname", context.UserName
                                },
                                {
                                     "role", usersTbl.UserRole
                                }
                             });

                            var ticket = new AuthenticationTicket(identity, props);
                            context.Validated(ticket);
                        }
                        else
                        {
                            context.SetError("invalid_grant", "Provided username and password is incorrect");
                            context.Rejected();
                        }
                    }
                }
                else
                {
                    context.SetError("invalid_grant", "Provided username and password is incorrect");
                    context.Rejected();
                }
                return;
            }
        }
   

        #endregion

        #region[ValidateClientAuthentication]
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
                context.Validated();

            return Task.FromResult<object>(null);
        }
        #endregion

        #region[TokenEndpoint]
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            //ClaimsIdentity claimsIdentity = context.Identity.Claims.FirstOrDefault(u=>u.c;
            if (context.Properties.Dictionary["role"] == "admin")
            {
                context.Properties.ExpiresUtc = DateTime.UtcNow.AddDays(1);
            }
            return Task.FromResult<object>(null);
        }
        #endregion

        #region[CreateProperties]
        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
        #endregion
    }
}