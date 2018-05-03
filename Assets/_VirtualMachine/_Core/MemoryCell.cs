
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public class MemoryCell
    {
        
        public MemoryCell()
        {
            OpCode            = new BitSet(MachineConstants.OpCodeBits);
            AFieldAddressType = new BitSet(MachineConstants.AddressTypeBits);
            AFieldValue       = new BitSet(MachineConstants.ValueBits);
            BFieldAddressType = new BitSet(MachineConstants.AddressTypeBits);
            BFieldValue       = new BitSet(MachineConstants.ValueBits);
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
