using System.Collections.Immutable;
using System.Globalization;
using System.Reflection;

namespace Unofficial.CodeAnalysis.Scripting.Hosting.Resolvers
{
    /// <summary>
    /// Resolves assembly identities in Global Assembly Cache.
    /// </summary>
    internal sealed class GacFileResolver : IEquatable<GacFileResolver>
    {
        /// <summary>
        /// Returns true if GAC is available on the current platform.
        /// </summary>
        public static bool IsAvailable
        {
            get
            {
                try
                {
                    return !(Type.GetType("Mono.Runtime") is null);
                }
                catch
                {
                    // Arbitrarily assume we're not running on Mono.
                    return false;
                }
            }
        }

        /// <summary>
        /// Architecture filter used when resolving assembly references.
        /// </summary>
        public ImmutableArray<ProcessorArchitecture> Architectures { get; }

        /// <summary>
        /// <see cref="CultureInfo"/> used when resolving assembly references, or null to prefer no culture.
        /// </summary>
        public CultureInfo PreferredCulture { get; }

        /// <summary>
        /// Creates an instance of a <see cref="GacFileResolver"/>, if available on the platform (check <see cref="IsAvailable"/>).
        /// </summary>
        /// <param name="architectures">Supported architectures used to filter GAC assemblies.</param>
        /// <param name="preferredCulture">A culture to use when choosing the best assembly from
        /// among the set filtered by <paramref name="architectures"/></param>
        /// <exception cref="PlatformNotSupportedException">The platform doesn't support GAC.</exception>
        public GacFileResolver(
            ImmutableArray<ProcessorArchitecture> architectures = default,
            CultureInfo preferredCulture = null)
        {
            if (!IsAvailable)
            {
                throw new PlatformNotSupportedException();
            }

            if (architectures.IsDefault)
            {
                architectures = (IntPtr.Size == 4)
                    ? [ProcessorArchitecture.None, ProcessorArchitecture.MSIL, ProcessorArchitecture.X86]
                    : [ProcessorArchitecture.None, ProcessorArchitecture.MSIL, ProcessorArchitecture.Amd64];
            }

            Architectures = architectures;
            PreferredCulture = preferredCulture;
        }

        public string? Resolve(string assemblyName)
        {
            return null;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(PreferredCulture);
            foreach (var architecture in Architectures)
            {
                hash.Add(architecture);
            }

            return hash.ToHashCode();
        }

        public bool Equals(GacFileResolver? other)
        {
            return ReferenceEquals(this, other) ||
                other != null &&
                Architectures.SequenceEqual(other.Architectures) &&
                PreferredCulture == other.PreferredCulture;
        }

        public override bool Equals(object? obj) => Equals(obj as GacFileResolver);
    }
}
