using Autodesk.AutoCAD.Ribbon;
using Autodesk.Windows;
using CheckDimension.Properties;

namespace CheckDimension.RibbonButtons
{
    public static class DimensionButton
    {
        public static RibbonPanel AddOnePanel(string panelName)
        {
            RibbonCommandButton rb;
            RibbonPanelSource rps = new RibbonPanelSource();
            rps.Title = panelName;
            RibbonPanel rp = new RibbonPanel();
            rp.Source = rps;

            rb = new RibbonCommandButton();
            rb.Name = "CheckDim";
            rb.ShowText = true;
            rb.Text = "Check \r\n Dimension";
            rb.Id = "checkdim";

            rb.ShowImage = true;
            var image = Extensions.GetImageSource(Resources.icons8_print_32);
            rb.LargeImage = image;
            rb.Image = image;
            rb.Orientation = System.Windows.Controls.Orientation.Vertical;
            rb.Size = RibbonItemSize.Large;
            rb.CommandHandler = new RibbonCommandHandler();
            

            //Add the Button to the Tab
            rps.Items.Add(rb);
            return rp;
        }
    }
}