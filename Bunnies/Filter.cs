using System.Linq;
using Dalamud.Game.Gui.Toast;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;


namespace FirstPlugin
{
    public class Filter : IDisposable
    {
        private string? _lastToast;
        internal Filter()
        {
            P.ToastGui.Toast += this.OnToast;
        }

        public void Dispose()
        {
            P.ToastGui.Toast -= this.OnToast;
        }

        private bool AnyMatches(string text)
        {
            return C.Patterns.Any(regex => regex.IsMatch(text));
        }

        private void OnToast(ref SeString message, ref ToastOptions options, ref bool isHandled)
        {
            this.DoFilter(message, ref isHandled);
        }
        private void DoFilter(SeString message, ref bool isHandled)
        {
            _lastToast = message.TextValue;

            if (isHandled)
            {
                return;
            }

            if (this.AnyMatches(message.TextValue))
            {
                isHandled = true;
            }
        }

        public string? GetLastToast()
        {
            return _lastToast;
        }
    }
}
