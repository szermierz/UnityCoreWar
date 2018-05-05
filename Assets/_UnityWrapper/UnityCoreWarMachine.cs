using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCoreWarMachine : MonoBehaviour
{
    #region Settings

    public string[] InstructionsClassNames;

    public string CompilerName;

    public string FirstProgramFilename;
    public string SecondProgramFilename;

    public int ProgramBytesLengthLimit = 100;

    #endregion

    #region Fields

    VirtualMachine.Machine m_Machine;

    #endregion
    
    #region Signals

    protected virtual void Start()
    {
        SetupVirtualMachine();
        CompilePrograms();
    }

    protected virtual void Update()
    {
        ExecuteStep();
    }

    #endregion

    #region Construction

    protected virtual void SetupVirtualMachine()
    {
        var compiler = VirtualMachine.Utilities.Reflection.CreateObjectOfType<VirtualMachine.ICompiler>(CompilerName);
        if(null == compiler)
        {
            Debug.LogError("[UnityCoreWarMachine] Invalid compiler name!");
            return;
        }

        m_Machine = new VirtualMachine.Machine(compiler);

        foreach(var instructionsClassName in InstructionsClassNames)
        {
            var instruction = VirtualMachine.Utilities.Reflection.CreateObjectOfType<VirtualMachine.InstructionBase>(instructionsClassName);
            if(null == instruction)
            {
                Debug.LogError("[UnityCoreWarMachine] Couldnt create instruction (" + instructionsClassName + ")!");
                continue;
            }

            m_Machine.RegisterInstruction(instruction);
        }
    }

    protected virtual void CompilePrograms()
    {
        var program1 = CompileProgram(FirstProgramFilename);
        var program2 = CompileProgram(SecondProgramFilename);

        if(null == program1 || null == program2)
        {
            Debug.LogError("[UnityCoreWarMachine] Failed to compile programs!");
            return;
        }

        if(VirtualMachine.MemoryModel.Size < program1.Length + program2.Length)
        {
            Debug.LogError("[UnityCoreWarMachine] Programs length sum exceeds memory space!");
            return;
        }

        int randomMemoryPosition1 = VirtualMachine.Utilities.Math.RandomIntOpen(0, VirtualMachine.MemoryModel.Size);
        int freeSpace = VirtualMachine.MemoryModel.Size - program1.Length - program2.Length + 1;

        int randomMemoryPosition2 = VirtualMachine.Utilities.Math.RandomIntOpen(0, freeSpace);
        if(randomMemoryPosition2 + program2.Length > randomMemoryPosition1)
            randomMemoryPosition2 += program1.Length; // Move forward if generated position collides - the position was randomized from a free space

        m_Machine.LoadProgram(program1, randomMemoryPosition1);
        m_Machine.LoadProgram(program2, randomMemoryPosition2);

        //TODO:SZ - create processes
    }

    protected virtual VirtualMachine.MemoryCell[] CompileProgram(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, FirstProgramFilename);
        var programText = System.IO.File.ReadAllText(filePath);

        string errorMessage;
        var program = m_Machine.CompileProgram(programText, out errorMessage);

        if(null == program || 0 == program.Length)
        {
            Debug.LogError(errorMessage);
            return null;
        }

        return program;
    }

    #endregion

    #region RunningLogic

    public virtual void ExecuteStep()
    {
        //TODO:SZ - implement
    }

    #endregion
}
