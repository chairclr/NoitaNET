#pragma once
typedef void (*FOnWorldPostUpdate)();
typedef void (*FOnWorldPreUpdate)();
typedef void (*FOnModPreInit)();
typedef void (*FOnModInit)();
typedef void (*FOnModPostInit)();

typedef struct
{
    FOnWorldPostUpdate OnWorldPostUpdate;
    FOnWorldPreUpdate OnWorldPreUpdate;
    FOnModPreInit OnModPreInit;
    FOnModInit OnModInit;
    FOnModPostInit OnModPostInit;
} Callbacks;