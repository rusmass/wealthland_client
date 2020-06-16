using System.Security.Cryptography;

namespace Core
{
	public class Md5sum
	{
		public string GetHexDigest16(byte[] inputBuffer, int startIndex, int length)
		{
			if (null == inputBuffer)
			{
				return string.Empty;
			}

			var bytes = _md5.ComputeHash (inputBuffer);
			if (null == _outputBuffer16) 
			{
				_outputBuffer16 = new char[16];
			}

			int highByte, lowByte;
			for (int i = 4, index = 0; i < 12; ++i) 
			{
				var temp = bytes[i];
				highByte = temp >> 4;
				lowByte = temp & 0x0f;

				_outputBuffer16[index++] = highByte < 10 ? (char)(highByte + 48) : (char)(highByte - 10 + 97);
				_outputBuffer16[index++] = lowByte < 10 ? (char)(lowByte + 48) : (char)(lowByte - 10 + 97);
			}

			var digest = new string (_outputBuffer16, startIndex, length);
			return digest;
		}

		public string GetHexDigest32(byte[] inputBuffer)
		{
			if (null == inputBuffer) 
			{
				return string.Empty;
			}

			var bytes = _md5.ComputeHash (inputBuffer);
			if (null == _outputBuffer32) 
			{
				_outputBuffer32 = new char[32];
			}
			
			int highByte, lowByte;
			for (int i = 0, index = 0; i < 16; ++i) 
			{
				var temp = bytes[i];
				highByte = temp >> 4;
				lowByte = temp & 0x0f;
				
				_outputBuffer32[index++] = highByte < 10 ? (char)(highByte + 48) : (char)(highByte - 10 + 97);
				_outputBuffer32[index++] = lowByte < 10 ? (char)(lowByte + 48) : (char)(lowByte - 10 + 97);
			}
			
			var digest = new string (_outputBuffer32);
			return digest;
		}

		/// <summary>
		/// 资源上传到CDN服务器时，用于防止由于文件同名而不被CDN更新，配合文件名使用，
		/// 以filename+digest的形式命名新文件，因此只需要非常短的签名就可以了
		/// </summary>
		public string GetAssetDigest (byte[] inputBuffer)
		{
			return GetHexDigest16(inputBuffer, 0, AssetDigestLength);
		}

		public static readonly Md5sum Instance = new Md5sum();
		public const int AssetDigestLength = 3;

		private char[] _outputBuffer16;
		private char[] _outputBuffer32;

		private readonly MD5 _md5 = new MD5CryptoServiceProvider();
	}
}