using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace RockWeb.Blocks.CVSImport
{
    [DisplayName( "CSV Import" )]
    [Category( "CSV Import" )]
    [Description( "Block to import data into Rock using the CSV files." )]
    [Rock.SystemGuid.BlockTypeGuid( "EDA8F90D-1201-4AFF-9E6D-A8F6D6F618D9" )]
    public partial class CSVImport : Rock.Web.UI.RockBlock
    {
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
        }

        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );
        }
    }
}
