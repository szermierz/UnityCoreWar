
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public class MemoryModel
    {
        public int MemoryBitsCount
        {
            get
            {
                return MachineConstants.OpCodeBits + 2 * (MachineConstants.AddressTypeBits + MachineConstants.ValueBits);
            }
        }

        public MemoryModel()
        {
            InitializeMemory();
        }

        protected virtual void InitializeMemory()
        {
            m_Memory = new MemoryCell[Size];
            for(int i = 0; i < m_Memory.Length; ++i)
                m_Memory[i] = new MemoryCell();
        }

        public int Size { get { return Utilities.Math.Pow(2, MachineConstants.ValueBits); } }

        protected MemoryCell[] m_Memory;

        public MemoryCell this[int memoryIndex]
        {
            get
            {
                if(null == m_Memory)
                    InitializeMemory();

                return m_Memory[memoryIndex];
            }
        }
    }
}
