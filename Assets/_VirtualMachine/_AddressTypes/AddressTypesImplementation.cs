
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    internal class AddressBitCodesDistributor
    {
        private static AddressBitCodesDistributor s_Instance = null;

        public static AddressBitCodesDistributor Instance
        {
            get
            {
                if(null == s_Instance)
                    s_Instance = new AddressBitCodesDistributor();

                return s_Instance;
            }
        }

        private long m_LastGeneratedBitCode = 0;

        public BitSet GenerateFreeBitCode()
        {
            var result = new BitSet(MachineConstants.AddressTypeBits);
            result.Int = m_LastGeneratedBitCode++;
            return result;
        }
    }

    public abstract class SimpleAddressType : AddressTypeBase
    {
        private BitSet m_BitCode = null;

        public override BitSet GetBitCode()
        {
            if(null == m_BitCode)
                m_BitCode = AddressBitCodesDistributor.Instance.GenerateFreeBitCode();

            return m_BitCode;
        }
    }

    public class ImmediateAddressType : SimpleAddressType
    {
        public override char Symbol { get { return '#'; } }

        public override int GetAbsoluteIndex(MemoryModel model, int addressValue)
        {
            throw new NotImplementedException();
        }

        public override BitSet GetBitCode()
        {
            throw new NotImplementedException();
        }

        public override int PeekAbsoluteIndex(MemoryModel model, int addressValue)
        {
            throw new NotImplementedException();
        }
    }
}