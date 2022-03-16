using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;

namespace CheckDimension
{
    public class Initialization : IExtensionApplication
    {
        public void Initialize()
        {
            //new MyCommands().DimensionShow();
        }

        public void Terminate()
        {
            
        }
    }
}
