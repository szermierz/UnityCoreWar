
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public class ImmediateAddressType : AddressTypeBase
    {
        public override char Symbol { get { return '#'; } }

        public override int GetAbsoluteIndex(MemoryModel model, int addressValue)
        {
            throw new NotImplementedException();
        }

        public override int PeekAbsoluteIndex(MemoryModel model, int addressValue)
        {
            throw new NotImplementedException();
        }
    }
}