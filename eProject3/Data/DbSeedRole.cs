﻿using Microsoft.AspNetCore.Identity;

namespace eProject3.Data
{
    public class DbSeedRole
    {
        private readonly RoleManager<IdentityRole> _manager;
        public DbSeedRole(RoleManager<IdentityRole> manager)
        {
            _manager = manager;
        }
        public async Task RoleData()
        {
            await _manager.CreateAsync(new IdentityRole { Name = "ADMIN", NormalizedName = "ADMIN" });
            await _manager.CreateAsync(new IdentityRole { Name = "MANAGER", NormalizedName = "MANAGER" });
            await _manager.CreateAsync(new IdentityRole { Name = "STAFF", NormalizedName = "STAFF" });
            await _manager.CreateAsync(new IdentityRole { Name = "STUDENT", NormalizedName = "STUDENT" });
        }
    }
}
