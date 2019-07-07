using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;

namespace ControllerTester
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
        }

        [TestMethod]
        public void Test_Method2()
        {
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireNewEvent();
            Assert.IsTrue(stub.CalledOpenNew);
        }
        [TestMethod]
        public void Test_Method3()
        {
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireSetCellContentEvent("hello");
            stub.FireOpenEvent("");
            

        }

        [TestMethod]
        public void Test_Method4()
        {
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireSaveEvent();
            
        }

        [TestMethod]
        public void Test_Method5()
        {
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireSaveAsEvent("s");
        }

        [TestMethod]
        public void Test_Method6()
        {
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireArrowKeyEvent("up");
            stub.FireArrowKeyEvent("down");
            stub.FireArrowKeyEvent("left");
            stub.FireArrowKeyEvent("right");
            
        }

        [TestMethod]
        public void Test_Method7()
        {
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireSetCellContentEvent("hello");
            stub.FireCloseEvent();
        }

        [TestMethod]
        public void Test_Method8()
        {
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireSetCellContentEvent("=5+5");
           
        }

        [TestMethod]
        public void Test_Method9()
        {
            string path = @"..\..\dfs.ss";
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireOpenEvent(path);
        }

        [TestMethod]
        public void Test_Method10()
        {
            string path = @"..\..\2s.txt";
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireOpenEvent(path);
        }

        [TestMethod]
        public void Test_Method11()
        {
            
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FileName = "file";
            stub.FireSetCellContentEvent("text");
            stub.FireSaveEvent();
        }

        [TestMethod]
        public void Test_Method12()
        {

            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireSetCellContentEvent("=5+twenty");
            
        }

        [TestMethod]
        public void Test_Method13()
        {

            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireSetCellContentEvent("=A1");

        }

        [TestMethod]
        public void Test_Method14()
        {
            SpreadSheetViewStub stub = new SpreadSheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireSaveAsEvent("");
        }



    }
}
