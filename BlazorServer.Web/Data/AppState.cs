using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServer.Web.Data
{
    public class AppState
    {
        public Place Place { get; private set; }
        public event Action OnChange;

        public void SetAppState(Place place)
        {
            Place = place;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}