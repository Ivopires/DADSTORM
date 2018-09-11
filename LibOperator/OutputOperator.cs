using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DADSTORM
{
    public class OutputOperator
    {
        public IList<IList<string>> CustomOperation(IList<string> l)
        {
            string outputFile = @".\output.txt";

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(outputFile, true))
            {
                //file.WriteLine(l);
                int cont = 0;
                foreach (string line in l)
                {
                    cont++;
                    file.Write(line);
                    if (cont < l.Count)
                        file.Write(",");
                }
                file.WriteLine();
            }
            IList<IList<string>> result = new List<IList<string>>();
            result.Add(l);
            return result;
        }
    }
}
