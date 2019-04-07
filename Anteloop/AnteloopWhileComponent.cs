using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Anteloop
{
    public class AnteloopWhileComponent : GH_Component
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
            pManager.AddBooleanParameter("Condition", "C", "While condition is true, the Anteloop will continue to run", GH_ParamAccess.item, false);
            pManager.AddGenericParameter("Data", "Data", "Data", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Loop", "Loop", "Loop", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Data", "Data", "Data", GH_ParamAccess.tree);
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