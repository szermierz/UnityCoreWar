

using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public abstract class InstructionBase
    {
        public abstract BitSet GetOpCode();
    }
}