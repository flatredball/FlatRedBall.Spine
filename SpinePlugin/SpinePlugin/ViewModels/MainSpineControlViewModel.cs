using FlatRedBall.Glue.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinePlugin.ViewModels
{
    internal class MainSpineControlViewModel : PropertyListContainerViewModel
    {
        [SyncedProperty]
        public string AtlasName 
        {
            get => Get<string>();
            set => SetAndPersist(value);
        }

        
    }
}
