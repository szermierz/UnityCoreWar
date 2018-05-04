

using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public abstract class InstructionBase
    {

        #region Result Types

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

        #endregion

        #region Interface

        public abstract string GetPneumonic();

        public abstract ExecutionResult Execute(Process process, MemoryModel model, int address);

        #endregion

        #region OpCode

        public BitSet GetOpCode()
        {
            if(null == m_Bits)
                m_Bits = s_BitCodesDistributor.GenerateFreeBitCode();

            return m_Bits;
        }

        private BitSet m_Bits;
        private static BitCodesDistributor s_BitCodesDistributor = new BitCodesDistributor(MachineConstants.OpCodeBits);

        #endregion

    }
}