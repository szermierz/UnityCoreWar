using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualMachine
{
    public class MemoryModel : MonoBehaviour
    {
        public int OpCodeBits;
        public int AddressTypeBits;
        public int ValueBits;

        public int MemoryBitsCount
        {
            get
            {
                return OpCodeBits + 2 * (AddressTypeBits + ValueBits);
            }
        }

        protected virtual void Start()
        {
            InitializeMemory();
        }

        protected virtual void InitializeMemory()
        {
            Memory = new MemoryCell[(int)Mathf.Pow(2, ValueBits)];
            for(int i = 0; i < Memory.Length; ++i)
                Memory[i] = new MemoryCell(OpCodeBits, AddressTypeBits, ValueBits);
        }

        protected MemoryCell[] Memory;

        public long this[int memoryIndex]
        {
            get
            {
                //TODO:SZ
                return 0;
            }
            set
            {

            }
        }
    }
}
