using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.IO;
using System.Diagnostics;
using System.Collections;

namespace DADSTORM
{

    class PuppetMaster
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public static PCServices services;
        public static OperatorServices opServices;
        public static Form1 formPuppetMaster = new Form1();
        public static TcpChannel channel = new TcpChannel(10000);
        private static Dictionary<string, string> operatorsAddresses = new Dictionary<string, string>(); //addresses by id TODOOOOO
        private static string[] fileLines = null;
        private static int lastLine = 0;

        [STAThread]
        static void Main()
        {
            initPuppetMasterChannel();
            connectToPCS();

            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(formPuppetMaster);
        }

        private static void initPuppetMasterChannel()
        {
            ChannelServices.RegisterChannel(channel, false);
            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(PuppetMasterServicesClass),
                "PuppetMasterServices",
                WellKnownObjectMode.Singleton);
        }

        private static void connectToPCS()
        {
            services = (PCServices)Activator.GetObject(
                typeof(PCServices),
                "tcp://localhost:10001/PCService");
        }

        public static void reset()
        {
            lastLine = 0;
            operatorsAddresses.Clear();
            fileLines = null;
            services.resetPCS();
    }

        public static void startReadingConfigFile(string filepath, bool step)
        {
            PuppetMasterReadConfig readConfig = new PuppetMasterReadConfig();

            //TODO FileNotFound Exception

            if (fileLines == null)
            {
                fileLines = File.ReadAllLines(filepath);
            }

            int limit = 0;
            if (step)
            {
                limit = lastLine + 1;
                if (limit > fileLines.Length) limit = fileLines.Length;
            }
            else
            {
                limit = fileLines.Length;
            }

            for (int i = lastLine; i < limit; i++)
            {
                lastLine++;
                string[] line = fileLines[i].Split();
                if (!String.IsNullOrEmpty(line[0]))
                {
                    int cont = i + 1;
                    Debug.WriteLine("line "  + cont + "=" + line[0]);
                    Dictionary<string, string> lineContentDictionary = readConfig.readLine(line);
                    string lineID = lineContentDictionary["LINE_ID"];
                    Debug.WriteLine("lineID = " + lineID);

                    if (lineID.Equals("OP"))
                    {
                        int replicaCount = 0;
                        string operatorID = lineContentDictionary["OPERATOR_ID"];
                        foreach(string address in lineContentDictionary["ADDRESSES"].Split('$'))
                        {
                            string replicaID = operatorID + "-" + replicaCount;
                            operatorsAddresses.Add(replicaID, address);
                            replicaCount++;
                        }

                        foreach (string key in lineContentDictionary.Keys)
                        {
                            string value = lineContentDictionary[key];
                            PuppetMaster.formPuppetMaster.addNewLineToLog(key + " = " + value);
                        }
                        PuppetMaster.formPuppetMaster.addNewLineToLog("\r\n");
                        services.sendOperatorInfoToPCS(lineContentDictionary);
                    }

                    else if (lineID.Equals("START"))
                    {
                        string operatorID = lineContentDictionary["OPERATOR_ID"];
                        PuppetMaster.formPuppetMaster.addNewLineToLog("PCS is about to start: " + operatorID);

                        foreach (string replicaID in operatorsAddresses.Keys)
                        {
                            if (replicaID.Contains(operatorID))
                            {
                                Debug.WriteLine("REPLICA ADDRESS = " + operatorsAddresses[replicaID]);
                                PuppetMaster.formPuppetMaster.addNewLineToLog("REPLICA ADDRESS = " + operatorsAddresses[replicaID]);
                                opServices = (OperatorServices)Activator.GetObject(
                                            typeof(OperatorServices),
                                            operatorsAddresses[replicaID]);

                                opServices.startToProcess();
                            }
                        }
                    }

                    else if (line.Equals("INTERVAL"))
                    {
                        //TODO
                    }

                    else if (line.Equals("STATUS"))
                    {
                        //TODO
                    }

                    else if (line.Equals("CRASH"))
                    {
                        //TODO
                    }

                    else if (line.Equals("FREEZE"))
                    {
                        //TODO
                    }

                    else if (line.Equals("UNFREEZE"))
                    {
                        //TODO
                    }

                    else if (line.Equals("WAIT"))
                    {
                        //TODO
                    }
                }
            }
        }
    }

    class PuppetMasterServicesClass: MarshalByRefObject, PuppetMasterServices
    {
        public string operatorConnected (String msgFromOperator)
        {       
            PuppetMaster.formPuppetMaster.addNewLineToLog("Msg from Operator: " + msgFromOperator);
            return msgFromOperator;
        }
    }
}
