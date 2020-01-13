using System;

namespace decrypt
{
	class MainClass
	{
		public string Decrypt(string cipherText, string passPhrase)
		{
			// Get the complete stream of bytes that represent:
			// [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
			var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
			// Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
			var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
			// Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
			var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
			// Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
			var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

			var password = new Rfc2898DeriveBytes (passPhrase, saltStringBytes, DerivationIterations);
			var keyBytes = password.GetBytes(Keysize / 8);
			var symmetricKey = new RijndaelManaged();
			symmetricKey.BlockSize = 256;
			symmetricKey.Mode = CipherMode.CBC;
			symmetricKey.Padding = PaddingMode.PKCS7;
			var decryptor = symmetricKey.CreateDecryptor (keyBytes, ivStringBytes);
			var memoryStream = new MemoryStream (cipherTextBytes);
			var cryptoStream = new CryptoStream (memoryStream, decryptor, CryptoStreamMode.Read);
			var plainTextBytes = new byte[cipherTextBytes.Length];
			var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
		}
	}
}
