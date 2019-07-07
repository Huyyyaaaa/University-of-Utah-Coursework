using SSGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public interface ISpreadsheetView
    {
        string FileName { get;  set; }

        SpreadsheetPanel sp { get; set; }

        string Message { set; }

               
        event Action NewEvent;

        event Action<string> OpenEvent;

        event Action SaveEvent;

        event Action<string> SaveAsEvent;

        event Action CloseEvent;

        event Action<string> SetCellContentEvent;

        event Action<string> ArrowKeyEvent;
                
        void OpenNew();

        void DoClose();

        void setContentBox(string value);

        void setValueBox(string content);

        void setCellBox(string s);

        void GoSaveAs();
    }
}
