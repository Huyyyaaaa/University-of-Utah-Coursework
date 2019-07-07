using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{    
    class SpreadsheetApplicationContext : ApplicationContext
    {
        // Number of windows that are open
        private int windowCount = 0;

        // Singleton ApplicationContext
        private static SpreadsheetApplicationContext context;

        /// <summary>
        /// Private constructor for singleton pattern
        /// </summary>
        private SpreadsheetApplicationContext()
        {
        }

        /// <summary>
        /// Returns the one DemoApplicationContext
        /// </summary>
        /// <returns></returns>
        public static SpreadsheetApplicationContext GetContext()
        {
            if(context == null)
            {
                context = new SpreadsheetApplicationContext();
            }
            return context;
        }

        /// <summary>
        /// Run the form in this application context
        /// </summary>
        public void RunNew()
        {
            // Creates a window and a controller
            SpreadsheetGUI window = new SpreadsheetGUI();
            new Controller(window);

            // Add to the count
            windowCount++;

            // Check for whether the window has been closed
            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };

            // Run the form
            window.Show();
        }


    }
}
