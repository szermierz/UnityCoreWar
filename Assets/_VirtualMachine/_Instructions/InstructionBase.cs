

using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public abstract class InstructionBase
    {
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

        public abstract BitSet GetOpCode();
        public abstract string GetPneumonic();

        public abstract ExecutionResult Execute(Process process, MemoryModel model, int address);
    }
}