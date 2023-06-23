using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Intrinsics;
using System.Text;
using System.Runtime.CompilerServices;

namespace NoitaNET.API;

[StructLayout(LayoutKind.Explicit)]
public struct MessageListenerPassthroughData
{
    [FieldOffset(0)]
    public ulong Data;

    [FieldOffset(0)]
    public nint Function;

    [FieldOffset(4)]
    public int ModObjectIndex;

    public Mod ModObject => Mod.Mods[ModObjectIndex];

    public unsafe MessageListenerPassthroughData(nint function, Mod modObject)
    {
        Function = function;
        ModObjectIndex = Mod.Mods.IndexOf(modObject);
    }

    public MessageListenerPassthroughData(Span<byte> chars)
    {
        if (Sse2.IsSupported)
        {
            Data = SseUnescapeData(chars);
        }
        else
        {
            Data = RegularUnescapeData(chars);
        }
    }

    public void CopyToString(Span<byte> stringBytes)
    {
        if (Sse2.IsSupported)
        {
            SseEscapeData(stringBytes, Data);
        }
        else
        {
            RegularEscapeData(stringBytes, Data);
        }
    }

    private static void RegularEscapeData(Span<byte> output, ulong data)
    {
        const ulong Mask = 0x0F0F0F0F0F0F0F0F;

        ulong data1 = (data << 4) | Mask;
        ulong data2 = data | Mask;

        Unsafe.As<byte, ulong>(ref output[0]) = data1;
        Unsafe.As<byte, ulong>(ref output[1]) = data2;
    }

    private unsafe static void SseEscapeData(Span<byte> output, ulong data)
    {
        Vector128<ulong> mask = Vector128.Create((byte)0x0F).AsUInt64();

        Vector128<ulong> bytes = Vector128.Create(data, 0);

        bytes = Sse2.ShiftLeftLogical(bytes, 4)
            .WithElement(1, data);

        bytes = Sse2.Or(bytes, mask);

        bytes.StoreUnsafe(ref Unsafe.As<byte, ulong>(ref output[0]));
    }

    private unsafe static ulong RegularUnescapeData(Span<byte> data)
    {
        const ulong Mask = 0x0F0F0F0F0F0F0F0F;

        ref ulong lower = ref Unsafe.As<byte, ulong>(ref data[0]);
        ulong upper = Unsafe.Add(ref lower, 1);

        return ((lower & Mask) >> 4) | (upper & Mask);
    }

    private unsafe static ulong SseUnescapeData(Span<byte> data)
    {
        Vector128<byte> mask = Vector128.Create((byte)0xF0);

        Vector128<byte> input = Unsafe.As<byte, Vector128<byte>>(ref data[0]);

        Vector128<byte> unmaskedInput = Sse2.And(input, mask);

        ref ulong lower = ref Unsafe.As<Vector128<byte>, ulong>(ref unmaskedInput);
        ulong upper = Unsafe.Add(ref lower, 1);

        return (lower >> 4) | upper;
    }
}
