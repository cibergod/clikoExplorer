using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Web1
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getData a = new getData();
            a.ReadDigest();
            TreeView S = new TreeView();
            
            foreach(string Name in a.NameDigest){
                TreeNode root = new TreeNode(Name);
                S.Nodes.Add(root);
            }

            S.ExpandAll();
            this.Page.Form.Controls.Add(S);
        }
    }
}