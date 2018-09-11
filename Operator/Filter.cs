using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADSTORM
{
    class Filter :Operator
    {
        public override void execute()
        {
            Console.WriteLine("EXECUTE FILTER");

            while (true)
            {
                if (this.inputTuples.Count != 0)
                {
                    string[] inputTuple;
                    lock (this.inputTuples)
                    {
                        inputTuple = (string[])this.inputTuples[0].Clone();
                    }
                    Console.WriteLine("execute tuple -> " + inputTuple[this.fieldNumber - 1]);
                    Console.WriteLine("cond val -> " + this.conditionValue);

                        //int compare = inputTuple[this.fieldNumber].CompareTo(this.conditionValue);

                        int compare = String.Compare(inputTuple[this.fieldNumber - 1].Trim('"'), this.conditionValue);

                        if (this.condition.Equals("=") && compare == 0)
                        {
                        lock (this.outputTuples)
                        {
                            outputTuples.Add(inputTuple);
                        }
                    }   
                        if (this.condition.Equals(">") && compare > 0)
                        {
                        lock (this.outputTuples)
                        {
                            outputTuples.Add(inputTuple);
                        }
                    }
                        if (this.condition.Equals("<") && compare < 0)
                        {
                        lock (this.outputTuples)
                        {
                            outputTuples.Add(inputTuple);
                        }
                    }
                    lock (this.inputTuples)
                    {
                        inputTuples.RemoveAt(0);
                    }
                }
            }
        }
    }
}
