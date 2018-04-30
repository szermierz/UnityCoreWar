﻿
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public static class Utilities
    {

        public static class Math
        {
            public static T Pow<T>(T powBase, T powExp)
            {
                return (T)(object)UnityEngine.Mathf.Pow((float)(object)powBase, (float)(object)powExp);
            }
        }

        public static class Reflection
        {
            public static T CreateObjectOfType<T>(string typeName)
            {
                var objectType = Type.GetType(typeName);
                if(null == objectType)
                    return default(T); //TODO - look over assemblies

                var result = Activator.CreateInstance(objectType);
                
                try
                {
                    return (T)result;
                }
                finally
                { }

                return default(T);
            }
        }

    }

    public class BitSet : System.ICloneable, System.IComparable<BitSet>, System.IEquatable<BitSet>
    {
        public BitSet(int bitsCount)
        {
            if(bitsCount > sizeof(long) * 8)
                bitsCount = sizeof(long) * 8;

            m_BitsCount = bitsCount;
        }

        public long Int
        {
            get
            {
                long value = m_Value & ~(1L << m_BitsCount);

                if(this[m_BitsCount - 1])
                    value = -value;

                return value;
            }

            set
            {
                bool positive = value > 0;
                long abs = positive ? value : -value;

                m_Value = abs;

                if(!positive)
                    this[m_BitsCount - 1] = true;
            }
        }

        public bool this[int bitIndex]
        {
            get
            {
                return (m_Value & bitIndex) != 0;
            }

            set
            {
                if(bitIndex >= m_BitsCount)
                    return;

                m_Value |= 1L << bitIndex;
            }
        }

        protected readonly int m_BitsCount;
        protected long m_Value = 0;

        object ICloneable.Clone()
        {
            var result = new BitSet(m_BitsCount);
            result.m_Value = m_Value;
            return result;
        }

        int IComparable<BitSet>.CompareTo(BitSet other)
        {
            return Comparer.Default.Compare(m_Value, other.m_Value);
        }

        bool IEquatable<BitSet>.Equals(BitSet other)
        {
            return m_Value == other.m_Value;
        }

        public BitSet Combine(BitSet rhv)
        {
            var result = new BitSet(m_BitsCount + rhv.m_BitsCount);
            result.m_Value = (m_Value << rhv.m_BitsCount) | rhv.m_Value;

            return result;
        }

        public void Decompose(params BitSet[] results)
        {
            //TODO:SZ - decompose whole bitset into separate bitsets based on their length.
        }
    }
}


