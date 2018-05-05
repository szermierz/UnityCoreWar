
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{

    public static class AddressTypes
    {
        private static Dictionary<char, AddressTypeBase> m_AddressTypesBySymbol;
        private static Dictionary<long, AddressTypeBase> m_AddressTypesBytBitset;
        private static AddressTypeBase m_DefaultAddressType;

        private static void BuildAddressTypesDatabase()
        {
            var defaultAddressTypeType = typeof(ImmediateAddressType);//TODO:SZ - default address type

            m_AddressTypesBySymbol = new Dictionary<char, AddressTypeBase>();
            m_AddressTypesBytBitset = new Dictionary<long, AddressTypeBase>();

            var addressTypes = Utilities.Reflection.GetSubclasses(typeof(AddressTypeBase), false);

            foreach(var addressTypeType in addressTypes)
            {
                var addressType = Utilities.Reflection.CreateObjectOfType<AddressTypeBase>(addressTypeType.Name);
                if(null == addressType)
                    throw new System.Exception("[AddressTypes] Invalid type (" + addressTypeType + ")!");

                if(m_AddressTypesBySymbol.ContainsKey(addressType.Symbol))
                    throw new System.Exception("[AddressTypes] Duplicated symbol (" + addressType.Symbol + ") of types (" + addressTypeType + ") and (" + m_AddressTypesBySymbol[addressType.Symbol].GetType().Name + ")!");

                m_AddressTypesBySymbol.Add(addressType.Symbol, addressType);
                m_AddressTypesBytBitset.Add(addressType.GetBitCode().Int, addressType);

                if(addressTypeType == defaultAddressTypeType)
                    m_DefaultAddressType = addressType;
            }

            if(null == m_DefaultAddressType)
                m_DefaultAddressType = Utilities.Reflection.CreateObjectOfType<AddressTypeBase>(defaultAddressTypeType.Name);
        }

        static AddressTypes()
        {
            BuildAddressTypesDatabase();
        }

        public static AddressTypeBase GetAddressTypeOfSymbol(string symbol)
        {
            if(symbol.Equals(""))
                return m_DefaultAddressType;

            if(symbol.Length > 1)
                return null;

            return GetAddressTypeOfSymbol(symbol[0]);
        }

        public static AddressTypeBase GetAddressTypeOfSymbol(char symbol)
        {
            if(!m_AddressTypesBySymbol.ContainsKey(symbol))
                return null;

            return m_AddressTypesBySymbol[symbol];
        }

        public static AddressTypeBase GetAddressTypeOfBitSet(long value)
        {
            if(!m_AddressTypesBytBitset.ContainsKey(value))
                return null;

            return m_AddressTypesBytBitset[value];
        }
    }

    public abstract class AddressTypeBase
    {
        //TODO:SZ - implement program with absolute address loading - move values by program offset

        #region Interface

        public abstract char Symbol { get; }

        public abstract int GetAbsoluteIndex(MemoryModel model, int addressValue);
        public abstract int PeekAbsoluteIndex(MemoryModel model, int addressValue);

        #endregion

        #region Bits

        public BitSet GetBitCode()
        {
            if(null == m_Bits)
                m_Bits = s_BitCodesDistributor.GenerateFreeBitCode();

            return m_Bits;
        }

        private BitSet m_Bits;
        private static BitCodesDistributor s_BitCodesDistributor = new BitCodesDistributor(MachineConstants.AddressTypeBits);

        #endregion

    }

}