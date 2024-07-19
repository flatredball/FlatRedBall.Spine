using FlatRedBall.Entities;
using FlatRedBall.Glue.Errors;
using FlatRedBall.Glue.Plugins.ExportedImplementations;
using Spine.Plugin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spine.Plugin.Managers
{
    class SpineErrorReporter : ErrorReporterBase
    {
        public override ErrorViewModel[] GetAllErrors()
        {
            var project = GlueState.Self.CurrentGlueProject;
            if (project == null) return new ErrorViewModel[0];

            List<ErrorViewModel> errors = new List<ErrorViewModel>();
            foreach(var screen in project.Screens)
            {
                foreach(var file in screen.ReferencedFiles)
                {
                    if(MissingAtlasErrorViewModel.GetIfHasError(screen, file))
                    {
                        errors.Add(new MissingAtlasErrorViewModel(screen, file));
                    }
                }
            }
            foreach(var entity in project.Entities)
            {
                foreach(var file in entity.ReferencedFiles)
                {
                    if(MissingAtlasErrorViewModel.GetIfHasError(entity, file))
                    {
                        errors.Add(new MissingAtlasErrorViewModel(entity, file));
                    }
                }
            }
            foreach(var file in project.GlobalFiles)
            {
                if (MissingAtlasErrorViewModel.GetIfHasError(null, file))
                {
                    errors.Add(new MissingAtlasErrorViewModel(null, file));
                }
            }

            return errors.ToArray();
        }
    }
}
