using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services;

public class UserService
{
    private const string FilePath = "Config/users.securejson";
    private const string EncryptionKey = "MEnkkThuKpXdh3.fv@R7FnHjNtuzPmCQ"; // 32 Zeichen für AES-256

    private List<AppUser> _userList = new();


    public IReadOnlyList<AppUser> Users => _userList;
    public void AddUser(AppUser user)
    {
        _userList.Add(user);
    }


    public void LoadUsers()
    {
        if (!File.Exists(FilePath))
        {
            _userList = new List<AppUser>();
            return;
        }

        var encryptedData = File.ReadAllBytes(FilePath);
        var json = Decrypt(encryptedData, GetEncryptionKey());
        _userList = JsonSerializer.Deserialize<List<AppUser>>(json) ?? new List<AppUser>();
    }

    public void SaveUsers()
    {
        var json = JsonSerializer.Serialize(_userList, new JsonSerializerOptions { WriteIndented = true });
        var encrypted = Encrypt(json, GetEncryptionKey());
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        File.WriteAllBytes(FilePath, encrypted);
    }

    public AppUser GetOrCreateUser(string username)
    {
        var user = _userList.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        if (user == null)
        {
            user = new AppUser { Username = username };
            _userList.Add(user);
            SaveUsers();
        }
        return user;
    }

    public IEnumerable<AppUser> GetUnconfirmedUsers()
        => _userList.Where(u => !u.IsConfirmed);

    private static string GetEncryptionKey()
    {
        var keyPath = Path.Combine("Config", "user.key.txt");
        if (!File.Exists(keyPath))
            throw new FileNotFoundException("Verschlüsselungs-Schlüssel wurde nicht gefunden (Config/user.key.txt).");

        var key = File.ReadAllText(keyPath).Trim();

        if (key.Length != 32)
            throw new InvalidOperationException("Der Verschlüsselungsschlüssel muss exakt 32 Zeichen lang sein (AES-256).");

        return key;
    }

    #region AES Crypto

    private static byte[] Encrypt(string plainText, string key)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        aes.GenerateIV();
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        using var ms = new MemoryStream();
        ms.Write(iv, 0, iv.Length);
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
            sw.Write(plainText);

        return ms.ToArray();
    }

    private static string Decrypt(byte[] cipherData, string key)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(key);
        var iv = new byte[16];
        Array.Copy(cipherData, iv, 16);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipherData, 16, cipherData.Length - 16);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }

    #endregion
}
