// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Security.Cryptography;
using System.Text.Json;
using KeySharp;

namespace MineOrc.Security.Storage;

public sealed class SecureStorageService
{
    private readonly string _fileName;
    private readonly string _serviceName;

    private readonly Aes _aes = Aes.Create();

    public SecureStorageService(string serviceName, string fileName)
    {
        _serviceName = serviceName;
        _fileName = fileName;
    }

    private void LoadKeyRing()
    {
        var keyring = GetKeyRing();
        var key = Convert.FromBase64String(keyring.Key);
        var iv = Convert.FromBase64String(keyring.InitializationVector);

        _aes.Key = key;
        _aes.IV = iv;
    }

    private SecureStorageKeyRing GetKeyRing()
    {
        var secret = Keyring.GetPassword(_serviceName, _serviceName, _serviceName);
        if (secret == null)
        {
            return CreateKeyRing();
        }

        return JsonSerializer.Deserialize(secret,
            SecureStorageJsonContext.Default.SecureStorageKeyRing)!;
    }

    private SecureStorageKeyRing CreateKeyRing()
    {
        _aes.GenerateKey();
        _aes.GenerateIV();

        var keyRing = new SecureStorageKeyRing(Convert.ToBase64String(_aes.Key),
            Convert.ToBase64String(_aes.IV));

        Keyring.SetPassword(_serviceName,
            _serviceName,
            _serviceName,
            JsonSerializer.Serialize(keyRing,
                SecureStorageJsonContext.Default.SecureStorageKeyRing));

        return keyRing;
    }

    public Stream OpenRead()
    {
        LoadKeyRing();

        return new CryptoStream(File.OpenRead(_fileName),
            _aes.CreateDecryptor(),
            CryptoStreamMode.Read);
    }

    public Stream Create()
    {
        LoadKeyRing();

        return new CryptoStream(File.Create(_fileName),
            _aes.CreateEncryptor(),
            CryptoStreamMode.Write);
    }
}