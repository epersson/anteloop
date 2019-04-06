using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Antiloop
{
    public class AntiloopWhileComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public AntiloopWhileComponent()
          : base("Antiloop While", "While",
              "Description",
              "Antiloop", "Antiloop")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Condition", "C", "While condition is true, the Antiloop will continue to run", GH_ParamAccess.item, false);
            pManager.AddNumberParameter("Number", "N", "Input Number", GH_ParamAccess.item);
            pManager.AddGenericParameter("Loop", "Loop", "Loop", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Number", "N", "outputNumber", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool condition = new bool();
            double n = new double();
            AntiloopDoComponent loopStart = new AntiloopDoComponent();

            if (!DA.GetData(0, ref condition)) { return; }
            if (!DA.GetData(1, ref n)) { return; }
            if (!DA.GetData(2, ref loopStart)) { return; }


            if (condition)
            {
                GH_Structure<GH_Number> oldStructure = ((GH_Structure<GH_Number>)loopStart.Params.Input[0].VolatileData);
                oldStructure.Clear();
                oldStructure.Append(new GH_Number(n));

                // Dangerous
                loopStart.ExpireSolution(true);
            }
            else
            {
                DA.SetData(0, n);
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