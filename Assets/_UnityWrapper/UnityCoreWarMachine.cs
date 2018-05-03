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

    private virtual void CompilePrograms()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, FirstProgramFilename);
        var result = System.IO.File.ReadAllText(filePath);
    }

    #endregion

    #region RunningLogic

    public virtual void ExecuteStep()
    {
        //TODO:SZ - implement
    }

    #endregion
}
