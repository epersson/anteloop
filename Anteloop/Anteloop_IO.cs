using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace Anteloop
{
    public interface IAnteloop_Component
    {
        int InputParamCount { get; }
        int OutputParamCount { get; }
    }

    public class Anteloop_IO
    {
        private AnteloopDoComponent DoComponent { get; set; } = null;

        private AnteloopWhileComponent WhileComponent { get; set; } = null;

        public Anteloop_IO(AnteloopDoComponent doComponent, AnteloopWhileComponent whileComponent)
        {
            DoComponent = doComponent;
            WhileComponent = whileComponent;
        }

        public void AddParams(IAnteloop_Component component, GH_ParameterSide side, int index)
        {
            int normalizedIndex = (side == GH_ParameterSide.Input) ? index - component.InputParamCount : index - component.OutputParamCount;

            DoComponent.Params.RegisterInputParam(CreateNamedParam(normalizedIndex), normalizedIndex + DoComponent.InputParamCount);
            DoComponent.Params.RegisterOutputParam(CreateNamedParam(normalizedIndex), normalizedIndex + DoComponent.OutputParamCount);
            WhileComponent.Params.RegisterInputParam(CreateNamedParam(normalizedIndex), normalizedIndex + WhileComponent.InputParamCount);
            WhileComponent.Params.RegisterOutputParam(CreateNamedParam(normalizedIndex), normalizedIndex + WhileComponent.OutputParamCount);
        }

        public void RemoveParams(IAnteloop_Component component, GH_ParameterSide side, int index)
        {
            int normalizedIndex = (side == GH_ParameterSide.Input) ? index - component.InputParamCount : index - component.OutputParamCount;

            DoComponent.Params.UnregisterInputParameter(DoComponent.Params.Input[normalizedIndex + DoComponent.InputParamCount]);
            DoComponent.Params.UnregisterOutputParameter(DoComponent.Params.Output[normalizedIndex + DoComponent.OutputParamCount]);
            WhileComponent.Params.UnregisterInputParameter(WhileComponent.Params.Input[normalizedIndex + WhileComponent.InputParamCount]);
            WhileComponent.Params.UnregisterOutputParameter(WhileComponent.Params.Output[normalizedIndex + WhileComponent.OutputParamCount]);
        }

        private Param_GenericObject CreateNamedParam(int normalizedIndex)
        {
            normalizedIndex += 1; // Start naming at 1
            return new Param_GenericObject()
            {
                Name = "Data " + normalizedIndex.ToString(),
                NickName = "D" + normalizedIndex.ToString(),
                Description = "Data",
                Optional = true,
                Access = GH_ParamAccess.tree
            };
        }
    }
}
