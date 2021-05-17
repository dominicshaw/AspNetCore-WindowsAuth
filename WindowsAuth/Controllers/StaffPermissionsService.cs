using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace WindowsAuth.Controllers
{
    public class StaffPermissionsService
    {
        [Flags]
        private enum Departments
        {
            None = 0,
            HumanResources = 1,
            Compliance = 2,
            Developer = 4,
            Marketing = 8,
            Management = 16,
            Finance = 32
        }

        private static readonly Dictionary<string, Departments> _departmentLookup = new();

        private readonly string _username;

        public StaffPermissionsService(IHttpContextAccessor httpContextAccessor)
        {
            _username = httpContextAccessor.HttpContext?.User.Identity?.Name!;
        }

        public bool IsHumanResources() => GetDepartmentScore().HasFlag(Departments.HumanResources);
        public bool IsCompliance() => GetDepartmentScore().HasFlag(Departments.Compliance);
        public bool IsDeveloper() => GetDepartmentScore().HasFlag(Departments.Developer);
        public bool IsMarketing() => GetDepartmentScore().HasFlag(Departments.Marketing);
        public bool IsManagement() => GetDepartmentScore().HasFlag(Departments.Management);
        public bool IsFinance() => GetDepartmentScore().HasFlag(Departments.Finance);

        public int GetPermissionsFlag() => (int)GetDepartmentScore();

        private Departments GetDepartmentScore()
        {
            //var username = _username;

            //if (_username.StartsWith("DOMAIN", StringComparison.InvariantCultureIgnoreCase))
            //    username = username[6..];

            //if (_departmentLookup.TryGetValue(username, out var score))
            //    return score;

            //using var pctx = new PrincipalContext(ContextType.Domain, "DOMAIN", null, ContextOptions.Negotiate);
            //using var user = UserPrincipal.FindByIdentity(pctx, IdentityType.SamAccountName, username);

            //if (user != null)
            //{
            //    _departmentLookup[username] = Departments.None;

            //    using var groups = user.GetGroups();

            //    foreach (var group in groups.Where(x => x.Name.Contains("", StringComparison.OrdinalIgnoreCase)))
            //    {
            //        if (string.Equals(group.Name, "HR", StringComparison.OrdinalIgnoreCase))
            //            _departmentLookup[username] = _departmentLookup[username] | Departments.HumanResources;
            //        else if (string.Equals(group.Name, "Compliance", StringComparison.OrdinalIgnoreCase))
            //            _departmentLookup[username] = _departmentLookup[username] | Departments.Compliance;
            //        else if (string.Equals(group.Name, "DEV", StringComparison.OrdinalIgnoreCase))
            //            _departmentLookup[username] = _departmentLookup[username] | Departments.Developer;
            //        else if (string.Equals(group.Name, "Accounts", StringComparison.OrdinalIgnoreCase))
            //            _departmentLookup[username] = _departmentLookup[username] | Departments.Finance;
            //        else if (string.Equals(group.Name, "BusinessMgt", StringComparison.OrdinalIgnoreCase))
            //            _departmentLookup[username] = _departmentLookup[username] | Departments.Management;
            //        else if (string.Equals(group.Name, "Marketing", StringComparison.OrdinalIgnoreCase))
            //            _departmentLookup[username] = _departmentLookup[username] | Departments.Marketing;
            //    }

            //    return _departmentLookup[username];
            //}

            //_departmentLookup[username] = 0;
            return 0;
        }
    }
}