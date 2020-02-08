using IdentityApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityApp.Context
{
    public class ProjectContext:IdentityDbContext<AppUser>
    {
        public ProjectContext():base("IdentityConnection")
        {
            
        }
    }
}