// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using Unofficial.CodeAnalysis.Scripting.Hosting;

namespace Unofficial.CodeAnalysis.CSharp.Scripting.Hosting
{
    internal class CSharpMemberFilter : CommonMemberFilter
    {
        protected override bool IsGeneratedMemberName(string name)
        {
            // Generated fields, e.g. "<property_name>k__BackingField"
            return name.Length > 0 && name[0] == '<';
        }
    }
}
