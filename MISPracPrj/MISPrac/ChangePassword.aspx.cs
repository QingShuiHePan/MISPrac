using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBManager;
using SSPUCore;
using SSPUCore.Controls;

namespace MISPrac
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            this.ValidatedSubmit1.Click += new EventHandler(JButton1_Click);

            AddClientJSFunction();


        }

        private void AddClientJSFunction()
        {

            Utility.Instance.AppendJS = string.Format("if( $('#{0}').val() != $('#{1}').val() ) " +
                                     "{{" +
                                     "     alert('两次输入密码不一致');" +
                                     "     return false;" +
                                     "}}"
                                     , this.JTextBox_NewPassword1.ClientID
                                     , this.JTextBox_NewPassword2.ClientID
                                     );

            Utility.Instance.AppendJS = string.Format("if( $('#{0}').val().length < 6 ) " +
                                    "{{" +
                                    "     alert('请输入一个不小于6位的密码');" +
                                    "     return false;" +
                                    "}}"
                                    , this.JTextBox_NewPassword1.ClientID
                                    );

            ////数字1，大写为2，小写为4，特殊字符为8,1111
            //Utility.Instance.AppendJS = string.Format("if( $.PasswordStrength( $('#{0}').val() ) < 2  )" +
            //                         "{{" +
            //                         "      alert('请输入一个包含字母数字不小于6位的密码');" +
            //                         "      return false;" +
            //                         "}} "
            //                         , this.JTextBox_NewPassword1.ClientID);



            Utility.Instance.AppendJS = string.Format("$('#{0}').val( $.md5($('#{0}').val()) );", this.JTextBox_OldPassword.ClientID);
            Utility.Instance.AppendJS = string.Format("$('#{0}').val( $.md5($('#{0}').val()) );", this.JTextBox_NewPassword1.ClientID);
            Utility.Instance.AppendJS = string.Format("$('#{0}').val( $.md5($('#{0}').val()) );", this.JTextBox_NewPassword2.ClientID);

            Utility.Instance.AppendJS = string.Format("$('form').submit({0});", ControlUtility.BuildJSFunction(Utility.Instance.AppendJS, new string[] { "e" }));

            ScriptManagerItem item = new ScriptManagerItem();
            item.ClientWholeFunction = Utility.Instance.AppendJS;
            this.ScriptManager1.JSFunction = item;
        }
        void JButton1_Click(object sender, EventArgs e)
        {

            if (JTextBox_NewPassword1.Text.Length < 30)
            {
                this.Label_Info.Visible = true;
                this.Label_Info.Text = "输入非法";
                ClearPassword();
                return;
            }

            TO_UserDefChangePassword oldUser =
                CachingManager.Instance.GetTO_ObjByCondition<TO_UserDefChangePassword>(Utility.Instance.CurrentUser.ID);
            
            string oldPassword = this.JTextBox_OldPassword.Text;

            if (oldUser.Password.Equals(oldPassword))
            {
                oldUser.Password = JTextBox_NewPassword1.Text;
                oldUser.ModifyToDB();

                Label_Info.Visible = true;

            }
            else
            {
                this.Label_Info.Visible = true;
                this.Label_Info.Text = "原密码错误";

            }
            ClearPassword();

        }

        private void ClearPassword()
        {
            this.JTextBox_NewPassword1.Text =
                          this.JTextBox_NewPassword2.Text = this.JTextBox_OldPassword.Text = string.Empty;
        }
    }
}