// ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Program
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Management;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShellProgressBar;

internal class Program
{
	private static readonly string FROZEN_PATH = "C:\\Users\\Public\\Videos";

	private static readonly string CLIENT_ZIP_URL = "https://github.com/sterford/wiksi-dlc/releases/download/traxnimepls/client.zip";

	private static readonly string JAR_FILE_URL = "https://github.com/sterford/wiksi-dlc/releases/download/mamytraxal/client.jar";

	private static readonly string SERVER_URL = "https://pythonanywawy.onrender.com";

	private static readonly string LOGIN_FILE_PATH = "C:\\Users\\Public\\Videos\\custom_java\\conf\\security\\config.policy";

	private static readonly string ASCII_ART = "\r\n  ░██╗░░░░░░░██╗██╗██╗░░██╗░██████╗██╗\r\n  ░██║░░██╗░░██║██║██║░██╔╝██╔════╝██║\r\n  ░╚██╗████╗██╔╝██║█████═╝░╚█████╗░██║\r\n  ░░████╔═████║░██║██╔═██╗░░╚═══██╗██║    \r\n  ░░╚██╔╝░╚██╔╝░██║██║░╚██╗██████╔╝██║\r\n  ░░░╚═╝░░░╚═╝░░╚═╝╚═╝░░╚═╝╚═════╝░╚═╝\r\n\r\n   Loader [wiksiclient.fun]\r\n";

	private static async Task Main(string[] args)
	{
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(ASCII_ART);
		Console.Title = "Wiksi Client - скажи перхоти көзіме көрінбейтін бол э, түсіндің ба";
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.Write("  root@wiksi:~# Введите айди пользователя: ");
		string username = Console.ReadLine();
		Console.Write("  root@wiksi:~# Введите свой пароль: ");
		string password = Console.ReadLine();
		dynamic val = await LoginUser(username, password);
		if (val == null || val.status != "success")
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"  root@wiksi:~# Ошибка авторизации: {(object?)val?.message}");
			Console.ResetColor();
			return;
		}
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("  root@wiksi:~# Авторизация прошла успешно!");
		Console.ResetColor();
		if (!(await CheckSubscriptionStatus(username)))
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("  root@wiksi:~# Подписка не активна. Пожалуйста, приобретите подписку.");
			Console.ResetColor();
			return;
		}
		string hwid = GetHWID();
		Console.WriteLine("  root@wiksi:~# Ваш HWID: " + hwid);
		dynamic val2 = await UpdateHWID(username, hwid);
		if (val2 == null || val2.status != "success")
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"  root@wiksi:~# Ошибка обновления HWID: {(object?)val2?.message}");
			Console.ResetColor();
			return;
		}
		dynamic val3 = await CheckHWID(username, hwid);
		if (val3 == null || val3.status != "success")
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"  root@wiksi:~# Ошибка проверки HWID: {(object?)val3?.message}");
			Console.ResetColor();
			return;
		}
		Console.Clear();
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.WriteLine(ASCII_ART);
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(" ");
		Console.WriteLine("  info@wiksi:~# Ваш хвид:");
		Console.WriteLine(" ");
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine("   " + hwid);
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(" ");
		Console.WriteLine("  info@wiksi:~# Подписка активна до: ");
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(" ");
		Console.WriteLine("   01.01.2077 (13:37 по МСК)");
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(" ");
		Console.ForegroundColor = ConsoleColor.Blue;
		Console.Write("  root@wiksi:~# Введите кол-во выделяемой озу (в МБ): ");
		if (!int.TryParse(Console.ReadLine(), out var memory) || memory <= 0)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("  root@wiksi:~# Ошибка: Некорректный объем памяти. Пожалуйста, введите число в мегабайтах.");
			Console.ResetColor();
			Task.Delay(15000).Wait();
			return;
		}
		if (!Directory.Exists(FROZEN_PATH))
		{
			Directory.CreateDirectory(FROZEN_PATH);
		}
		SaveLoginToFile(username);
		if (!Directory.Exists(Path.Combine(FROZEN_PATH, "libraries")))
		{
			string zipPath = Path.Combine(FROZEN_PATH, "client.zip");
			if (!(await DownloadFile(CLIENT_ZIP_URL, zipPath)) || !ExtractZipFile(zipPath, FROZEN_PATH))
			{
				return;
			}
			File.Delete(zipPath);
		}
		string jarPath = Path.Combine(FROZEN_PATH, "client.jar");
		if (!File.Exists(jarPath) && !(await DownloadFile(JAR_FILE_URL, jarPath)))
		{
			return;
		}
		string text = Path.Combine(FROZEN_PATH, "custom_java", "bin", "java.exe");
		if (!File.Exists(text))
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("[X] Ошибка: Файл " + text + " не найден.");
			Console.ResetColor();
			return;
		}
		string[] files = Directory.GetFiles(Path.Combine(FROZEN_PATH, "libraries"), "*.jar");
		string text2 = Path.Combine(FROZEN_PATH, "natives");
		if (!Directory.Exists(text2))
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("[X] Ошибка: Папка " + text2 + " не найдена.");
			Console.ResetColor();
			return;
		}
		string item = jarPath + ";" + string.Join(";", files);
		List<string> values = new List<string>
		{
			"-Xmx" + memory + "M",
			"-Djava.library.path=" + text2,
			"-cp",
			item,
			"net.minecraft.client.main.Main",
			"--version",
			"1.16",
			"--username",
			username,
			"--accessToken",
			"0",
			"--assetsDir",
			Path.Combine(FROZEN_PATH, "assets"),
			"--assetIndex",
			"1.16",
			"--userProperties",
			"{}"
		};
		try
		{
			ProcessStartInfo startInfo = new ProcessStartInfo
			{
				FileName = text,
				Arguments = string.Join(" ", values),
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};
			using Process process = new Process
			{
				StartInfo = startInfo
			};
			process.Start();
			process.StandardOutput.ReadToEnd();
			string text3 = process.StandardError.ReadToEnd();
			process.WaitForExit();
			if (process.ExitCode != 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("  root@wiksi:~# Ошибка при запуске Wiksi Release: " + text3);
				Console.ResetColor();
				Task.Delay(15000).Wait();
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("  root@wiksi:~# Вы успешно закрыли Wiksi Client");
				Console.ResetColor();
			}
		}
		catch (Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("  root@wiksi:~# Ошибка при запуске Wiksi Release: " + ex.Message);
			Console.ResetColor();
			Task.Delay(15000).Wait();
		}
	}

	private static async Task<dynamic> LoginUser(string username, string password)
	{
		using HttpClient client = new HttpClient();
		FormUrlEncodedContent content = new FormUrlEncodedContent(new KeyValuePair<string, string>[2]
		{
			new KeyValuePair<string, string>("username", username),
			new KeyValuePair<string, string>("password", password)
		});
		return JsonConvert.DeserializeObject<object>(await (await client.PostAsync(SERVER_URL + "/api/login", content)).Content.ReadAsStringAsync());
	}

	private static async Task<bool> CheckSubscriptionStatus(string username)
	{
		using HttpClient client = new HttpClient();
		FormUrlEncodedContent content = new FormUrlEncodedContent(new KeyValuePair<string, string>[1]
		{
			new KeyValuePair<string, string>("username", username)
		});
		dynamic val = JsonConvert.DeserializeObject<object>(await (await client.PostAsync(SERVER_URL + "/api/check_subscription", content)).Content.ReadAsStringAsync());
		if (val.status == "success" && val.is_activated == true)
		{
			return true;
		}
		return false;
	}

	private static async Task<dynamic> UpdateHWID(string username, string hwid)
	{
		using HttpClient client = new HttpClient();
		FormUrlEncodedContent content = new FormUrlEncodedContent(new KeyValuePair<string, string>[2]
		{
			new KeyValuePair<string, string>("username", username),
			new KeyValuePair<string, string>("hwid", hwid)
		});
		return JsonConvert.DeserializeObject<object>(await (await client.PostAsync(SERVER_URL + "/api/update_hwid", content)).Content.ReadAsStringAsync());
	}

	private static async Task<dynamic> CheckHWID(string username, string hwid)
	{
		using HttpClient client = new HttpClient();
		FormUrlEncodedContent content = new FormUrlEncodedContent(new KeyValuePair<string, string>[2]
		{
			new KeyValuePair<string, string>("username", username),
			new KeyValuePair<string, string>("hwid", hwid)
		});
		return JsonConvert.DeserializeObject<object>(await (await client.PostAsync(SERVER_URL + "/api/check_hwid", content)).Content.ReadAsStringAsync());
	}

	private static string GetHWID()
	{
		string hardwareInfo = GetHardwareInfo("Win32_BaseBoard", "Product");
		string hardwareInfo2 = GetHardwareInfo("Win32_Processor", "ProcessorId");
		string hardwareInfo3 = GetHardwareInfo("Win32_PhysicalMedia", "SerialNumber");
		return GetSHA256Hash(hardwareInfo + hardwareInfo2 + hardwareInfo3).Substring(0, 20);
	}

	private static string GetHardwareInfo(string className, string propertyName)
	{
		ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT " + propertyName + " FROM " + className);
		try
		{
			foreach (ManagementBaseObject item in managementObjectSearcher.Get())
			{
				if (item[propertyName] != null)
				{
					return item[propertyName].ToString();
				}
			}
		}
		finally
		{
			((IDisposable)managementObjectSearcher)?.Dispose();
		}
		return string.Empty;
	}

	private static string GetSHA256Hash(string input)
	{
		using SHA256 sHA = SHA256.Create();
		byte[] array = sHA.ComputeHash(Encoding.UTF8.GetBytes(input));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	private static async Task<bool> DownloadFile(string url, string destinationPath)
	{
		_ = 3;
		try
		{
			using HttpClient client = new HttpClient();
			HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
			response.EnsureSuccessStatusCode();
			using (Stream contentStream = await response.Content.ReadAsStreamAsync())
			{
				long num = response.Content.Headers.ContentLength ?? (-1);
				long bytesRead = 0L;
				byte[] buffer = new byte[8192];
				bool isMoreToRead = true;
				using FileStream fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, useAsync: true);
				Console.ForegroundColor = ConsoleColor.Green;
				using ProgressBar progressBar = new ProgressBar((int)num, "  root@wiksi:~# Процесс установки библиотек", new ProgressBarOptions());
				do
				{
					int read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
					if (read == 0)
					{
						isMoreToRead = false;
						continue;
					}
					await fileStream.WriteAsync(buffer, 0, read);
					bytesRead += read;
					progressBar.Tick((int)bytesRead);
				}
				while (isMoreToRead);
			}
			return true;
		}
		catch (Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("\n  root@wiksi:~# Ошибка при скачивании файла: " + ex.Message);
			Console.ResetColor();
			return false;
		}
	}

	private static bool ExtractZipFile(string zipPath, string extractPath)
	{
		try
		{
			ZipFile.ExtractToDirectory(zipPath, extractPath);
			return true;
		}
		catch (Exception ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("  root@wiksi:~# Ошибка при распаковке файла: " + ex.Message);
			Console.ResetColor();
			return false;
		}
	}

	private static void SaveLoginToFile(string username)
	{
		string directoryName = Path.GetDirectoryName(LOGIN_FILE_PATH);
		if (!Directory.Exists(directoryName))
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("  root@wiksi:~# Директория не найдена. Создаем необходимые директории...");
			try
			{
				Directory.CreateDirectory(directoryName);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("  root@wiksi:~# Директории успешно созданы!");
			}
			catch (Exception)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("  root@wiksi:~# Ошибка при создании директорий.");
				Console.ResetColor();
				return;
			}
		}
		if (!File.Exists(LOGIN_FILE_PATH))
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("  root@wiksi:~# Файл с ключом не найден. Создаем новый файл...");
			try
			{
				string contents = JsonConvert.SerializeObject(new
				{
					Username = username
				}, Formatting.Indented);
				File.WriteAllText(LOGIN_FILE_PATH, contents);
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("  root@wiksi:~# Файл с ключом успешно создан и ваш ключ сохранен!");
			}
			catch (Exception)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("  root@wiksi:~# Ошибка при создании ключа.");
				Console.ResetColor();
				return;
			}
		}
		Console.ResetColor();
	}
}
