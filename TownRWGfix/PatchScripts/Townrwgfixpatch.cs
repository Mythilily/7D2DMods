using System;
using SDX.Compiler;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;

public class TownRWGFixPatch : IPatcherMod

{
    public bool Patch(ModuleDefinition module)

    {

        Console.WriteLine("==TownRWGFix Patcher Patcher===");
        var gm = module.Types.First(d => d.Name == "GenerationManager");
        var method = gm.Methods.First(d => d.Name == "GenerateTowns");
        SetMethodToPublic(method);
        return true;
    }

    public bool Link(ModuleDefinition gameModule, ModuleDefinition modModule)
    {
        return true;
    }

    private void SetMethodToVirtual(MethodDefinition meth)
    {
        meth.IsVirtual = true;
    }

    private void SetFieldToPublic(FieldDefinition field)
    {
        field.IsFamily = false;
        field.IsPrivate = false;
        field.IsPublic = true;
    }

    private void SetMethodToPublic(MethodDefinition field)
    {
        field.IsFamily = false;
        field.IsPrivate = false;
        field.IsPublic = true;
    }
}

