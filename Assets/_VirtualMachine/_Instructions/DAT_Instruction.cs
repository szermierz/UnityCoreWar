using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualMachine;

public class DAT_Instruction : VirtualMachine.InstructionBase
{
    public override ExecutionResult Execute(Process process, MemoryModel model, int address)
    {
        return new ExecutionResult(ExecutionResult.ResultType.InvalidOpCode);
    }

    public override string GetPneumonic()
    {
        return "DAT";
    }
}
