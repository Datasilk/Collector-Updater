using System.Text;
using System.Management.Automation;

namespace Updater
{
    public static class Ps
    {
        private static PowerShell ps = PowerShell.Create();

        public static string Command(string script)
        {
            string err = "";

            ps.AddScript(script);
            ps.AddCommand("Out-String");

            var outputCollection = new PSDataCollection<PSObject>();
            ps.Streams.Error.DataAdded += (object sender, DataAddedEventArgs e) =>
            {
                err = ((PSDataCollection<ErrorRecord>)sender)[e.Index].ToString();
            };

            var result = ps.BeginInvoke<PSObject, PSObject>(null, outputCollection);
            ps.EndInvoke(result);

            var sb = new StringBuilder();
            foreach(var item in outputCollection)
            {
                sb.AppendLine(item.BaseObject.ToString());
            }
            ps.Commands.Clear();

            if (!string.IsNullOrEmpty(err))
            {
                return err;
            }
            return sb.ToString();
        }
    }
}
