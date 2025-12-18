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

    private string GetProfileDirectory(string profileName)
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

    /// <summary>
    /// Gets the specified profile.
    /// </summary>
    /// <param name="profileName">The name of the profile to acquire.</param>
    /// <returns>The profile, or <see langword="null"/> if it does not exist.</returns>
    /// <exception cref="ProfileException">
    /// The contents of the profile configuration file is <c>null</c>.
    /// </exception>
    public async Task<ProfileInstance?> GetProfileOrDefaultAsync(string profileName)
    {
        ThrowIfInvalidProfileName(profileName);

        var profileDir = Path.GetFullPath(profileName,
            _baseDirectory);
        var profileFile = Path.Combine(profileDir,
            ProfileJsonName);

        if (!File.Exists(profileFile))
        {
            return null;
        }

        var profile = await ReadProfileOrDefaultInternalAsync(profileFile)
                          .ConfigureAwait(false)
                      ?? throw new ProfileException(ExceptionMessages.ProfileConfigNull);

        return new ProfileInstance(profileDir, profileFile, profile);
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