using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _Default : System.Web.UI.Page
    {
        //string menubar = "<asp:Button ID='Button1' runat='server' onclick='Button1_Click' Text='Параметры' Width='113px' />";
        protected ConfigAction GetConfig = new ConfigAction();

        protected void ShowConfig() 
        {
            Label[] paramname = new Label[5];
            TextBox[] paramval = new TextBox[4];

            for (int i = 0; i < 5; i++) paramname[i] = new Label();

            for (int i = 0; i < 4; i++) paramval[i] = new TextBox();

            paramname[0].Text = "параметры";
            this.Page.Form.Controls.Add(paramname[0]);


            paramname[1].Text = "путь к дирекотрии"; paramval[0].Text = GetConfig.Parametrs.Patch_Cliko;

            this.Page.Form.Controls.Add(paramname[1]);
            this.Page.Form.Controls.Add(paramval[0]);

            paramname[2].Text = "Имя файла конфигурации"; paramval[1].Text = GetConfig.Parametrs.CFG_file;
            this.Page.Form.Controls.Add(paramname[2]);
            this.Page.Form.Controls.Add(paramval[1]);

            paramname[3].Text = "путь к папке со справочниками"; paramval[2].Text = GetConfig.Parametrs.SPR_DIR;
            this.Page.Form.Controls.Add(paramname[3]);
            this.Page.Form.Controls.Add(paramval[2]);

            paramname[4].Text = "имя таблицы со списком справочников"; paramval[3].Text = GetConfig.Parametrs.Patch_Cliko;
            this.Page.Form.Controls.Add(paramname[4]);
            this.Page.Form.Controls.Add(paramval[3]);


            Button Load = new Button()
            {
                Text = "загрузить"
            };

            Load.Click += Button_Load;

            this.Page.Form.Controls.Add(Load);
            Button Save = new Button()
            {
                Text = "сохранить"
            };

            this.Page.Form.Controls.Add(Save);
            Save.Click += Button_Save;
        }
        

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {

        }

        

        protected void Button_Load(object sender, EventArgs e) 
        {
            GetConfig.LoadParam();
            ShowConfig();

        }

        protected void Button_Save(object sender, EventArgs e) 
        {
            GetConfig.SaveParam();
            GetConfig.LoadParam();
            ShowConfig();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GetConfig.LoadParam();
            ShowConfig();
           // ClikoService S = new ClikoService();
           // this.Page.Form.InnerHtml =  S.GetData(""); 
        }
    }
}