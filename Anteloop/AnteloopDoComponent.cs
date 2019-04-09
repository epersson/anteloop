using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Anteloop
{
    public class AnteloopDoComponent : GH_Component, IGH_VariableParameterComponent, IAnteloop_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public AnteloopDoComponent()
          : base("Anteloop Do", "Do",
              "Description",
              "Anteloop", "Anteloop")
        {
        }

        public int InputParamCount => 0;

        public int OutputParamCount => 1;

        public AnteloopWhileComponent WhileComponent { get; set; } = null;

        public Anteloop_IO IO { get; set; } = null;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Data 1", "D1", "Data", GH_ParamAccess.tree);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Loop", "Loop", "Loop", GH_ParamAccess.item);
            pManager.AddGenericParameter("Data 1", "D1", "Data", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, this);

            GH_Structure<IGH_Goo> data = new GH_Structure<IGH_Goo>();

            DA.SetDataTree(1, data);
            int dataParamCount = Params.Input.Count - InputParamCount;
            for (int i = 0; i < dataParamCount; i++)
            {
                int doInput_i = i + InputParamCount;
                int doOutput_i = i + OutputParamCount;

                if (!DA.GetDataTree(doInput_i, out data)) { continue; }
                DA.SetDataTree(doOutput_i, data);
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
            return true;
        }

        public void VariableParameterMaintenance()
        {
            return;
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ab6f0d6a-b17d-4221-bcfd-057e9e771292"); }
        }
    }
}
