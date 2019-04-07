﻿using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Antiloop
{
    public class AnteloopInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Anteloop";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("8d622798-31d0-4bb6-b41e-ff834184b3e3");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}