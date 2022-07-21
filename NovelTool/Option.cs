using System.Windows.Forms;

namespace NovelTool
{
    public partial class Option : Form
    {
        public Option()
        {
            InitializeComponent();
            OptionTreeView1.InitSettings(Properties.Settings.Default);
        }
    }
}
