
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public abstract class ICompiler
    {
        public abstract MemoryCell[] Compile(string programText);
    }
}
