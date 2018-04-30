using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCoreWarMachine : MonoBehaviour
{
    #region Settings

    public int OpCodeBits;
    public int AddressTypeBits;
    public int ValueBits;

    public string[] InstructionsClassNames;

    public string CompilerName;

    #endregion

    #region Fields

    VirtualMachine.Machine m_Machine;

    #endregion
    
    #region Signals

    protected virtual void Start()
    {
        SetupVirtualMachine();
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

        m_Machine = new VirtualMachine.Machine(compiler, OpCodeBits, AddressTypeBits, ValueBits);

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

    #endregion

    #region RunningLogic

    public virtual void ExecuteStep()
    {
        //TODO:SZ - implement
    }

    #endregion
}
