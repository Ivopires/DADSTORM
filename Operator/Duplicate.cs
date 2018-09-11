using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADSTORM
{
    class Duplicate : Operator
    {
        private List<string[]> tuples = new List<string[]>();

        public override void execute()
        {
            Console.WriteLine("EXECUTE DUPLICATE");

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
                    lock (this.outputTuples)
                    {
                        outputTuples.Add(inputTuple);
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
