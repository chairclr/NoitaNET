#pragma once
#include "Util.h"
#include "framework.h"
#include <fstream>
#include "Entry.h"
#include <iostream>

class NativeLog
{
public:
    static bool Init();

    static void LogInformation(const std::string& message);

    static void LogWarning(const std::string& message);

    static void LogError(const std::string& message);

private:
    static inline bool HasOutputFileStream;

    static inline std::ofstream OutputFileStream;

    static inline bool HasAllocatedConsole;

    static inline HANDLE ConsoleOutputHandle;
};
