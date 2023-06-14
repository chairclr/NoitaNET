using NoitaNET.API;

namespace NoitaNET.TestMod;

[ModEntry]
public class TestMod : Mod
{
    public TestMod()
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Hello from TestMod");
    }
}