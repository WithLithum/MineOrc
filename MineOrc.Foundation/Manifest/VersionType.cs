using System.Text.Json.Serialization;
using MineOrc.Foundation.Json;

namespace MineOrc.Foundation.Manifest;

/// <summary>
/// Specifies the type of version release.
/// </summary>
[JsonConverter(typeof(VersionTypeConverter))]
public enum VersionType
{
    /// <summary>
    /// Indicates a release version.
    /// </summary>
    Release,
    /// <summary>
    /// Indicates a development version, including snapshots, pre-release versions and release
    /// candidate versions.
    /// </summary>
    Snapshot,
    /// <summary>
    /// Indicates an unobfuscated version for testing purposes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This version type is briefly used during the transition period between obfuscated and
    /// unobfuscated binary files, at the late stage of the Mounts of Mayhem drop.
    /// </para>
    /// <para>
    /// Versions of this type is not added into the version manifest but instead has a zip file
    /// containing a <see cref="ClientManifest"/> that can extracted to <c>versions</c> directory
    /// for the vanilla Minecraft Launcher to load. 
    /// </para>
    /// </remarks>
    Unobfuscated,
    /// <summary>
    /// Indicates a version that belongs to the Beta development phase.
    /// </summary>
    /// <seealso href="https://minecraft.wiki/w/Java_Edition_Beta"/>
    OldBeta,
    /// <summary>
    /// Indicates a version that belongs to, or is older than, the Alpha development phase.
    /// </summary>
    /// <remarks>
    /// This version type covers from the pre-Classic releases all the way up to the final release
    /// in the Alpha development phase.
    /// </remarks>
    OldAlpha
}