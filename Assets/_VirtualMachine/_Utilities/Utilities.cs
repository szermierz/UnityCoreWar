using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualMachine
{
    public static class Utilities
    {



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

        public BitSet Combine()
        {
            //TODO:SZ
            return null;
        }
    }
}


