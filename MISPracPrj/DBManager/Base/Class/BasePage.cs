using System.Text;
using System.Web.UI;

namespace DBManager
{
    public class BasePage : Page
    {
        private StringBuilder _script;

        protected string AppendJS
        {
            get
            {
                if (_script == null)
                {
                    _script = new StringBuilder();
                }

                EnsureChildControls();

                string s = _script.ToString();
                _script.Remove(0, _script.Length);

                return s;
            }
            set
            {
                if (_script == null)
                {
                    _script = new StringBuilder();
                }
                EnsureChildControls();

                _script.AppendLine(value);
            }
        }



    }
}