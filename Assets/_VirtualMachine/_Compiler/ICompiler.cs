
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public abstract class ICompiler
    {
        public delegate void ErrorEventDelegate(string errorMessage);

        public ErrorEventDelegate OnCompilerError;

        public abstract MemoryCell[] Compile(string programText);
    }
}
