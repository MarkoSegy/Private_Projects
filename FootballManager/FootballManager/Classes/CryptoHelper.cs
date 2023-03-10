using FootballManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FootballManager.Classes
{
	public class CryptoHelper
	{
		public static void SaltAndEncrypt(Person person)
		{
			if (person.Password != null)
			{
				byte[] input = Encoding.UTF8.GetBytes(person.Password);
				byte[] result;
				SHA512 shaM = new SHA512Managed();
				//1. salten: zufallsbytes erzeugen und an das pwd dranhaengen; erst dann hashen
				RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
				byte[] saltByteArray = new byte[128];
				rngCsp.GetBytes(saltByteArray);
				person.SaltedPassword = Convert.ToBase64String(saltByteArray);
				//2. plaintext pwd und salt werden zusammen gehashed
				var combinedArray = input.Concat(saltByteArray).ToArray();
				result = shaM.ComputeHash(combinedArray);

				//person.PasswordEncrypted = System.Text.Encoding.UTF8.GetString(result);
				person.PasswordEncrypted = Convert.ToBase64String(result);

				//return Convert.ToBase64String(result); 
			}
		}
	}
}
