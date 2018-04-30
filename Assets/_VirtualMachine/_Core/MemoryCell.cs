
using System.Collections;
using System.Collections.Generic;

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

            OpCode            = new BitSet(OpCodeBits);
            AFieldAddressType = new BitSet(AddressTypeBits);
            AFieldValue       = new BitSet(ValueBits);
            BFieldAddressType = new BitSet(AddressTypeBits);
            BFieldValue       = new BitSet(ValueBits);
        }

        public readonly BitSet OpCode;
        public readonly BitSet AFieldAddressType;
        public readonly BitSet BFieldAddressType;
        public readonly BitSet AFieldValue;
        public readonly BitSet BFieldValue;

        public long Value
        {
            get
            {
                return Bits.Int;
            }
        }

        public BitSet Bits
        {
            get
            {
                return OpCode.Combine(AFieldAddressType.Combine(BFieldAddressType.Combine(AFieldValue.Combine(BFieldValue))));
            }
        }
    }
}
