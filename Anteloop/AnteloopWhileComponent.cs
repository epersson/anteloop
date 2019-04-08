﻿using System;
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

        private bool eventHandlerSet = false;

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

            SetupEventHandler();

            AnteloopDoComponent loopStart = new AnteloopDoComponent();
            bool condition = new bool();
            GH_Structure<IGH_Goo> data = new GH_Structure<IGH_Goo>();

            if (!DA.GetData(0, ref loopStart)) { return; }
            if (!DA.GetData(1, ref condition)) { return; }


            int dataParamCount = Math.Min(Params.Input.Count - InputParamCount, DoComponent.Params.Output.Count - DoComponent.OutputParamCount);

            if (condition)
            {
                for (int i = 0; i < dataParamCount; i++)
                {
                    int while_i = i + InputParamCount;
                    int do_i = i + DoComponent.OutputParamCount;

                    if (!DA.GetDataTree(while_i, out data)) { continue; }
                    loopStart.Params.Input[do_i].ClearData();
                    loopStart.Params.Input[do_i].AddVolatileDataTree(data);
                }

                // Dangerous
                loopStart.ExpireSolution(true);
            }
            else
            {
                for (int i = 0; i < dataParamCount; i++)
                {
                    int whileInput_i = i + InputParamCount;
                    int whileOutput_i = i + OutputParamCount;

                    if (!DA.GetDataTree(whileInput_i, out data)) { continue; }
                    DA.SetDataTree(whileOutput_i, data);
                }
            }


        }

        private void SetupEventHandler()
        {
            if (eventHandlerSet)
                return;

            Params.Input[0].ObjectChanged += LoopParameterChanged;

            eventHandlerSet = true;
        }

        private void LoopParameterChanged(IGH_DocumentObject sender, GH_ObjectChangedEventArgs e)
        {
            // Unregister
            if (DoComponent != null)
            {
                DoComponent.WhileComponent = null;
                DoComponent.IO = null;
                DoComponent = null;
            }
            if (!Params.Input[0].VolatileData.IsEmpty)
            {
                //GH_Path path = Params.Input[0].VolatileData.Paths[0];
                //var vol = Params.Input[0].VolatileData as GH_Structure<IGH_Goo>;
                //var comp = vol.Branches[0][0];
                //var anteCompWrap = comp as GH_ObjectWrapper;
                //DoComponent = anteCompWrap.Value as AnteloopDoComponent;
                GH_Path path = Params.Input[0].VolatileData.Paths[0];
                DoComponent = ((GH_ObjectWrapper)Params.Input[0].VolatileData.get_Branch(path)[0]).Value as AnteloopDoComponent;
                if (DoComponent != null)
                {
                    DoComponent.WhileComponent = this;
                    IO = new Anteloop_IO(DoComponent, this);
                    DoComponent.IO = IO;
                }
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