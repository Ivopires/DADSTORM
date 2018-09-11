using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DADSTORM
{
    class Uniq : Operator
    {
        private List<string[]> uniqTuples = new List<string[]>();

        public override void execute()
        {
            Console.WriteLine("EXECUTE UNIQ");

            while (true)
            {
                string[] inputTuple;
                if (this.inputTuples.Count != 0)
                {
                    lock (this.inputTuples)
                    {
                        inputTuple = (string[])this.inputTuples[0].Clone();
                    }
                    Console.WriteLine("execute tuple -> " + inputTuple[0]);
                    if (!uniqTuples.Exists(x => x[this.fieldNumber - 1].Equals(inputTuple[this.fieldNumber - 1]))) //MAIS ORGULHO
                    {
                        uniqTuples.Add(inputTuple);
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
