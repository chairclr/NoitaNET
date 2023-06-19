using System.Linq;
using Microsoft.CodeAnalysis;

namespace NoitaNET.API.Generators;

internal static class Util
{
    public static INamedTypeSymbol? GetMemberTypeByMetadataName(this Compilation compilation, string fullyQualifiedParentType, string memberName)
    {
        return compilation.GetTypeByMetadataName(fullyQualifiedParentType)?.GetTypeMembers().FirstOrDefault(x => x.MetadataName == memberName);
    }
}
