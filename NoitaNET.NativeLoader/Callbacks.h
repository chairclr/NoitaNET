#pragma once
typedef void (*FOnWorldPostUpdate)();
typedef void (*FOnWorldPreUpdate)();

typedef struct
{
    FOnWorldPostUpdate OnWorldPostUpdate;
    FOnWorldPreUpdate OnWorldPreUpdate;
} Callbacks;