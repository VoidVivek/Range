using System;
using System.Diagnostics;
using System.Windows.Forms;
using Aveva.ApplicationFramework.Presentation;

namespace Aveva.CounterRange
{
    /// <summary>
    ///     Class ShowCommand.
    /// </summary>
    /// <seealso cref="Aveva.ApplicationFramework.Presentation.Command" />
    internal class ShowCommand : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ShowCommand" /> class.
        /// </summary>
        public ShowCommand()
        {
            // Set the command key
            Key = "Aveva.CounterRange.ShowCommand";
        }

        /// <summary>
        ///     Executes this instance.
        /// </summary>
        public override void Execute()
        {
            try
            {
                var frm = new Form1();
                frm.ShowDialog(Application.OpenForms[0]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}