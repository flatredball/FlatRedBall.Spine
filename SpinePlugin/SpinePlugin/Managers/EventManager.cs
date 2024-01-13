using FlatRedBall.Glue.Events;
using FlatRedBall.Glue.Reflection;
using FlatRedBall.Glue.SaveClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinePlugin.Managers
{
    internal static class EventManager
    {
        internal static void HandleAddEventsForObject(NamedObjectSave save, List<ExposableEvent> list)
        {
            var ati = save.GetAssetTypeInfo();

            if(ati == AssetTypeInfoManager.SpineDrawableBatchAssetTypeInfo)
            {
                // add the ability to expose Event
                var exposableEvent = new ExposableEvent("Event");
                list.Add(exposableEvent);
            }
        }

        internal static void HandleGetEventSignatureArgs(NamedObjectSave namedObjectSave, EventResponseSave eventResponseSave, 
            out string type, out string signatureArgs)
        {
            type = null;
            signatureArgs = null;
            if(namedObjectSave != null && !string.IsNullOrEmpty(eventResponseSave.SourceObject) && eventResponseSave.SourceObjectEvent == "Event")
            {
                var ati = namedObjectSave.GetAssetTypeInfo();

                if(ati == AssetTypeInfoManager.SpineDrawableBatchAssetTypeInfo )
                {
                    type = "global::Spine.AnimationState.TrackEntryEventDelegate";
                    signatureArgs = "global::Spine.TrackEntry trackEntry, global::Spine.Event e";
                }
            }
        }
    }
}
