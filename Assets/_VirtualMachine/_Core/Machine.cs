﻿
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{

    public class Machine
    {
        public sealed class InstructionSet
        {
            private Dictionary<BitSet, InstructionBase> m_Instructions;

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

            public IEnumerator<InstructionBase> GetEnumerator()
            {
                foreach(var instruction in m_Instructions)
                    yield return instruction.Value;
            }

            public InstructionBase GetInstruction(BitSet opCode)
            {
                if(m_Instructions.ContainsKey(opCode))
                    return m_Instructions[opCode];

                return null;
            }
        }

        public readonly ICompiler Compiler;
        public readonly int OpCodeBits;
        public readonly int AddressTypeBits;
        public readonly int ValueBits;

        protected MemoryModel m_MemoryModel;
        protected ProcessQueue m_ProcessQueue;
        protected readonly InstructionSet m_Instructions;

        public Machine(ICompiler _Compiler, int _OpCodeBits, int _AddressTypeBits, int _ValueBits)
        {
            Compiler = _Compiler;
            OpCodeBits = _OpCodeBits;
            AddressTypeBits = _AddressTypeBits;
            ValueBits = _ValueBits;

            m_MemoryModel = new MemoryModel(OpCodeBits, AddressTypeBits, ValueBits);
            m_ProcessQueue = new ProcessQueue(this);
            m_Instructions = new InstructionSet();
        }

        public virtual bool RegisterInstruction(InstructionBase instruction)
        {
            return m_Instructions.RegisterInstruction(instruction);
        }

        public virtual InstructionSet Instructions
        {
            get { return m_Instructions; }
        }

    }

}