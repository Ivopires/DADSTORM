using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace DADSTORM
{
    static class PCS
    {
        public static Dictionary<string, Operator> operatorsDict = new Dictionary<string, Operator>();

        static void Main(string[] args)
        {
            TcpChannel channel = new TcpChannel(10001);
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(PCServ), "PCService",
                WellKnownObjectMode.Singleton);
            System.Console.ReadLine();
        }

        public static string getAllFieldsOperator(string key)
        {
            //string joinAddressesToSend = String.Join("$", operatorsDict[key].sendAddresses);
            string joinPreviousAddresses = String.Join("$", operatorsDict[key].previousAddresses);

            string result = operatorsDict[key].id + " " +
                            operatorsDict[key].repID + " " +
                            operatorsDict[key].myAddress + " " +
                            operatorsDict[key].input + " " +
                            operatorsDict[key].status + " " +
                            operatorsDict[key].repFact + " " +
                            operatorsDict[key].type + " " +
                            operatorsDict[key].routing + " " +
                            operatorsDict[key].fieldNumber + " " +
                            operatorsDict[key].condition + " " +
                            operatorsDict[key].conditionValue + " " +
                            operatorsDict[key].dllCustom + " " +
                            operatorsDict[key].classCustom + " " +
                            operatorsDict[key].methodCustom + " " + 
                            joinPreviousAddresses;
            
            return result;
        }

        public static void setMyPreviousAddresses(string operatorID, int repID)
        {

            string operatorKey = operatorID + "-" + repID;

            foreach (string input in operatorsDict[operatorKey].input.Split('$'))
            {
                if (input.Contains("OP"))
                {
                    operatorsDict[operatorKey].inputOPs.Add(input);
                }
            }

            foreach (string previousNodeId in operatorsDict[operatorKey].inputOPs)
            {
                foreach (string previousOpRep in operatorsDict.Keys)
                {
                    if (previousOpRep.Contains(previousNodeId))
                    {
                        operatorsDict[operatorKey].previousAddresses.Add(operatorsDict[previousOpRep].myAddress);
                    }
                }
            }
            /*
            string[] separator = new string[] { "OP" };
            string[] previousStringID = operatorID.Split(separator, StringSplitOptions.None);
            int previousID = Int32.Parse(previousStringID[1]) - 1;
            string previousOperatorID = "OP" + previousID;
            //Debug.WriteLine("PREVIOUS ID = " + previousOperatorID);

            foreach (string previousOP in operatorsDict.Keys)
            {
                if (previousOP.Contains(previousOperatorID))
                {
                    operatorsDict[operatorKey].previousAddresses.Add(operatorsDict[previousOP].myAddress);
                }
            }
            */
        }

        public static string startOperator(string operatorID)
        {
            foreach (string key in PCS.operatorsDict.Keys)
            {
                if (key.Contains(operatorID))
                {
                    string operatorPath = BuildPaths.getExecPath("\\Operator\\bin\\Debug\\Operator.exe");
                    string operatorArgs = PCS.getAllFieldsOperator(key);
                    ProcessStartInfo operatorStartInfo = new ProcessStartInfo();
                    operatorStartInfo.FileName = operatorPath;
                    operatorStartInfo.Arguments = operatorArgs;
                    Process operatorProcess = Process.Start(operatorStartInfo);
                    Console.WriteLine("Comecei operador com id: " + operatorID + " e repID: " + PCS.operatorsDict[key].repID);
                }
            }
            return "Operator " + operatorID + " launched";
        }
    }

    class PCServ : MarshalByRefObject, PCServices
    {
        public string printHello()
        {
            System.Console.WriteLine("Sou o pcs");
            return "Sou o PuppetMaster";
        }

        public string changeInfo(string info)
        {
            string operatorPath = BuildPaths.getExecPath("\\Operator\\bin\\Debug\\Operator.exe");
            string operatorArgs = info;
            ProcessStartInfo operatorStartInfo = new ProcessStartInfo();
            operatorStartInfo.FileName = operatorPath;
            operatorStartInfo.Arguments = operatorArgs;
            Process operatorProcess = Process.Start(operatorStartInfo);
            Console.WriteLine("Comecei processo com info: " + info);
            return "PCS recebeu: " + info;
        }

        public string sendOperatorInfoToPCS(Dictionary<string, string> operatorDict)
        {
            Operator _operator = new Operator();
            _operator.type = operatorDict["TYPE"];
            _operator.id = operatorDict["OPERATOR_ID"];
            _operator.input = operatorDict["INPUT"];
            _operator.repFact = Int32.Parse(operatorDict["REP_FACT"]);
            _operator.routing = operatorDict["ROUTING"];
            if (operatorDict["FIELD_NUMBER"] != "")
            {
                _operator.fieldNumber = Int32.Parse(operatorDict["FIELD_NUMBER"]);
            }
            _operator.condition = operatorDict["CONDITION"];
            _operator.conditionValue = operatorDict["CONDITION_VALUE"];
            _operator.status = "Stand_by";
            _operator.dllCustom = operatorDict["DLL"];
            _operator.classCustom = operatorDict["CLASS"];
            _operator.methodCustom = operatorDict["METHOD"];
            
            int operatorNumber = 0;
            foreach (string address in operatorDict["ADDRESSES"].Split('$'))
            {
                _operator.myAddress = address;
                _operator.repID = operatorNumber;
                Operator finalOperator = new Operator();
                finalOperator.duplicate(_operator);
                PCS.operatorsDict.Add(finalOperator.id + "-" + finalOperator.repID, finalOperator);
                operatorNumber++;
                PCS.setMyPreviousAddresses(finalOperator.id, finalOperator.repID);
            }

            foreach (string key in PCS.operatorsDict.Keys)
            {
                Operator op = PCS.operatorsDict[key];
                op.print();
                Console.WriteLine("\r\n");
            }

            PCS.startOperator(_operator.id);
            return _operator.id + " recebido pelo PCS"; ;
        }

        public void resetPCS()
        {
            PCS.operatorsDict.Clear();
        }
    }

    public static class BuildPaths
    {
        public static string processPath;

        public static string getPath()
        {
            string currentPath = Directory.GetCurrentDirectory();
            string beforePath = Directory.GetParent(currentPath).ToString();
            string beforePath2 = Directory.GetParent(beforePath).ToString();
            processPath = Directory.GetParent(beforePath2).ToString();
            return processPath;
        }

        public static string getExecPath(string execPath)
        {
            processPath = getPath() + execPath;
            return processPath;
        }
    }
}
