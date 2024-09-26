using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Spotify.Slsk.Integration.Models;
using Spotify.Slsk.Integration.Services.Download;
using Spotify.Slsk.Integration.Services.SoulSeek;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Soulseek;

namespace Spotify.Slsk.Integration.Cli.Commands
{
    [HelpOption("--help")]
	abstract class SpotSeekCommandBase
	{

        protected SoulseekService _soulseekService;
        protected DownloadService _downloadService;
        protected SoulseekClient _soulseekClient;
        private UserProfile _userProfile;
		protected ILogger _logger;
		protected IConsole _console;

        [Option(CommandOptionType.SingleValue, ShortName = "", LongName = "profile", Description = "local profile name", ValueName = "profile name", ShowInHelpText = true)]
        public string Profile { get; set; } = "default";

        protected virtual Task<int> OnExecute(CommandLineApplication app)
		{
            _downloadService = new();
            _soulseekService = new();
            _soulseekClient = SoulseekService.GetClient();
			return Task.FromResult(0);
		}

        protected static string ProfileFolder
        {
            get
            {
                return $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile, Environment.SpecialFolderOption.Create)}/.spotseek/";
            }
        }


        protected UserProfile UserProfile
        {
            get
            {
                if (_userProfile == null)
                {
                    var text = System.IO.File.ReadAllText($"{ProfileFolder}{Profile}");
                    if (!string.IsNullOrEmpty(text))
                    {
                        _userProfile = JsonSerializer.Deserialize<UserProfile>(text);
                        if (_userProfile != null)
                        {
                            _userProfile.Password = Decrypt(_userProfile.Password);
                        }
                    }
                }
                return _userProfile;
            }
        }

        protected static string SecureStringToString(SecureString value)
		{
			IntPtr valuePtr = IntPtr.Zero;
			try
			{
				valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
				return Marshal.PtrToStringUni(valuePtr);
			}
			finally
			{
				Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
			}
		}

        private static string EncryptKey
        {
            get
            {
                int keyLen = 32;
                string key = Environment.UserName;
                if (key.Length > keyLen)
                {
                    key = key.Substring(0, keyLen);
                }
                else if (key.Length < keyLen)
                {
                    int len = key.Length;
                    for (int i = 0; i < keyLen - len; i++)
                    {
                        key += ((char)(65 + i)).ToString();
                    }
                }
                return key;
            }
        }

        protected static string Encrypt(string text)
        {
            string keyString = EncryptKey;
            byte[] key = Encoding.UTF8.GetBytes(keyString);
            using (Aes aesAlg = Aes.Create())
            {
                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }
                        byte[] iv = aesAlg.IV;
                        byte[] decryptedContent = msEncrypt.ToArray();
                        byte[] result = new byte[iv.Length + decryptedContent.Length];
                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        protected static string Decrypt(string cipherText)
        {
            string keyString = EncryptKey;
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            byte[] iv = new byte[16];
            byte[] cipher = new byte[fullCipher.Length - 16];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            byte[] key = Encoding.UTF8.GetBytes(keyString);
            using (Aes aesAlg = Aes.Create())
            {
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (MemoryStream msDecrypt = new MemoryStream(cipher))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                    return result;
                }
            }
        }

        protected void OnException(Exception ex)
		{
			OutputError(ex.Message);
			_logger.LogError(ex.Message);
			_logger.LogDebug(ex, ex.Message);
		}

		protected void OutputToConsole(string data)
		{
			_console.BackgroundColor = ConsoleColor.Black;
			_console.ForegroundColor = ConsoleColor.White;
			_console.Out.Write(data);
			_console.ResetColor();
		}

		protected void OutputError(string message)
		{
			_console.BackgroundColor = ConsoleColor.Red;
			_console.ForegroundColor = ConsoleColor.White;
			_console.Error.WriteLine(message);
			_console.ResetColor();
		}

    }
}

