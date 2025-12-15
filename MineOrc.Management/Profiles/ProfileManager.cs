// SPDX-FileCopyrightText: 2025 WithLithum & contributors
// SPDX-License-Identifier: GPL-3.0-or-later

using System.Text.Json;
using System.Text.RegularExpressions;
using MineOrc.Foundation.Instancing;
using MineOrc.Foundation.Json;
using MineOrc.Instancing;
using MineOrc.Management.Resources;

namespace MineOrc.Management.Profiles;

public sealed partial class ProfileManager
{
    private const string ProfileJsonName = "profile.json";
    public static readonly Regex ProfileNameRegex = GenerateProfileNameRegex();

    private readonly string _baseDirectory;

    public ProfileManager(string baseDirectory)
    {
        _baseDirectory = baseDirectory;
    }

    private static void ThrowIfInvalidProfileName(string profileName)
    {
        if (!ProfileNameRegex.IsMatch(profileName))
        {
            throw new ArgumentException(ExceptionMessages.ProfileNameInvalid,
                nameof(profileName));
        }
    }

    public string GetProfileDirectory(string profileName)
    {
        ThrowIfInvalidProfileName(profileName);

        return Path.GetFullPath(profileName,
            _baseDirectory);
    }

    public bool HasProfile(string profileName)
    {
        return File.Exists(Path.Combine(GetProfileDirectory(profileName),
            ProfileJsonName));
    }

    public async Task CreateProfileAsync(string profileName,
        ProfileInfo info)
    {
        ThrowIfInvalidProfileName(profileName);

        var profileDir = GetProfileDirectory(profileName);
        var profileFile = Path.Combine(profileDir,
            ProfileJsonName);

        // Make sure it does not exist
        if (HasProfile(profileName))
        {
            throw new ProfileException(string.Format(ExceptionMessages.ProfileExists,
                profileName));
        }

        Directory.CreateDirectory(profileDir);

        // Officially save the profile
        var stream = File.Create(profileFile);
        await using (stream.ConfigureAwait(false))
        {
            await JsonSerializer.SerializeAsync(stream,
                info,
                MineOrcFilesJsonContext.Default.ProfileInfo).ConfigureAwait(false);
        }
    }

    public async Task UpdateProfileAsync(string profileName, ProfileInfo info)
    {
        ThrowIfInvalidProfileName(profileName);

        if (!HasProfile(profileName))
        {
            throw new InvalidOperationException(string.Format(ExceptionMessages.ProfileNotExists,
                profileName));
        }
        
        var profileDir = GetProfileDirectory(profileName);
        var profileFile = Path.Combine(profileDir,
            ProfileJsonName);
        
        // Officially save the profile
        var stream = File.Create(profileFile);
        await using (stream.ConfigureAwait(false))
        {
            await JsonSerializer.SerializeAsync(stream,
                info,
                MineOrcFilesJsonContext.Default.ProfileInfo).ConfigureAwait(false);
        }
    }

    public async Task<ProfileInfo> ReadProfileAsync(string profileName)
    {
        ThrowIfInvalidProfileName(profileName);

        var profileDir = Path.GetFullPath(profileName,
            _baseDirectory);
        var profileFile = Path.Combine(profileDir,
            ProfileJsonName);

        var profile = await ReadProfileOrDefaultInternalAsync(profileFile)
            .ConfigureAwait(false);

        return profile ?? throw new ProfileException(string.Format(ExceptionMessages.ProfileConfigNull,
            profileName));
    }

    private static async Task<ProfileInfo?> ReadProfileOrDefaultInternalAsync(string profileFile)
    {
        var stream = File.OpenRead(profileFile);
        await using (stream.ConfigureAwait(false))
        {
            return await JsonSerializer.DeserializeAsync(stream,
                MineOrcFilesJsonContext.Default.ProfileInfo).ConfigureAwait(false);
        }
    }

    [GeneratedRegex(@"^[A-Za-z0-9_\-]+$")]
    private static partial Regex GenerateProfileNameRegex();
}