using Aveva.ApplicationFramework;
using Aveva.ApplicationFramework.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aveva.CounterRange
{
    public class AddIn : IAddin
    {
        public string Name => "Aveva.CounterRange";

        public string Description => "Addin ofr range reservation";

        public void Start(ServiceManager serviceManager)
        {
            // Get the CommandManager
            var commandManager = (CommandManager)serviceManager.GetService(typeof(CommandManager));

            commandManager.Commands.Add(new ShowCommand());
            commandManager.Commands.Add(new RunCommand());
        }

        public void Stop()
        {
        }
    }
}
