using System;
using System.Threading;
using System.Threading.Tasks;

namespace Synchronization
{
    public class Synch
    {
        /// <summary>
        /// Demonstrates synchronization of Tasks.
        /// </summary>
        public static void Main()
        {
            new Synch().Compute(100000000);
            Console.ReadLine();
        }

        /// <summary>
        /// This variable is shared by the two tasks created below.
        /// </summary>
        private int Total;

        /// <summary>
        /// Creates two tasks.  One increments Total n times, the other
        /// decrements Total n times.  The value of Total is displayed
        /// every 500 msec.
        /// </summary>
        public void Compute(int n)
        {
            Task t1 = new Task(() => this.Increment(n));
            Task t2 = new Task(() => this.Decrement(n));
            t1.Start();
            t2.Start();

            while (!t1.IsCompleted || !t2.IsCompleted)
            {
                Console.WriteLine(Total);
                Thread.Sleep(500);
            }

            Console.WriteLine("Finished: " + Total);
            Console.ReadLine();
        }

        /// <summary>
        /// Increments Total n times.
        /// </summary>
        public void Increment(int n)
        {
            while (n > 0)
            {
                Interlocked.Increment(ref Total);
                n--;
            }
            Console.WriteLine("Add done");
        }

        /// <summary>
        /// Decements Total n times.
        /// </summary>
        public void Decrement(int n)
        {
            while (n > 0)
            {
                Interlocked.Decrement(ref Total);
                n--;
            }
            Console.WriteLine("Sub done");
        }
    }
}
