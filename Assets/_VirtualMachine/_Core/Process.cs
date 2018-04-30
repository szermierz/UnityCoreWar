
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{

    public class Process
    {
        public int InstructionCounter;

        public readonly ProcessQueue Owner;

        public Process(ProcessQueue owner, MemoryModel model, int startAddress)
        {
            Owner = owner;
            InstructionCounter = startAddress;
        }

        public ExecutionResult ExecuteStep()
        {
            //TODO:SZ
        }

        public sealed class ExecutionResult
        {
            public enum ResultType
            {
                Success,
                CouldntStart,
                InvalidOpCode,
            }

            public readonly ResultType Type;

            public ExecutionResult(ResultType type)
            {
                Type = type;
            }

            public bool Success
            {
                get
                {
                    return ResultType.Success == Type;
                }
            }
        }
    }

}