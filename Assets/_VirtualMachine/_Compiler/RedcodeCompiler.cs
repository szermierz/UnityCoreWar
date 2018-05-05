
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public sealed class RedcodeCompiler : ICompiler
    {
        private class TextInstruction
        {
            public string OpPneumonic;
            public string FirstArgument;
            public string FirstArgumentAddressType = "$";
            public string SecondArgument;
            public string SecondArgumentAddressType = "$";

            public TextInstruction(string _OpPneumonic, string _FirstArgument, string _SecondArgument)
            {
                OpPneumonic = _OpPneumonic;

                if(0 == _FirstArgument.Length)
                    throw new System.Exception("[RedcodeCompiler] Empty instruction argument (" + _OpPneumonic + ")");

                var firstArgumentSymbol = _FirstArgument[0];
                var firstArgumentAddressType = AddressTypes.GetAddressTypeOfSymbol(firstArgumentSymbol);
                if(null != firstArgumentAddressType)
                {
                    FirstArgument = _FirstArgument.Substring(1);
                    FirstArgumentAddressType = firstArgumentSymbol.ToString();
                }
                else
                {
                    FirstArgument = _FirstArgument;
                }

                var secondArgumentSymbol = _SecondArgument[0];
                var secondArgumentAddressType = AddressTypes.GetAddressTypeOfSymbol(secondArgumentSymbol);
                if(null != secondArgumentAddressType)
                {
                    SecondArgument = _SecondArgument.Substring(1);
                    SecondArgumentAddressType = secondArgumentSymbol.ToString();
                }
                else
                {
                    SecondArgument = _SecondArgument;
                }
            }
        }

        public override MemoryCell[] Compile(string programText, Machine machine)
        {
            try
            {
                var parsedProgram = Parse(programText);
                var preprocessedProgram = Preprocess(parsedProgram, machine);
                var compiledProgram = Compile(preprocessedProgram, machine);

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

        private TextInstruction[] Preprocess(string[] parsedProgram, Machine machine)
        {
            var result = new TextInstruction[parsedProgram.Length];

            Dictionary<string, int> labels = new Dictionary<string, int>();

            /* Gather labels definitions */
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

            /* Parse lines */
            for(int i = 0; i < parsedProgram.Length; ++i)
            {
                var lineText = parsedProgram[i];

                var labelEndPos = lineText.IndexOf(':');
                if(labelEndPos >= 0)
                    lineText = lineText.Substring(labelEndPos + 1);

                var instructionArguments = lineText.Split(new char[] { ' ', '\t', ',', '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
                if(instructionArguments.Length != 3)
                    throw new System.Exception("[RedcodeCompiler] Incorrect arguments count in line (" + parsedProgram[i] + ")");

                //TODO:SZ - diffrent arguments count
                result[i] = new TextInstruction(instructionArguments[0].ToUpper(), instructionArguments[1], instructionArguments[2]);
            }

            /* Resolve labels */
            for(int i = 0; i < result.Length; ++i)
            {
                if(labels.ContainsKey(result[i].FirstArgument))
                    result[i].FirstArgument = labels[result[i].FirstArgument].ToString();

                if(labels.ContainsKey(result[i].SecondArgument))
                    result[i].SecondArgument = labels[result[i].SecondArgument].ToString();
            }

            return result;
        }

        private MemoryCell[] Compile(TextInstruction[] preprocessedProgram, Machine machine)
        {
            var instructionsByPneumonic = GatherInstructions(machine);

            var result = new MemoryCell[preprocessedProgram.Length];
            for(int i = 0; i < preprocessedProgram.Length; ++i)
            {
                var programLine = preprocessedProgram[i];
                if(!instructionsByPneumonic.ContainsKey(programLine.OpPneumonic))
                    throw new System.Exception("[RedcodeCompiler] No instruction with pneumonic (" + programLine.OpPneumonic + ")");

                var instruction = instructionsByPneumonic[programLine.OpPneumonic];

                var cell = new MemoryCell();
                result[i] = cell;

                cell.OpCode.Int = instruction.GetOpCode().Int;

                var aFieldAddressType = AddressTypes.GetAddressTypeOfSymbol(programLine.FirstArgumentAddressType);
                if(null == aFieldAddressType)
                    throw new System.Exception("[RedcodeCompiler] No address type for specific symbol (" + programLine.FirstArgumentAddressType + ") in line (" + preprocessedProgram[i] + ")");
                cell.AFieldAddressType.Int = aFieldAddressType.GetBitCode().Int;

                //cell.AFieldValue = programLine.FirstArgument;

                var bFieldAddressType = AddressTypes.GetAddressTypeOfSymbol(programLine.SecondArgumentAddressType);
                if(null == bFieldAddressType)
                    throw new System.Exception("[RedcodeCompiler] No address type for specific symbol (" + programLine.SecondArgumentAddressType + ") in line (" + preprocessedProgram[i] + ")");
                cell.BFieldAddressType.Int = AddressTypes.GetAddressTypeOfSymbol(programLine.SecondArgumentAddressType).GetBitCode().Int;

                //cell.BFieldValue = programLine.SecondArgument;
            }

            return result;
        }

        private Dictionary<string, InstructionBase> GatherInstructions(Machine machine)
        {
            var result = new Dictionary<string, InstructionBase>();

            foreach(var instruction in machine.Instructions)
            {
                var pneumonic = instruction.GetPneumonic();
                if(result.ContainsKey(pneumonic))
                    throw new System.Exception("[RedcodeCompiler] Duplicated instructions pneumonics (" + pneumonic + ")");

                result.Add(pneumonic.ToUpper(), instruction);
            }

            return result;
        }

    }
}
