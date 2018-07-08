using System;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MyDoomLauncher.EventTriggers
{
    class EnterKeyDownEventTrigger : EventTrigger
    {
        public EnterKeyDownEventTrigger() : base("KeyDown")
        { }

        protected override void OnEvent(EventArgs eventArgs)
        {
            KeyEventArgs e = eventArgs as KeyEventArgs;
            if (e != null && e.Key == Key.Return)
                InvokeActions(eventArgs);
        }
    }
}