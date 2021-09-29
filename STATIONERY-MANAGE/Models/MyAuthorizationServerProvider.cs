using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace STATIONERY_MANAGE.Models
{
    
        public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
        {
            public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
            {
                context.Validated();
            }
            public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
            {
                using (UserRepository _repo = new UserRepository())
                {
                    var user = _repo.ValidateUser(context.UserName, context.Password);
                    if (user == null)
                    {
                        context.SetError("invalid_grant", "Provided username and password is incorrect");
                        return;
                    }
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                    identity.AddClaim(new Claim(ClaimTypes.MobilePhone, user.phone));
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, user.firstname));
                identity.AddClaim(new Claim(ClaimTypes.Surname, user.lastname));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.id.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.email));
                    context.Validated(identity);
                }
            }
        }
    
}
