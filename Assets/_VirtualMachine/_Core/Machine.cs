
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{

    public class Machine
    {
        public readonly int OpCodeBits;
        public readonly int AddressTypeBits;
        public readonly int ValueBits;

        protected MemoryModel m_MemoryModel;
        protected ProcessQueue m_ProcessQueue;
        protected Dictionary<BitSet, InstructionBase> m_Instructions;

        public Machine(int _OpCodeBits, int _AddressTypeBits, int _ValueBits)
        {
            OpCodeBits = _OpCodeBits;
            AddressTypeBits = _AddressTypeBits;
            ValueBits = _ValueBits;

            m_MemoryModel = new MemoryModel(OpCodeBits, AddressTypeBits, ValueBits);
            m_ProcessQueue = new ProcessQueue();
            m_Instructions = new Dictionary<BitSet, InstructionBase>();
        }

        public bool RegisterInstruction(InstructionBase instruction)
        {
            if(null == instruction)
                return false;

            var opCode = instruction.GetOpCode();
            if(null == opCode)
                return false;

            if(m_Instructions.ContainsKey(opCode))
                return false;

            m_Instructions.Add(opCode, instruction);
            return true;
        }

    }

}