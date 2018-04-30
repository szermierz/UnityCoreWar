
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public sealed class RedcodeCompiler : ICompiler
    {
        private class TextInstruction
        {
            public readonly string OpPneumonic;
            public readonly string FirstArgument;
            public readonly string SecondArgument;

            public TextInstruction(string _OpPneumonic, string _FirstArgument, string _SecondArgument)
            {
                OpPneumonic = _OpPneumonic;
                FirstArgument = _FirstArgument;
                SecondArgument = _SecondArgument;
            }
        }

        public override MemoryCell[] Compile(string programText)
        {
            try
            {
                var parsedProgram = Parse(programText);
                var preprocessedProgram = Preprocess(parsedProgram);
                var compiledProgram = Compile(preprocessedProgram);

                return compiledProgram;
            }
            catch(System.Exception e)
            {
                if(null != OnCompilerError)
                    OnCompilerError.Invoke(e.Message);
            }

            return null;
        }

        private string[] Parse(string programText)
        {
            return programText.Split(new string[] { "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        }

        private TextInstruction[] Preprocess(string[] parsedProgram)
        {
            var result = new TextInstruction[parsedProgram.Length];

            Dictionary<string, int> labels = new Dictionary<string, int>();

            for(int i = 0; i < parsedProgram.Length; ++i)
            {
                var parsedIntoLabels = parsedProgram[i].Split(':');
                if(parsedIntoLabels.Length <= 1)
                    continue;

                var label = parsedIntoLabels[0];
                if(labels.ContainsKey(label))
                    throw new System.Exception("[RedcodeCompiler] Duplicated label (" + label + ")");

                labels.Add(label, i);
            }

            for(int i = 0; i < parsedProgram.Length; ++i)
            {
                var lineText = parsedProgram[i];

                var labelEndPos = lineText.IndexOf(':');
                if(labelEndPos >= 0)
                    lineText = lineText.Substring(labelEndPos + 1);

                var instructionArguments = lineText.Split(new char[] { ' ', '\t', ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                if(instructionArguments.Length != 3)
                    throw new System.Exception("[RedcodeCompiler] Incorrect arguments count in line (" + parsedProgram[i] + ")");

                //TODO:SZ - diffrent arguments count
                //TODO:SZ - labels parsing
                result[i] = new TextInstruction(instructionArguments[0].ToUpper(), instructionArguments[1], instructionArguments[2]);
            }

            return result;
        }

        private MemoryCell[] Compile(TextInstruction[] preprocessedProgram)
        {

        }
        
    }
}
