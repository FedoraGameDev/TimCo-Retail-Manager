using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

namespace FedoraDev.TimCo.UserInterface.Portal.Authentication
{
	public static class JwtParser
	{
		public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
		{
			List<Claim> claims = new List<Claim>();
			string payload = jwt.Split('.')[1];

			byte[] jsonBytes = ParseBase64WithoutPadding(payload);
			Dictionary<string, object> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
			
			ExtractRolesFromJwt(ref claims, ref keyValuePairs);

			claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

			return claims;
		}

		private static void ExtractRolesFromJwt(ref List<Claim> claims, ref Dictionary<string, object> keyValuePairs)
		{
			keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

			if (roles != null)
			{
				string[] parsedRoles = roles.ToString().Trim().TrimStart('[').TrimEnd(']').Split(',');

				if (parsedRoles.Length > 1)
					foreach (string parsedRole in parsedRoles)
						claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
				else
					claims.Add(new Claim(ClaimTypes.Role, parsedRoles[0]));

				_ = keyValuePairs.Remove(ClaimTypes.Role);
			}
		}

		private static byte[] ParseBase64WithoutPadding(string base64)
		{
			int remainder = base64.Length % 4;
			base64 += remainder == 2 ? "==" : "";
			base64 += remainder == 3 ? "=" : "";

			return Convert.FromBase64String(base64);
		}
	}
}
