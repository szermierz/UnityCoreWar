using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualMachine
{
    public class MemoryCell
    {
        public readonly int OpCodeBits;
        public readonly int AddressTypeBits;
        public readonly int ValueBits;
        
        public MemoryCell(int _OpCodeBits, int _AddressTypeBits, int _ValueBits)
        {
            OpCodeBits = _OpCodeBits;
            AddressTypeBits = _AddressTypeBits;
            ValueBits = _ValueBits;

            //TODO:SZ - bitsets
        }

        public readonly BitSet OpCode;
        public readonly BitSet AFieldAddressType;
        public readonly BitSet AFieldValue;
        public readonly BitSet BFieldAddressType;
        public readonly BitSet BFieldValue;
    }
}
