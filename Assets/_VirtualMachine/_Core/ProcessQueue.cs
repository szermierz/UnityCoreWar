
using System;
using System.Collections;
using System.Collections.Generic;

namespace VirtualMachine
{
    public class ProcessQueue
    {
        public Process SpawnProcess(string queueID, MemoryModel model, int startAddress)
        {
            var result = new Process(this, model, startAddress);
            this[queueID].Enqueue(result);
            return result;
        }

        protected readonly Dictionary<string, Queue<Process>> m_Processes = new Dictionary<string, Queue<Process>>();

        protected virtual Queue<Process> this[string queueID]
        {
            get
            {
                if(!m_Processes.ContainsKey(queueID))
                    m_Processes.Add(queueID, new Queue<Process>());

                return m_Processes[queueID];
            }
        }

        public virtual Process.ExecutionResult Execute(string queueID)
        {
            if(!m_Processes.ContainsKey(queueID))
                return new Process.ExecutionResult(Process.ExecutionResult.ResultType.CouldntStart);

            var queue = this[queueID];
            if(queue.Count == 0)
                return new Process.ExecutionResult(Process.ExecutionResult.ResultType.CouldntStart);

            var process = queue.Dequeue();

            var result = process.ExecuteStep();

            queue.Enqueue(process);

            return result;
        }
    }
}
