using System.Diagnostics;
using System.Globalization;

namespace NoitaNET.API.Hooks;

public static class PatternScanner
{
    public static unsafe nint Scan(ProcessModule module, string pattern)
    {
        pattern = pattern.Trim().ReplaceLineEndings(" ");

        string[] splits = pattern.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        bool[] mask = new bool[splits.Length];
        byte[] bytePattern = new byte[splits.Length];

        for (int i = 0; i < splits.Length; i++)
        {
            string s = splits[i];
            if (s.Length != 2)
            {
                throw new ArgumentException($"Invalid pattern byte at {i}", pattern);
            }

            if (s == "??")
            {
                mask[i] = true;
            }
            else
            {
                if (byte.TryParse(s, NumberStyles.HexNumber, null, out byte result))
                {
                    bytePattern[i] = result;
                }
                else
                {
                    throw new ArgumentException($"Invalid pattern byte at {i}", pattern);
                }
            }
        }

        return Scan(module, mask, bytePattern);
    }

    public static unsafe nint Scan(ProcessModule module, bool[] mask, byte[] bytePattern)
    {
        nint baseAddress = module.BaseAddress;
        nint endAddress = module.BaseAddress + module.ModuleMemorySize;

        for (nint i = baseAddress; i < endAddress; i++)
        {
            try
            {
                Span<byte> read = new Span<byte>((void*)i, bytePattern.Length);

                bool found = false;

                for (int j = 0; j < read.Length; j++)
                {
                    found = mask[j] || read[j] == bytePattern[j];

                    if (!found)
                        break;
                }

                if (found)
                {
                    return i;
                }
            }
            catch
            {

            }
        }

        return -1;
    }

    public static unsafe List<nint> ScanMulitple(ProcessModule module, string pattern)
    {
        pattern = pattern.Trim().ReplaceLineEndings(" ");

        string[] splits = pattern.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        bool[] mask = new bool[splits.Length];
        byte[] bytePattern = new byte[splits.Length];

        for (int i = 0; i < splits.Length; i++)
        {
            string s = splits[i];
            if (s.Length != 2)
            {
                throw new ArgumentException($"Invalid pattern byte at {i}", pattern);
            }

            if (s == "??")
            {
                mask[i] = true;
            }
            else
            {
                if (byte.TryParse(s, NumberStyles.HexNumber, null, out byte result))
                {
                    bytePattern[i] = result;
                }
                else
                {
                    throw new ArgumentException($"Invalid pattern byte at {i}", pattern);
                }
            }
        }

        return ScanMultiple(module, mask, bytePattern);
    }

    public static unsafe List<nint> ScanMultiple(ProcessModule module, bool[] mask, byte[] bytePattern)
    {
        nint baseAddress = module.BaseAddress;
        nint endAddress = module.BaseAddress + module.ModuleMemorySize;

        List<nint> foundAddresses = new List<nint>();

        for (nint i = baseAddress; i < endAddress; i++)
        {
            try
            {
                Span<byte> read = new Span<byte>((void*)i, bytePattern.Length);

                bool found = false;

                for (int j = 0; j < read.Length; j++)
                {
                    found = mask[j] || read[j] == bytePattern[j];

                    if (!found)
                        break;
                }

                if (found)
                {
                    foundAddresses.Add(i);
                }
            }
            catch
            {

            }
        }

        return foundAddresses;
    }
}
