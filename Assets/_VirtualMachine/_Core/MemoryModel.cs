
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public class MemoryModel
    {
        public readonly int OpCodeBits;
        public readonly int AddressTypeBits;
        public readonly int ValueBits;

        public int MemoryBitsCount
        {
            get
            {
                return OpCodeBits + 2 * (AddressTypeBits + ValueBits);
            }
        }

        public MemoryModel(int _OpCodeBits, int _AddressTypeBits, int _ValueBits)
        {
            OpCodeBits = _OpCodeBits;
            AddressTypeBits = _AddressTypeBits;
            ValueBits = _ValueBits;

            InitializeMemory();
        }

        protected virtual void InitializeMemory()
        {
            m_Memory = new MemoryCell[Size];
            for(int i = 0; i < m_Memory.Length; ++i)
                m_Memory[i] = new MemoryCell(OpCodeBits, AddressTypeBits, ValueBits);
        }

        public int Size { get { return Utilities.Math.Pow(2, ValueBits); } }

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
