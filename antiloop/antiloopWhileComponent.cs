using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace antiloop
{
    public class antiloopWhileComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public antiloopWhileComponent()
          : base("Antiloop", "AntiloopWhile",
              "Description",
              "Antiloop", "Antiloop")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Number", "n", "Input Number", GH_ParamAccess.item);
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
            double n = new double();
            antiloopStartComponent loopStart = new antiloopStartComponent();

            if (!DA.GetData(0, ref n)) { return; }
            if (!DA.GetData(1, ref loopStart)) { return; }

            if (n < 10)
            {
                GH_Number num = new GH_Number(n + 1);
                GH_Structure<GH_Number> oldStructure = ((GH_Structure<GH_Number>)loopStart.Params.Input[0].VolatileData);
                oldStructure.Clear();
                oldStructure.Append(num);
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