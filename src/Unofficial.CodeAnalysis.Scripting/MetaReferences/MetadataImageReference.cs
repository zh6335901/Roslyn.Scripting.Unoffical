using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Text;

namespace Unofficial.CodeAnalysis.Scripting.MetaReferences
{
    /// <summary>
    /// Represents an in-memory Portable-Executable image.
    /// </summary>
    [DebuggerDisplay("{GetDebuggerDisplay(), nq}")]
    internal sealed class MetadataImageReference : PortableExecutableReference
    {
        private readonly string? _display;
        private readonly Metadata _metadata;

        internal MetadataImageReference(Metadata metadata, MetadataReferenceProperties properties, DocumentationProvider? documentation, string? filePath, string? display)
            : base(properties, filePath, documentation ?? DocumentationProvider.Default)
        {
            _display = display;
            _metadata = metadata;
        }

        protected override Metadata GetMetadataImpl()
        {
            return _metadata;
        }

        protected override DocumentationProvider CreateDocumentationProvider()
        {
            // documentation provider is initialized in the constructor
            throw new InvalidOperationException($"This program location is thought to be unreachable.");
        }

        protected override PortableExecutableReference WithPropertiesImpl(MetadataReferenceProperties properties)
        {
            return new MetadataImageReference(
                _metadata,
                properties,
                this.CreateDocumentationProvider(),
                this.FilePath,
                _display);
        }

        public override string Display
        {
            get
            {
                return _display ?? FilePath ?? (Properties.Kind == MetadataImageKind.Assembly ? "InMemoryAssembly" : "InMemoryModule");
            }
        }

        private string GetDebuggerDisplay()
        {
            var sb = new StringBuilder();
            sb.Append(Properties.Kind == MetadataImageKind.Module ? "Module" : "Assembly");
            if (!Properties.Aliases.IsEmpty)
            {
                sb.Append(" Aliases={");
                sb.Append(string.Join(", ", Properties.Aliases));
                sb.Append('}');
            }

            if (Properties.EmbedInteropTypes)
            {
                sb.Append(" Embed");
            }

            if (FilePath != null)
            {
                sb.Append(" Path='");
                sb.Append(FilePath);
                sb.Append('\'');
            }

            if (_display != null)
            {
                sb.Append(" Display='");
                sb.Append(_display);
                sb.Append('\'');
            }

            return sb.ToString();
        }
    }
}
