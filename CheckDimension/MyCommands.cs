using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;
using System.Reflection;

[assembly: CommandClass(typeof(CheckDimension.MyCommands))]
namespace CheckDimension
{
    public class MyCommands
    {
        [CommandMethod("checkdim")]
        public void DimensionShow()
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor editor = doc.Editor;
            Database db = doc.Database;

            CheckDimAppShow.wpfCheckDim= new wpfCheckDim();
            Application.ShowModalWindow(Application.MainWindow.Handle, CheckDimAppShow.wpfCheckDim, false);
            string folderInput = CheckDimAppShow.wpfCheckDim.txtFolderInput.Text;
            string folderOutput = CheckDimAppShow.wpfCheckDim.txtFolderOutput.Text;
            if (string.IsNullOrEmpty(folderInput)||string.IsNullOrEmpty(folderOutput)) return;
            IList<Dimension> listDim= new List<Dimension>();
            using(Transaction t= doc.TransactionManager.StartOpenCloseTransaction())
            {
                BlockTableRecord tableRecord = t.GetObject(db.CurrentSpaceId, OpenMode.ForRead) as BlockTableRecord;
                foreach(ObjectId objId in tableRecord)
                {
                    Dimension dim = t.GetObject(objId, OpenMode.ForWrite) as Dimension;
                    if (dim != null)
                    {
                        listDim.Add(dim);
                        if (!string.IsNullOrEmpty(dim.DimensionText))
                        {
                            if (dim.DimensionText != "")
                            {
                                //dim.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 0, 0);
                                dim.Dimclrt= Autodesk.AutoCAD.Colors.Color.FromRgb(255, 0, 0);
                            }
                        }
                    }
                }
                t.Commit();
            }
           
        }
    }
}
