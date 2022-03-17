﻿using System;
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
using System.IO;

[assembly: CommandClass(typeof(CheckDimension.MyCommands))]
namespace CheckDimension
{
    public class MyCommands
    {
        [CommandMethod("checkdim")]
        public void DimensionShow()
        {

            CheckDimAppShow.wpfCheckDim = new wpfCheckDim();
            Application.ShowModalWindow(Application.MainWindow.Handle, CheckDimAppShow.wpfCheckDim, false);
            string folderInput = CheckDimAppShow.wpfCheckDim.txtFolderInput.Text;
            string folderOutput = CheckDimAppShow.wpfCheckDim.txtFolderOutput.Text;
            if (string.IsNullOrEmpty(folderInput) || string.IsNullOrEmpty(folderOutput)) return;
            DirectoryInfo directoryInfor= new DirectoryInfo(folderInput);
            FileInfo[] fileDwgs = directoryInfor.GetFiles("*.dwg");
            Database currentDatabase = Application.DocumentManager.MdiActiveDocument.Database;
           
            foreach(var drawing in fileDwgs)
            {
                using (Database db = new Database(false, true))
                {
                    try
                    {
                        db.ReadDwgFile(drawing.FullName, FileOpenMode.OpenForReadAndAllShare, false, null);
                        HostApplicationServices.WorkingDatabase = db;
                        bool isEditDim = CheckEditDim(db);
                        if (isEditDim)
                        {
                            string fileSave = folderOutput + @"\" + drawing.Name.Split('.').First() + "_DimChecked" + ".dwg";
                            db.SaveAs(fileSave, DwgVersion.Current);
                        }
                    }
                    catch { }
                   
                }
                HostApplicationServices.WorkingDatabase = currentDatabase;
            }
            Application.ShowAlertDialog("Successed");
        }

        private void FixTextDim(ref Dimension dim)
        {
            dim.Dimclrt= Autodesk.AutoCAD.Colors.Color.FromRgb(255, 255, 0);
            dim.Dimtxt = dim.Dimtxt * 2;
            string oldValue = dim.DimensionText;
            string newValue = dim.DimensionText +"\n\r"+ @"[実寸="+Math.Round(dim.Measurement,3).ToString()+"]";
            dim.DimensionText = newValue;
        }

        private void ExploreAllBlock(BlockReference blockRef)
        {
            DBObjectCollection entitySet = new DBObjectCollection();
            blockRef.Explode(entitySet);
            foreach (DBObject obj in entitySet)
            {
                BlockReference blockItem = obj as BlockReference;
                
            }
        }
        private bool CheckEditDim(Database db)
        {
            try
            {
                using (Transaction t2 = db.TransactionManager.StartTransaction())
                {
                    BlockTable blockTable = t2.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord blockTableRecord = t2.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    foreach (ObjectId objId in blockTableRecord)
                    {
                        BlockReference blockRef = t2.GetObject(objId, OpenMode.ForWrite) as BlockReference;
                        if (blockRef != null)
                        {
                            DBObjectCollection entitySet = new DBObjectCollection();
                            blockRef.Explode(entitySet);
                            foreach (DBObject obj in entitySet)
                            {
                                if(obj is Entity)
                                {
                                    blockTableRecord.AppendEntity((Entity)obj);
                                    t2.AddNewlyCreatedDBObject(obj,true);
                                }
                            }
                            blockRef.Erase();
                            blockRef.Dispose();
                        }

                    }
                    t2.Commit();
                }
            }
            catch { }

            bool isEditDim = false;
            using (Transaction t = db.TransactionManager.StartTransaction())
            {
                BlockTable blockTable;
                blockTable = t.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord blockTableRecord;
                blockTableRecord = t.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                foreach (ObjectId objId in blockTableRecord)
                {
                    Dimension dim = t.GetObject(objId, OpenMode.ForWrite) as Dimension;
                    if (dim != null)
                    {
                        if (!string.IsNullOrEmpty(dim.DimensionText))
                        {
                            if (dim.DimensionText != "")
                            {
                                FixTextDim(ref dim);
                                isEditDim = true;
                                //var locationText = dim.TextPosition;
                                //Circle circle = new Circle();
                                //circle.SetDatabaseDefaults();
                                //circle.Center = locationText;
                                //circle.Radius = 2000;
                                //circle.Color = Autodesk.AutoCAD.Colors.Color.FromRgb(255, 0, 0);
                                //blockTableRecord.AppendEntity(circle);
                                //t.AddNewlyCreatedDBObject(circle, true);
                            }
                        }
                    }

                }
                t.Commit();
            }
            return isEditDim;
        }
    }
}
