using SpreadsheetGUI;
using SSGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ControllerTester
{
    class SpreadSheetViewStub : ISpreadsheetView
    {
        public string FileName { get; set; }
        public SpreadsheetPanel sp { get=> new SpreadsheetPanel() ; set=>new SpreadsheetPanel(); }

        public string Message { get; set; }

        public bool CalledDoClose
        {
            get; private set;
        }

        public bool CalledOpenNew
        {
            get; private set;
        }

        public bool CalledSetCellBox
        {
            get; set;
        }

        public bool CalledSetContentBox
        {
            get; set;
        }

        public bool CalledSetValueBox
        {
            get; set;
        }

        public bool CalledGoSaveAs
        {
            get; set;
        }

        public event Action NewEvent;
        public event Action<string> OpenEvent;
        public event Action SaveEvent;
        public event Action<string> SaveAsEvent;
        public event Action CloseEvent;
        public event Action<string> SetCellContentEvent;
        public event Action<string> ArrowKeyEvent;

        public void FireNewEvent()
        {
            NewEvent?.Invoke();
        }
        public void FireOpenEvent(string file)
        {
            OpenEvent?.Invoke(file);
        }
        public void FireSaveAsEvent(string s)
        {
            SaveAsEvent?.Invoke(s);
        }
        public void FireSaveEvent()
        {
            SaveEvent?.Invoke();
        }
        public void FireCloseEvent()
        {
            CloseEvent?.Invoke();
        }
        public void FireSetCellContentEvent(string content)
        {
            SetCellContentEvent?.Invoke(content);
        }
        public void FireArrowKeyEvent(string d)
        {
            ArrowKeyEvent?.Invoke(d);
        }
        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }
        
        public void setCellBox(string s)
        {
            CalledSetCellBox = true;
        }

        public void setContentBox(string value)
        {
            CalledSetContentBox = true;
        }

        public void setValueBox(string content)
        {
            CalledSetValueBox = true;
        }

        public void GoSaveAs()
        {
            CalledGoSaveAs = true;
        }
    }
}
