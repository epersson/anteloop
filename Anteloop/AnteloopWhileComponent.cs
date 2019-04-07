using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace Anteloop
{
    public class AnteloopWhileComponent : GH_Component, IGH_VariableParameterComponent

    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public AnteloopWhileComponent()
          : base("Anteloop While", "While",
              "Description",
              "Anteloop", "Anteloop")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Anteloop Do", "Loop", "Loop", GH_ParamAccess.item);
            pManager.AddBooleanParameter("While Condition", "While", "While the condition is true, the Antiloop will continue to run", GH_ParamAccess.item, false);
            pManager.AddGenericParameter("Data 1", "D1", "Data", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data 1", "D1", "Data", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool condition = new bool();
            GH_Structure<IGH_Goo> data = new GH_Structure<IGH_Goo>();
            AnteloopDoComponent loopStart = new AnteloopDoComponent();

            if (!DA.GetData(0, ref condition)) { return; }
            if (!DA.GetDataTree(1, out data)) { return; }
            if (!DA.GetData(2, ref loopStart)) { return; }


            if (condition)
            {
               loopStart.Params.Input[0].ClearData();
               loopStart.Params.Input[0].AddVolatileDataTree(data);

               // Dangerous
               loopStart.ExpireSolution(true);
            }
            else
            {
               DA.SetDataTree(0, data);
            }


        }

        private int fixedParams(GH_ParameterSide side)
        {
            return side == GH_ParameterSide.Input ? 2 : 0;
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return index >= fixedParams(side);
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return index >= fixedParams(side);
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            Param_GenericObject newParam = new Param_GenericObject();
            Param_GenericObject otherParam = new Param_GenericObject();

            if (side == GH_ParameterSide.Input)
                this.Params.RegisterOutputParam(otherParam, index - 2);
            else if (side == GH_ParameterSide.Output)
                this.Params.RegisterInputParam(otherParam, index + 2);

            this.Params.OnParametersChanged();

            return newParam;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            
            return true;
        }

        public void VariableParameterMaintenance()
        {
            for (int i = 0; i < this.Params.Output.Count; i++)
            {
                this.Params.Input[i + 2].Name = "Data " + i.ToString();
                this.Params.Input[i + 2].NickName = "D" + i.ToString();
                this.Params.Output[i].Name = "Data " + i.ToString();
                this.Params.Output[i].NickName = "D" + i.ToString();
            }

            return;
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6e09ff36-caed-4fee-9fdc-3390c10366f8"); }
        }
    }
}