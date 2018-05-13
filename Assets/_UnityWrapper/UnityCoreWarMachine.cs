using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnityCoreWarMachine : MonoBehaviour
{
    #region Settings

    public string[] InstructionsClassNames;

    public string CompilerName;

    public string FirstProgramFilename;
    public string SecondProgramFilename;

    public int ProgramBytesLengthLimit = 100;

    public string FirstProgramName = "Player1";
    public string SecondProgramName = "Player2";

    #endregion

    #region Fields

    VirtualMachine.Machine m_Machine;

    #endregion
    
    #region Signals

    protected virtual void Start()
    {
        SetupVirtualMachine();
        SetupPrograms();
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

        compiler.OnCompilerError -= OnCompilerError;
        compiler.OnCompilerError += OnCompilerError;

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

    protected virtual void SetupPrograms()
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

        m_Machine.LoadProgram(program1, randomMemoryPosition1, FirstProgramName);
        m_Machine.LoadProgram(program2, randomMemoryPosition2, SecondProgramName);

        int startingPlayer = VirtualMachine.Utilities.Math.RandomIntClosed(1, 2);
        if(1 == startingPlayer)
        {
            m_ProcessQueue.Enqueue(FirstProgramName);
            m_ProcessQueue.Enqueue(SecondProgramName);
        }
        else
        {
            m_ProcessQueue.Enqueue(SecondProgramName);
            m_ProcessQueue.Enqueue(FirstProgramName);
        }
    }

    protected virtual VirtualMachine.MemoryCell[] CompileProgram(string fileName)
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, FirstProgramFilename);
        var programText = System.IO.File.ReadAllText(filePath);

        var program = m_Machine.CompileProgram(programText);

        if(null == program || 0 == program.Length)
            return null;

        return program;
    }

    protected virtual void OnCompilerError(string errorMessage)
    {
        Debug.LogError(errorMessage);
    }

    #endregion

    #region RunningLogic

    protected Queue<string> m_ProcessQueue = new Queue<string>();

    public virtual void ExecuteStep()
    {
        if(!m_ProcessQueue.Any())
            return;

        string processName = m_ProcessQueue.Dequeue();
        m_ProcessQueue.Enqueue(processName);

        var result = m_Machine.ExecuteStep(processName);

        if(!result.Success)
        {
            m_ProcessQueue.Clear();
            Debug.Log("Program (" + processName + ") has lost!");
        }
    }

    #endregion
}
