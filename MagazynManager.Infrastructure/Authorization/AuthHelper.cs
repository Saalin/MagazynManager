using MagazynManager.Technical.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MagazynManager.Infrastructure.Authorization
{
    public static class AuthHelper
    {
        private static readonly Dictionary<char, Access> accessByLetter = Enum.GetValues(typeof(Access)).Cast<Access>().ToDictionary(key => key.GetAttributeOfType<AccessLetterAttribute>().Letter, value => value);
        private static readonly Dictionary<string, AppArea> appAreasByName = Enum.GetValues(typeof(AppArea)).Cast<AppArea>().ToDictionary(key => key.GetAttributeOfType<PermissionNameAttribute>().Name, value => value);

        public static Access StringToAccess(string accessString)
        {
            Access access = 0;

            foreach (var letter in accessString)
            {
                if (accessByLetter.ContainsKey(letter))
                {
                    access |= accessByLetter[letter];
                }
                else
                {
                    throw new ArgumentException($"Invalid access letter: {letter}", nameof(accessString));
                }
            }

            return access;
        }

        public static AppArea StringToAppArea(string appAreaString)
        {
            if (appAreasByName.ContainsKey(appAreaString))
            {
                return appAreasByName[appAreaString];
            }

            throw new ArgumentException("Invalid app area string", nameof(appAreaString));
        }

        public static string AccessToString(Access access)
        {
            var sb = new StringBuilder();

            foreach (var item in (Access[])Enum.GetValues(typeof(Access)))
            {
                if ((access & item) == item)
                {
                    sb.Append(item.GetAttributeOfType<AccessLetterAttribute>().Letter);
                }
            }

            return sb.ToString();
        }

        public static string PermissionToPolicy(AppArea area, Access access)
        {
            return AuthConst.PolicyPrefix + area.GetAttributeOfType<PermissionNameAttribute>().Name + "," + AccessToString(access);
        }

        public static (AppArea area, Access access) PolicyToPermission(string policy)
        {
            if (!policy.StartsWith(AuthConst.PolicyPrefix))
            {
                throw new ArgumentException("Invalid policy structure", nameof(policy));
            }

            string policyContent = policy.Substring(AuthConst.PolicyPrefix.Length);

            string[] data = policyContent.Split(",");

            if (data.Length != 2)
            {
                throw new ArgumentException("Invalid policy structure", nameof(policy));
            }

            return (StringToAppArea(data[0]), StringToAccess(data[1]));
        }

        public static string PermissionToClaim(string permission)
        {
            return AuthConst.ClaimPrefix + permission;
        }
    }
}