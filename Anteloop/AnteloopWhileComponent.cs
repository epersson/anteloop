using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Rhino.Geometry;

namespace Anteloop
{
    public class AnteloopWhileComponent : GH_Component, IGH_VariableParameterComponent, IAnteloop_Component

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


        public int InputParamCount => 2;

        public int OutputParamCount => 0;

        private AnteloopDoComponent DoComponent { get; set; } = null;

        private Anteloop_IO IO { get; set; } = null;

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

            AnteloopDoComponent loopStart = new AnteloopDoComponent();
            bool condition = new bool();
            GH_Structure<IGH_Goo> data = new GH_Structure<IGH_Goo>();

            if (!DA.GetData(0, ref loopStart)) { return; }
            if (!DA.GetData(1, ref condition)) { return; }
            
            if (!DA.GetDataTree(2, out data)) { return; }

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

        private void LoopParameterChanged(IGH_DocumentObject sender, GH_ObjectChangedEventArgs e)
        {
            if (!(Params.Input[0] is AnteloopDoComponent))
            {
                DoComponent.WhileComponent = null;
                DoComponent.IO = null;
                DoComponent = null;
            }
            else 
            {
                // Unregister
                DoComponent.WhileComponent = null;
                DoComponent.IO = null;
                DoComponent = null;

                DoComponent = (AnteloopDoComponent) Params.Input[0];
                DoComponent.IO = IO;
                DoComponent.WhileComponent = this;
            }
        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return index >= (side == GH_ParameterSide.Input ? InputParamCount : OutputParamCount);
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return index >= (side == GH_ParameterSide.Input ? InputParamCount : OutputParamCount);
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            IO.AddParams(this, side, index);
            return null;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            IO.RemoveParams(this, side, index);
            return false;
        }

        public void VariableParameterMaintenance()
        {
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