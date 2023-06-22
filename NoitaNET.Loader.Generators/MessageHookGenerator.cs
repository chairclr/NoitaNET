using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

[Generator(LanguageNames.CSharp)]
public class MessageHookGenerator : ISourceGenerator
{
    private static string[] Functions = new string[]
    {
        "ItemChestComponent_HandleMessage_Message_Death",
        "AbilityComponent_HandleMessage_Message_UseAbility",
        "AbilityComponent_HandleMessage_Message_UseItem",
        "ProjectileComponent_HandleMessage_Message_ThrowItem",
        "ItemComponent_HandleMessage_Message_ThrowItem",
        "ItemComponent_HandleMessage_Message_TransformUpdated",
        "AnimalAIComponent_HandleMessage_Message_DamageReceived",
        "AnimalAIComponent_HandleMessage_Message_CollisionWithCell",
        "AnimalAIComponent_HandleMessage_Message_NamedEvent",
        "AnimalAIComponent_HandleMessage_Message_Teleported",
        "DamageModelComponent_HandleMessage_Message_Shot",
        "CharacterPlatformingComponent_HandleMessage_Message_MoveItem",
        "GameEffectComponent_HandleMessage_Message_Shot",
        "GameEffectComponent_HandleMessage_Message_Kick",
        "GameEffectComponent_HandleMessage_Message_MeleeOrDashAttack",
        "GameEffectComponent_HandleMessage_Message_DamageAboutToBeApplied",
        "GameEffectComponent_HandleMessage_Message_DamageAboutToBeReceived",
        "GameEffectComponent_HandleMessage_Message_DamageAboutToBeReceivedPost",
        "Inventory2Component_HandleMessage_Message_Death",
        "Inventory2Component_HandleMessage_Message_MoveItem",
        "Inventory2Component_HandleMessage_Message_ItemUnlock",
        "PhysicsThrowableComponent_HandleMessage_Message_ItemPickUp",
        "AudioComponent_HandleMessage_Message_NamedEvent",
        "VerletPhysicsComponent_HandleMessage_Message_Teleported",
        "PhysicsBodyComponent_HandleMessage_Message_Death",
        "PhysicsBodyComponent_HandleMessage_Message_PhysicsBodyDestroyed",
        "PhysicsBodyComponent_HandleMessage_Message_ThrowItem",
        "PhysicsBodyComponent_HandleMessage_Message_TransformUpdated",
        "ParticleEmitterComponent_HandleMessage_Message_TransformUpdated",
        "CrawlerAnimalComponent_HandleMessage_Message_DamageReceived",
        "DroneLauncherComponent_HandleMessage_Message_UseItem",
        "InventoryGuiComponent_HandleMessage_Message_CannotShootWand",
        "LuaComponent_HandleMessage_Message_DamageReceived",
        "LuaComponent_HandleMessage_Message_DamageAboutToBeReceived",
        "LuaComponent_HandleMessage_Message_ItemPickUp",
        "LuaComponent_HandleMessage_Message_Shot",
        "LuaComponent_HandleMessage_Message_CollisionTriggerHit",
        "LuaComponent_HandleMessage_Message_CollisionTriggerTimerFinished",
        "LuaComponent_HandleMessage_Message_PhysicsBodyModified",
        "LuaComponent_HandleMessage_Message_PhysicsBodyDestroyed",
        "LuaComponent_HandleMessage_Message_PressurePlateChange",
        "LuaComponent_HandleMessage_Message_Death",
        "LuaComponent_HandleMessage_Message_ThrowItem",
        "LuaComponent_HandleMessage_Message_MaterialAreaCheckerFailed",
        "LuaComponent_HandleMessage_Message_MaterialAreaCheckerSuccess",
        "LuaComponent_HandleMessage_Message_ElectricityReceiverSwitched",
        "LuaComponent_HandleMessage_Message_ElectricityReceiverElectrified",
        "LuaComponent_HandleMessage_Message_Kick",
        "LuaComponent_HandleMessage_Message_Interaction",
        "LuaComponent_HandleMessage_Message_AudioEventDead",
        "LuaComponent_HandleMessage_Message_WandFired",
        "LuaComponent_HandleMessage_Message_Teleported",
        "LuaComponent_HandleMessage_Message_PortalTeleportUsed",
        "LuaComponent_HandleMessage_Message_PolymorphingTo",
        "OrbComponent_HandleMessage_Message_ItemPickUp",
        "ExplodeOnDamageComponent_HandleMessage_Message_DamageReceived",
        "ExplodeOnDamageComponent_HandleMessage_Message_Death",
        "ExplodeOnDamageComponent_HandleMessage_Message_PhysicsBodyModified",
        "ExplodeOnDamageComponent_HandleMessage_Message_PhysicsBodyDestroyed",
        "ExplodeOnDamageComponent_HandleMessage_Message_Explosion",
        "MaterialInventoryComponent_HandleMessage_Message_Death",
        "MaterialInventoryComponent_HandleMessage_Message_DamageReceived",
        "MaterialInventoryComponent_HandleMessage_Message_PhysicsBodyDestroyed",
        "MaterialInventoryComponent_HandleMessage_Message_Kick",
        "GameStatsComponent_HandleMessage_Message_Death",
        "GameStatsComponent_HandleMessage_Message_KilledSomeone",
        "GameStatsComponent_HandleMessage_Message_PlayerDied",
        "GameStatsComponent_HandleMessage_Message_ItemPickUp",
        "GameStatsComponent_HandleMessage_Message_VisitedNewBiome",
        "GameStatsComponent_HandleMessage_Message_DamageReceived",
        "GameStatsComponent_HandleMessage_Message_Teleported",
        "GameLogComponent_HandleMessage_Message_GameLog",
        "GameLogComponent_HandleMessage_Message_DamageReceived",
        "GameLogComponent_HandleMessage_Message_ItemPickUp",
        "GameLogComponent_HandleMessage_Message_Death",
        "WorldStateComponent_HandleMessage_Message_PlayerDied",
        "IngestionComponent_HandleMessage_Message_ItemAboutToBeEaten",
        "InheritTransformComponent_HandleMessage_Message_TransformUpdated",
        "ItemPickUpperComponent_HandleMessage_Message_Kick",
        "ItemPickUpperComponent_HandleMessage_Message_Death",
        "ItemCostComponent_HandleMessage_Message_ItemAllowedToPickUp",
        "ItemCostComponent_HandleMessage_Message_ItemPickUp",
        "ItemStashComponent_HandleMessage_Message_ThrowItem",
        "ItemStashComponent_HandleMessage_Message_MoveItem",
        "SpriteStainsComponent_HandleMessage_Message_DamageReceived",
        "PhysicsBody2Component_HandleMessage_Message_Death",
        "PhysicsBody2Component_HandleMessage_Message_PhysicsBodyDestroyed",
        "PhysicsBody2Component_HandleMessage_Message_TransformUpdated",
        "PhysicsAIComponent_HandleMessage_Message_DamageReceived",
        "PhysicsBodyCollisionDamageComponent_HandleMessage_Message_PhysicsBodyCollision",
        "PlatformShooterPlayerComponent_HandleMessage_Message_CameraShake",
        "PlatformShooterPlayerComponent_HandleMessage_Message_DamageReceived",
        "PlatformShooterPlayerComponent_HandleMessage_Message_NamedEvent",
        "PotionComponent_HandleMessage_Message_ThrowItem",
        "SimplePhysicsComponent_HandleMessage_Message_TransformUpdated",
        "TorchComponent_HandleMessage_Message_ThrowableItemUsed",
    };

    public void Execute(GeneratorExecutionContext context)
    {
        Compilation compilation = context.Compilation;

        StringBuilder builder = new StringBuilder();

        builder.AppendLine("namespace NoitaNET.Loader;");
        builder.AppendLine("internal unsafe partial class Callbacks");
        builder.AppendLine("{");
        builder.AppendLine($"[System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.ThisCall)]");
        builder.AppendLine($"private delegate void __D_MessageHandler(nint pThis, nint param_1, nint param_2);");

        builder.AppendLine($"[System.Runtime.InteropServices.UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.ThisCall)]");
        builder.AppendLine($"private delegate void __D_ExecuteScript(nint pThis, nint param_1, nint param_2, nint param_3);");

        /* 
         * [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
         * private delegate void DMessageListener_LuaComponent_Message_Shot(nint pThis, nint param_1, nint param_2);
         */

        StringBuilder functionPointerParameters = new StringBuilder();

        IEnumerable<IMethodSymbol> methods = compilation.GetTypeByMetadataName($"NoitaNET.Loader.Callbacks")!.GetMembers().Where(x => x.Kind == SymbolKind.Method).Cast<IMethodSymbol>();

        foreach (string function in Functions)
        {
            IMethodSymbol? method = methods.FirstOrDefault(x => x.MetadataName == function);

            if (method is null)
                continue;

            functionPointerParameters.Clear();

            foreach (IParameterSymbol parameter in method.Parameters)
            {
                functionPointerParameters.Append($"{parameter.Type.ToDisplayString()}, ");
            }
            functionPointerParameters.Append(method.ReturnType.ToDisplayString());

            builder.AppendLine($"private static readonly __D_MessageHandler __O_{function};");
            builder.AppendLine($"private static readonly NoitaNET.API.Hooks.NativeHook __hd_{function};");
            builder.AppendLine($"private static bool __wh_{function} = false;");
            builder.AppendLine($"private static __D_ExecuteScript O{function};");

            builder.AppendLine(
              $$"""
                [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new Type[] { typeof(System.Runtime.CompilerServices.CallConvThiscall) })]
                private static void __H_{{function}}(nint pThis, nint component, nint message)
                {
                    if (!__wh_{{function}})
                    {
                        nint vtableFunction = *(nint*)(*(nint*)(pThis) + 8);
                        
                        NoitaNET.API.Hooks.NativeHook workingNativeHook = new NoitaNET.API.Hooks.NativeHook(vtableFunction, (nint)(delegate* unmanaged[Thiscall]<{{functionPointerParameters}}>)&{{function}});
                        
                        System.Reflection.FieldInfo? oField = typeof(Callbacks).GetField("O{{function}}", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
                
                        if (oField is not null)
                        {
                            oField.SetValue(null, System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer<__D_ExecuteScript>(workingNativeHook.Original));
                        }

                        __wh_{{function}} = true;
                    }

                    __O_{{function}}!(pThis, component, message);
                }
                """);
        }

        builder.AppendLine("}");

        context.AddSource("Callbacks.Hooks.g.cs", builder.ToString());
    }

    public void Initialize(GeneratorInitializationContext context)
    {

    }
}
