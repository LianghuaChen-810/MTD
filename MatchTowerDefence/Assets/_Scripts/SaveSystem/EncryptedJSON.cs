using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

#if !NETFX_CORE
namespace MatchTowerDefence.SaveSystem
{
    public class EncryptedJSON<Data> : JSONSave<Data> where Data : IData
    {
		const int initializationVectorLength = 16;

		const int keyLength = 32;

		static readonly byte[] s_Salt =
		{
			0x6b, 0xb0, 0xa1, 0x65, 0x08, 0xf8, 0xe6, 0xe8, 0x4d, 0x9e, 0x2f, 0x19, 0x97, 0xec, 0x0d, 0x6e,
			0xe7, 0xec, 0xe2, 0x0a, 0xd9, 0x47, 0xa7, 0x8d, 0xff, 0x3d, 0xe1, 0x65, 0x4f, 0x46, 0x00, 0x22
		};

		public EncryptedJSON(string _fileName): base(_fileName) {}

		/// <summary>
		/// Get device bytes to prevent copying save file to different device
		/// </summary>
		static byte[] GetUniqueDeviceBytes()
		{
			byte[] deviceIdentifier = Encoding.ASCII.GetBytes(SystemInfo.deviceUniqueIdentifier);

			return deviceIdentifier;
		}

		protected override StreamWriter WriteStream()
		{
			var underlyingStream = new FileStream(fileName, FileMode.Create);

			var byteGenerator = new Rfc2898DeriveBytes(GetUniqueDeviceBytes(), s_Salt, 1000);
			var random = new RNGCryptoServiceProvider();
			byte[] key = byteGenerator.GetBytes(keyLength);
			byte[] iv = new byte[initializationVectorLength];
			random.GetBytes(iv);

			Rijndael rijndael = Rijndael.Create();
			rijndael.Key = key;
			rijndael.IV = iv;

			underlyingStream.Write(iv, 0, initializationVectorLength);
			var encryptedStream = new CryptoStream(underlyingStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);

			return new StreamWriter(encryptedStream);
		}

		protected override StreamReader ReadStream()
		{
			var underlyingStream = new FileStream(fileName, FileMode.Open);

			var byteGenerator = new Rfc2898DeriveBytes(GetUniqueDeviceBytes(), s_Salt, 1000);
			byte[] key = byteGenerator.GetBytes(keyLength);
			byte[] iv = new byte[initializationVectorLength];

			underlyingStream.Read(iv, 0, initializationVectorLength);

			Rijndael rijndael = Rijndael.Create();
			rijndael.Key = key;
			rijndael.IV = iv;

			var encryptedStream = new CryptoStream(underlyingStream, rijndael.CreateDecryptor(), CryptoStreamMode.Read);

			return new StreamReader(encryptedStream);
		}
	}
}
#endif