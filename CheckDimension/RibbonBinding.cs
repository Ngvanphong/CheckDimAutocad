
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using System.Reflection;
using Autodesk.Windows;
using CheckDimension.Properties;
using Autodesk.AutoCAD.Ribbon;
using CheckDimension.RibbonButtons;

[assembly: ExtensionApplication(typeof(CheckDimension.RibbonBinding))]
namespace CheckDimension
{
    public class RibbonBinding : IExtensionApplication
    {
        public void Initialize()
        {
            RibbonControl ribbon = ComponentManager.Ribbon;
            if (ribbon != null)
            {
                RibbonTab rtab = ribbon.FindTab(AppConstants.RibbonName);
                if (rtab == null)
                {
                    rtab = new RibbonTab();
                    rtab.Title = AppConstants.RibbonName;
                    rtab.Id = AppConstants.RibbonName;
                    ribbon.Tabs.Add(rtab);
                }
                else
                {
                    //ribbon.Tabs.Remove(rtab);
                }
                //Add the Tab
                RibbonPanel dimensionPanel = DimensionButton.AddOnePanel(AppConstants.Structural);
                rtab.Panels.Add(dimensionPanel);

                
            }
        }

        public void Terminate()
        {
            return;
        }



    }
}
