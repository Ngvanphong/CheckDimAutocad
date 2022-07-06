using Autodesk.AutoCAD.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Windows;
using Autodesk.AutoCAD.Ribbon;

namespace CheckDimension
{
    internal class RibbonCommandHandler : System.Windows.Input.ICommand
    {

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            if (parameter is RibbonCommandButton)
            {
                RibbonCommandButton button = parameter as RibbonCommandButton;
                //string cmd = string.Format("{0}{1}", new string((char)03, 2), button.Id);
                //doc.SendStringToExecute(cmd, true, false, true);
                SendCommand(doc, button.Id);
            }
        }

        //public void CallBackFunction(String message)
        //{
        //    Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

        //    switch (message)
        //    {
        //        case "MyButtonID":
        //            doc.SendStringToExecute("checkdim",true,true,true);
        //            break;
        //    }
        //}

        private void SendCommand( Document doc, string command)
        {
            var acadDoc = doc.GetAcadDocument();
            acadDoc.GetType().InvokeMember(
                "SendCommand",
                System.Reflection.BindingFlags.InvokeMethod,
                null,
                acadDoc,
                new[] { command + "\n" });
        }
    }
}
