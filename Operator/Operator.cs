using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace DADSTORM
{
    public class Operator : MarshalByRefObject, OperatorServices
    {
        public string id = "";
        public int repID = -1;
        public string myAddress = "";
        public string input = "";
        public List<string> inputOPs = new List<string>();
        public List<string> inputFile = new List<string>();
        public string status = "";
        public int repFact = 0;
        public string type = "";
        public string routing = "";
        public int fieldNumber = -1;
        public string condition = "";
        public string conditionValue = "";
        public string dllCustom = "";
        public string classCustom = "";
        public string methodCustom = "";
        public Dictionary<string, string> sendAddresses = new Dictionary<string, string>();
        public List<string> previousAddresses = new List<string>();

        public static TcpChannel channel;
        public static Operator _operator;
        public static OperatorServices operatorServices;
        public List<string[]> inputTuples = new List<string[]>();
        public List<string[]> outputTuples = new List<string[]>();

        public Operator() {}

        static void Main(string[] args)
        {
            Console.WriteLine("SIZE ARGS = " + args.Length);
            string operatorType = args[6];
            switch (operatorType){
                case "COUNT":
                    _operator = new Count();
                    break;
                case "UNIQ":
                    _operator = new Uniq();
                    break;
                case "FILTER":
                    _operator = new Filter();
                    break;
                case "DUP":
                    _operator = new Duplicate();
                    break;
                case "CUSTOM":
                    _operator = new Custom();
                    break;
                default:
                    _operator = new Operator();
                    break;
            }
            _operator.id = args[0];
            _operator.repID = Int32.Parse(args[1]);
            _operator.myAddress = args[2];
            channel = new TcpChannel(getPort(_operator.myAddress));
            foreach (string input in args[3].Split('$'))
            {
                if (input.Contains(".dat"))
                {
                    _operator.inputFile.Add(input);
                } else if (input.Contains("OP"))
                {
                    _operator.inputOPs.Add(input);
                } else
                {
                    System.Console.WriteLine("unrecognized input!");
                }
            }
            //_operator.input = args[3];
            _operator.status = args[4];
            _operator.repFact = Int32.Parse(args[5]);
            _operator.type = args[6];
            _operator.routing = args[7];
            if (_operator.type.Equals("UNIQ"))
            {
                _operator.fieldNumber = Int32.Parse(args[8]);
                if (args.Length > 9)
                    _operator.previousAddresses = previousAddressesToList(args[9]);
            }
            else if (_operator.type.Equals("FILTER"))
            {
                _operator.fieldNumber = Int32.Parse(args[8]);
                _operator.condition = args[9];
                _operator.conditionValue = args[10];
                if (args.Length > 11)
                    _operator.previousAddresses = previousAddressesToList(args[11]);
            }
            else if (_operator.type.Equals("DUP") || _operator.type.Equals("COUNT"))
            {
                if (args.Length > 9)
                    _operator.previousAddresses = previousAddressesToList(args[9]);
            }
            else if (_operator.type.Equals("CUSTOM"))
            {
                _operator.dllCustom = args[9];
                _operator.classCustom = args[10];
                _operator.methodCustom = args[11];
                if (args.Length > 12)
                    _operator.previousAddresses = previousAddressesToList(args[12]);
            }

            ChannelServices.RegisterChannel(channel, true);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(Operator),
                "op",
                WellKnownObjectMode.Singleton);

            sendAddressesToPreviousOP();

            System.Console.ReadLine(); //So para manter a janela aberta
        }

        private static void sendAddressesToPreviousOP()
        {
            foreach (string address in _operator.previousAddresses)
            {
                operatorServices = (OperatorServices)Activator.GetObject(
                                          typeof(OperatorServices),
                                          address);
                string replica = _operator.id + "-" + _operator.repID;
                operatorServices.setSendAddresses(replica, _operator.myAddress);
            }
        }

        private static List<string> previousAddressesToList(string addressesToSend)
        {
            List<string> listAddresses = new List<string>();
            foreach (string address in addressesToSend.Split('$'))
            {
                listAddresses.Add(address);    
            }
            return listAddresses;
        }

        private static int getPort(string address)
        {
            int port = Int32.Parse(address.Split(':')[2].Split('/')[0]);
            return port;
        }

        private static void readFileToInputBuffer(string file)
        {
            string[] lines = File.ReadAllLines(file);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(new[] { ", " }, StringSplitOptions.None);
                if (!String.IsNullOrEmpty(line[0]))
                {
                    if (!line[0].StartsWith("%%"))
                    {
                        string[] fields = new string[line.Length];
                        for (int j = 0; j < line.Length; j++)
                        {
                            fields[j] = line[j];
                        }
                        _operator.inputTuples.Add(fields);
                    }
                }
            }
        }

        private void connectToPuppetMaster()
        {
            PuppetMasterServices services = (PuppetMasterServices)Activator.GetObject(
                typeof(PuppetMasterServices),
                "tcp://localhost:10000/PuppetMasterServices");

            System.Console.WriteLine(services.operatorConnected("Operator " + this.id + " with repID " + this.repID + " has started"));
        }

        public void duplicate(Operator _operator)
        {
            this.id = _operator.id;
            this.repID = _operator.repID;
            this.myAddress = _operator.myAddress;
            this.input = _operator.input;
            //this.inputOPs = _operator.inputOPs;
            //this.inputFile = _operator.inputFile;
            this.status = _operator.status;
            this.repFact = _operator.repFact;
            this.type = _operator.type;
            this.fieldNumber = _operator.fieldNumber;
            this.condition = _operator.condition;
            this.conditionValue = _operator.conditionValue;
            this.routing = _operator.routing;
            this.dllCustom = _operator.dllCustom;
            this.classCustom = _operator.classCustom;
            this.methodCustom = _operator.methodCustom;
        }

        public void print()
        {
            Console.WriteLine("ID = " + this.id);
            Console.WriteLine("REP_ID = " + this.repID);
            Console.WriteLine("MY_ADDRESS = " + this.myAddress);
            foreach (string input in this.inputFile)
            {
                Console.WriteLine("INPUT_FILE = " + input);
            }
            foreach (string input in this.inputOPs)
            {
                Console.WriteLine("INPUT_OP = " + input);
            }
            Console.WriteLine("STATUS = " + this.status);
            Console.WriteLine("REP_FACT = " + this.repFact);
            Console.WriteLine("TYPE = " + this.type);
            Console.WriteLine("FIELD_NUMBER = " + this.fieldNumber);
            Console.WriteLine("CONDITION = " + this.condition);
            Console.WriteLine("CONDITION_VALUE = " + this.conditionValue);
            int cont = 0;
            foreach (string address in this.sendAddresses.Values)
            {
                Console.WriteLine("ADDRESS TO SEND " + cont + ": " + address);
                cont++;
            }
            cont = 0;
            foreach (string address in this.previousAddresses)
            {
                Console.WriteLine("PREVIOUS " + cont + ": " + address);
                cont++;
            }
            Console.WriteLine("ROUTING = " + this.routing);
            Console.WriteLine("DLL = " + this.dllCustom);
            Console.WriteLine("CLASS = " + this.classCustom);
            Console.WriteLine("METHOD = " + this.methodCustom);
        }

        public List<string[]> readInputFromFile(string inputFilepath)
        {
            List<string[]> inputList = new List<string[]>();
            string[] lines = File.ReadAllLines(inputFilepath);
            for (int i=0; i < lines.Length; i++)
            {
                string[] line = lines[i].Split(new[] {", "}, StringSplitOptions.None);
                if (!String.IsNullOrEmpty(line[0])){
                    if (!line[0].StartsWith("%%"))
                    {
                        string[] fields = new string[line.Length];
                        for (int j=0; j < line.Length; j++)
                        {
                            fields[j] = line[j];
                        }
                        inputList.Add(fields);
                    }
                }
            }
            return inputList;
        }

        public void printTuples(List<string[]> tuples)
        {
            foreach (string[] _tuple in tuples){
                for (int i=0; i<_tuple.Length; i++)
                {
                    Console.WriteLine(i + " = " + _tuple[i]);
                }
            }
        }

        public virtual void execute()
        {
            Console.WriteLine("GENERAL OPERATOR");
        }

        public void startToProcess()
        {
            _operator.print();

            Thread executeThread = new Thread(_operator.execute);
            executeThread.Start();

            Thread sendTuplesThread = new Thread(_operator.sendTuples);
            sendTuplesThread.Start();

            foreach (string file in _operator.inputFile)
            {
                readFileToInputBuffer(file);
            }
        }

        public void sendTuples()
        {
            Console.WriteLine("sendTuples starting");
            while (true)
            {
                if (_operator.outputTuples.Count != 0)
                {
                    string[] outputTuple;
                    lock (_operator.outputTuples)
                    {
                        outputTuple = (string[])_operator.outputTuples[0].Clone();
                    }
                    
                    lock (_operator.sendAddresses)
                    {
                        switch (_operator.routing)
                        {
                            case "primary":
                                //Console.WriteLine("\n\r send adress -> " + _operator.sendAddresses.Count);
                                if (_operator.sendAddresses.Count != 0)
                                {
                                    foreach (string replicaID in _operator.sendAddresses.Keys)
                                    {
                                        if (replicaID.Contains("0"))
                                        {
                                            string sendAddress = _operator.sendAddresses[replicaID];
                                            //Console.WriteLine("SEND adress -> " + sendAddress + " tuple -> " + outputTuple[0]);
                                            operatorServices = (OperatorServices)Activator.GetObject(
                                                                typeof(OperatorServices),
                                                                sendAddress);
                                            operatorServices.exchangeTuples(outputTuple);
                                            lock (_operator.outputTuples)
                                            {
                                                _operator.outputTuples.RemoveAt(0);
                                            }
                                        }
                                    }
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        public void exchangeTuples(string[] tuple)
        {
            Console.WriteLine("RECIEVE tuple = " + tuple[1]);
            lock(_operator.inputTuples){
                _operator.inputTuples.Add(tuple);
            }
        }

        public void setSendAddresses(string operatorID, string sendAddress)
        {
            lock (_operator.sendAddresses)
            {
                _operator.sendAddresses.Add(operatorID, sendAddress);
            }
        }
    }
}
