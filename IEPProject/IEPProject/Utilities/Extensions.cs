using IEPProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Microsoft.AspNet.Identity;

namespace IEPProject.Utilities
{
    public static class Extensions
    {
        public static ApplicationUser GetApplicationUser(this IIdentity identity)
        {
            if (identity.IsAuthenticated)
            {
                using (var context = new ApplicationDbContext())
                {
                    var username = identity.GetUserName();
                    return context.Users.Where(u => u.UserName == username).First();
                }
            }
            else
            {
                return null;
            }
        }
    }
}