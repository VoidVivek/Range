using System;
using System.Diagnostics;
using System.Windows.Forms;
using Aveva.ApplicationFramework.Presentation;

namespace Aveva.CounterRange
{
    /// <summary>
    ///     Class RunCommand.
    /// </summary>
    /// <seealso cref="Aveva.ApplicationFramework.Presentation.Command" />
    internal class RunCommand : Command
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RunCommand" /> class.
        /// </summary>
        public RunCommand()
        {
            // Set the command key
            Key = "Aveva.CounterRange.RunCommand";
        }

        /// <summary>
        ///     Executes this instance.
        /// </summary>
        public override void Execute()
        {
            try
            {
                var frm = new ExpressionTests();
                frm.ShowDialog(Application.OpenForms[0]);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}