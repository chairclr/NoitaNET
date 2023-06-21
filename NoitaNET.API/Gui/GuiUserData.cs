namespace NoitaNET.API.Gui;

public struct GuiUserData
{
    internal nint InternalPointer;

    internal GuiUserData(nint internalPointer)
    {
        InternalPointer = internalPointer;
    }
}
