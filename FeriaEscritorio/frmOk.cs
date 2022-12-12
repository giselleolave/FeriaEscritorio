using MaterialSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;

namespace FeriaEscritorio
{
    public partial class frmOk : MaterialSkin.Controls.MaterialForm
    {
        MaterialSkin.MaterialSkinManager SkinManager;
        public frmOk()
        {
            InitializeComponent();
            SkinManager = MaterialSkinManager.Instance;
            SkinManager.AddFormToManage(this);
            SkinManager.Theme = MaterialSkinManager.Themes.DARK;
            SkinManager.ColorScheme = new ColorScheme(Primary.Cyan300, Primary.Green400, Primary.Blue50, Accent.Cyan700, TextShade.WHITE);
            
        }

      public string Message
        {
            get { return lblMessage.Text; }
            set { lblMessage.Text = value; }
        }
    }
}
