using Microsoft.AspNetCore.Identity;
using System;

namespace eShopSolution.Data.EF.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
