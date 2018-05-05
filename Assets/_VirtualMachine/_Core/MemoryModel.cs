
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public class MemoryModel
    {
        public static int MemoryBitsCount
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
        }

        public static int Size { get { return Utilities.Math.Pow(2, MachineConstants.ValueBits); } }

        protected MemoryCell[] m_Memory;

        public MemoryCell this[int memoryIndex]
        {
            get
            {
                if(null == m_Memory)
                    InitializeMemory();

                if(null == m_Memory[memoryIndex])
                    m_Memory[memoryIndex] = new MemoryCell();

                return m_Memory[memoryIndex];
            }

            set
            {
                if(null == m_Memory)
                    InitializeMemory();

                m_Memory[memoryIndex] = value;
            }
        }
    }
}
