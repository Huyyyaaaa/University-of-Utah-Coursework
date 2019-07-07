using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//unid: u0734119
//name: Aaron Templeton

namespace PS1
{
    class Program
    {
        /// <summary>
        /// takes input from standard and and checks to see if the input is a palindrome.
        /// if true, the palindrome is printed out using standard out
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string word;
            while((word = Console.ReadLine())!= null)
            {
                char[] arr = word.ToCharArray();
                Array.Reverse(arr);
                string reverse = new string(arr);

                if (reverse == word)
                {
                    Console.WriteLine(word);
                }
            }
            
        }
    }
}
