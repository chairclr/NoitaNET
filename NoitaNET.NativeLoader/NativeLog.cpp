#include "NativeLog.h"

static std::ofstream OutputFileStream;

bool NativeLog::Init()
{
    std::string logFile = Entry::GetDllRootDirectory() + "\\Coolua.NativeLoader_logs.txt";

    OutputFileStream = std::ofstream(logFile);

    if (OutputFileStream.is_open())
    {
        HasOutputFileStream = true;
    }

    LogInformation("Initializing native console logging");

    if (!AllocConsole())
    {
        LogError("Failed to allocate console");
        return false;
    }

    FILE* _;
    if (freopen_s(&_, "CONIN$", "r", stdin))
    {
        LogError("Failed to open stdin");
        return false;
    }

    if (freopen_s(&_, "CONOUT$", "w", stdout))
    {
        LogError("Failed to open stdout");
        return false;
    }

    if (freopen_s(&_, "CONOUT$", "w", stderr))
    {
        LogError("Failed to open stderr");
        return false;
    }

    ConsoleOutputHandle = GetStdHandle(STD_OUTPUT_HANDLE);

    HasAllocatedConsole = true;

    if (!HasOutputFileStream)
    {
        LogError("Failed to open log file");
    }

    return true;
}

void NativeLog::LogInformation(const std::string& message)
{
    if (HasOutputFileStream)
    {
        OutputFileStream << "[Info]    " << message << "\n";
        OutputFileStream.flush();
    }

    if (HasAllocatedConsole)
    {
        // Light grey console text
        SetConsoleTextAttribute(ConsoleOutputHandle, 8);
        std::cout <<        "[Info]    " << message << "\n";
    }
}

void NativeLog::LogWarning(const std::string& message)
{
    if (HasOutputFileStream)
    {
        OutputFileStream << "[Warning] " << message << "\n";
        OutputFileStream.flush();
    }

    if (HasAllocatedConsole)
    {
        // Yellow console text
        SetConsoleTextAttribute(ConsoleOutputHandle, 14);
        std::cout <<        "[Warning] " << message << "\n";
    }
}

void NativeLog::LogError(const std::string& message)
{
    if (HasOutputFileStream)
    {
        OutputFileStream << "[Error]   " << message << "\n";
        OutputFileStream.flush();
    }

    if (HasAllocatedConsole)
    {
        // Red console text
        SetConsoleTextAttribute(ConsoleOutputHandle, 12);
        std::cout <<        "[Error]   " << message << "\n";
    }
}
