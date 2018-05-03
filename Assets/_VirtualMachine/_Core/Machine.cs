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

        protected MemoryModel m_MemoryModel;
        protected ProcessQueue m_ProcessQueue;
        protected readonly InstructionSet m_Instructions;

        public Machine(ICompiler _Compiler)
        {
            Compiler = _Compiler;

            m_MemoryModel = new MemoryModel();
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

        /* returns number of compiled cells */
        public virtual int CompileProgram(string programText, int startIndex, out string errorMessage)
        {
            errorMessage = "";

            try
            {
                var code = Compiler.Compile(programText, this);

                if(code.Length > m_MemoryModel.Size)
                {
                    errorMessage = "[Machine] Compiled program is too big!";
                    return 0;
                }

                var cellIndex = startIndex;
                foreach(var codeCell in code)
                {
                    m_MemoryModel[cellIndex++] = codeCell;

                    if(cellIndex >= m_MemoryModel.Size)
                        cellIndex = 0;
                }

                return code.Length;
            }
            catch(Exception compilerError)
            {
                errorMessage = compilerError.Message;
                return 0;
            }
        }

    }

}