using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Collections;

namespace psfire
{
    class Program
    {
        static void Main(string[] args)
        {
            // Enter b64 PS code here. Empire stager perhaps?
            // Sample code below is $PSVersionTable
            string codetorun = "JABQAFMAVgBlAHIAcwBpAG8AbgBUAGEAYgBsAGUA";

            //Uncomment to read b64 encoded PS from .\launcher.txt
            //string codetorun = System.IO.File.ReadAllText(string.Format("{0}\\launcher.txt", cwd));

            byte[] decodedBytes = Convert.FromBase64String(codetorun);
            string decodedText = Encoding.Unicode.GetString(decodedBytes);
            //Console.WriteLine(string.Format("Code to run: {0}\n", decodedText));

            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(decodedText);
            Collection<PSObject> results = pipeline.Invoke();

            // Process output.
            // convert the script result into a single string 
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                //Console.WriteLine(obj.BaseObject.GetType().FullName);
                Hashtable hash = obj.BaseObject as Hashtable;
                if (hash != null)
                {
                    foreach (string k in hash.Keys)
                    {
                        //Comment out if using during engagement.
                        Console.WriteLine(string.Format("{0} - {1}", k, hash[k]));
                    }
                }
            }
            
            runspace.Close();

            // Uncomment for demo purposes
            //Console.ReadLine();
        }
    }
}
