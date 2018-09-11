using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADSTORM
{
    class Count : Operator 
    {
        private int count = 0;
        public override void execute()
        {

            Console.WriteLine("EXECUTE COUNT");

            while (true)
            {
                string[] inputTuple;
                if (this.inputTuples.Count != 0)
                {
                    lock (this.inputTuples)
                    {
                        inputTuple = (string[])this.inputTuples[0].Clone();
                    }
                    count++;
                    Console.WriteLine("count: " + count);
                    Console.WriteLine("execute tuple -> " + inputTuple[0]);
                    lock (this.inputTuples)
                    {
                        inputTuples.RemoveAt(0);
                    }
                }
            }
        }
    }
}
