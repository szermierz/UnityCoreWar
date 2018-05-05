
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{

    public class Process
    {
        public int InstructionCounter;

        public readonly Machine Machine;
        public readonly ProcessQueue Owner;
        public readonly MemoryModel Model;

        public Process(Machine _Machine, ProcessQueue _Owner, MemoryModel _Model, int startAddress)
        {
            Machine = _Machine;
            Owner = _Owner;
            Model = _Model;
            InstructionCounter = startAddress;
        }

        public virtual InstructionBase.ExecutionResult ExecuteStep()
        {
            var cell = Model[InstructionCounter];
            var instruction = Machine.Instructions.GetInstruction(cell.OpCode);

            var result = instruction.Execute(this, Model, InstructionCounter);

            if(result.Success)
                IncrementInstructionCounter();

            return result;
        }

        public virtual void IncrementInstructionCounter()
        {
            ++InstructionCounter;

            if(InstructionCounter >= MemoryModel.Size)
                InstructionCounter = 0;
        }
    }

}