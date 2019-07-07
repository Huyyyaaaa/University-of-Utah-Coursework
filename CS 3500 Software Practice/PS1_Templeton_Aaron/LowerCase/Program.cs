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
        /// takes input from standard in and writes the output using standard
        /// out and converts all the characters to lowercase. output will be printed one 
        /// word per line
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            String input;
            int i = 0;
            while((input = Console.ReadLine()) != null)
            {
               
                var words = input.Split(' ');
                foreach (String word in words)
                {
                    if (String.IsNullOrWhiteSpace(word))
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine(word.ToLower());
                    }
                    
                }

                i++;
            }
           
        }
    }
}
            