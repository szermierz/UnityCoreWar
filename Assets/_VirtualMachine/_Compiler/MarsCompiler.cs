
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public sealed class MarsCompiler : ICompiler
    {
        public override MemoryCell[] Compile(string programText)
        {
            var parsedProgram = Parse(programText);
            var preprocessedProgram = Preprocess(parsedProgram);
            var compiledProgram = Compile(preprocessedProgram);

            return compiledProgram;
        }

        private string[] Parse(string programText)
        {

        }

        private string[] Preprocess(string[] parsedProgram)
        {

        }

        private MemoryCell[] Compile(string[] preprocessedProgram)
        {

        }
    }
}
