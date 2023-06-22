using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace NoitaNET.Loader.Symbols;

internal static class Demangler
{
    public unsafe static string GetSymbolNameFromRTTI(nint RTTIAddress)
    {
        TypeDescriptor* typeDescriptor = (TypeDescriptor*)RTTIAddress;
        string name = typeDescriptor->Name;
        return DemangleSymbol(name);
    }

    public static string DemangleSymbol(string mangledName)
    {
        if (mangledName[4] == '?')
        {
            mangledName = mangledName[1..];
        }
        else if (mangledName[0] == '.')
        {
            mangledName = mangledName[4..];
        }
        else if (mangledName[0] == '?')
        {
            mangledName = mangledName[2..];
        }
        else
        {
            throw new Exception();
        }

        // * 3 is a good enough estimate?
        byte[] buffer = ArrayPool<byte>.Shared.Rent(mangledName.Length * 3);

        int bytesWritten = UnDecorateSymbolName($"??_7{mangledName}6B@", buffer, buffer.Length, UnDecorateFlags.UNDNAME_COMPLETE);

        if (bytesWritten == 0)
        {
            throw new Exception();
        }

        string s = Encoding.UTF8.GetString(buffer.AsSpan().Slice(0, bytesWritten));

        ArrayPool<byte>.Shared.Return(buffer);

        if (s.StartsWith("const "))
        {
            s = s.Remove(0, 6);
        }

        s = s
            .Replace("::`vftable'", "")
            .Replace("::`anonymous namespace'", "");

        return s;
    }

    [Flags]
    private enum UnDecorateFlags
    {
        UNDNAME_COMPLETE = (0x0000),  // Enable full undecoration
        UNDNAME_NO_LEADING_UNDERSCORES = (0x0001),  // Remove leading underscores from MS extended keywords
        UNDNAME_NO_MS_KEYWORDS = (0x0002),  // Disable expansion of MS extended keywords
        UNDNAME_NO_FUNCTION_RETURNS = (0x0004),  // Disable expansion of return type for primary declaration
        UNDNAME_NO_ALLOCATION_MODEL = (0x0008),  // Disable expansion of the declaration model
        UNDNAME_NO_ALLOCATION_LANGUAGE = (0x0010),  // Disable expansion of the declaration language specifier
        UNDNAME_NO_MS_THISTYPE = (0x0020),  // NYI Disable expansion of MS keywords on the 'this' type for primary declaration
        UNDNAME_NO_CV_THISTYPE = (0x0040),  // NYI Disable expansion of CV modifiers on the 'this' type for primary declaration
        UNDNAME_NO_THISTYPE = (0x0060),  // Disable all modifiers on the 'this' type
        UNDNAME_NO_ACCESS_SPECIFIERS = (0x0080),  // Disable expansion of access specifiers for members
        UNDNAME_NO_THROW_SIGNATURES = (0x0100),  // Disable expansion of 'throw-signatures' for functions and pointers to functions
        UNDNAME_NO_MEMBER_TYPE = (0x0200),  // Disable expansion of 'static' or 'virtual'ness of members
        UNDNAME_NO_RETURN_UDT_MODEL = (0x0400),  // Disable expansion of MS model for UDT returns
        UNDNAME_32_BIT_DECODE = (0x0800),  // Undecorate 32-bit decorated names
        UNDNAME_NAME_ONLY = (0x1000),  // Crack only the name for primary declaration;
                                       // return just [scope::]name.  Does expand template params
        UNDNAME_NO_ARGUMENTS = (0x2000),  // Don't undecorate arguments to function
        UNDNAME_NO_SPECIAL_SYMS = (0x4000),  // Don't undecorate special names (v-table, vcall, vector xxx, metatype, etc)
    }

    [DllImport("dbghelp.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Ansi)]
    private static extern int UnDecorateSymbolName(string decoratedName, byte[] undecoratedName, int undecoratedLength, UnDecorateFlags flags);

    public unsafe ref struct TypeDescriptor
    {
        public nint VFTable;
        private nint __reserved;
        public byte __name;

        public string Name => Marshal.PtrToStringUTF8((nint)Unsafe.AsPointer(ref __name))!;
    };
}
