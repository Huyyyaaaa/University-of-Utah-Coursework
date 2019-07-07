using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//uNID: u0734119
//name: Aaron Templeton

namespace PS1
{
    class Program
    {
        /// <summary>
        /// takes a positive integer n as command line argument. if no argument is given n=1.
        /// takes input from standard in and inserts a line break every n words.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            
            int n;
            if (args.Length == 0 || int.Parse(args[0])<1)
            {
                n = 1;
            }
            else
            {
                n = int.Parse(args[0]);
            }

            string line;
            int i = 1;
            while ((line = Console.ReadLine()) != null)
            {
                var words = line.Split(' ');
                foreach (string w in words)
                {

                    if (String.IsNullOrWhiteSpace(w))
                    {
                        continue;
                    }

                    if (i % n == 0)
                    {
                        Console.WriteLine(w);
                    }
                    else
                    {
                        Console.Write(w+" ");
                    }

                    i++;

                }
                
            }
           
        }
    }
}
