using System;
using System.ComponentModel;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CsvHelper;
using Rock.Web.UI.Controls;

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

            if ( !Page.IsPostBack )
            {
                ListItem peopleDataTypeItem = new ListItem( "People" );
                ddlDataType.Items.Add( peopleDataTypeItem );
            }
            if( hfcsvHeaders.Value  != null)
            {
                Array.ForEach( hfcsvHeaders.Value.Split(','), header => {
                    HtmlGenericControl headerControl = new HtmlGenericControl( "h2" );
                    headerControl.InnerText = header;
                    pnlheaders.Controls.Add( headerControl );
                } );
            }
        }

        protected void fupCSVFile_FileUploaded( object sender, EventArgs e )
        {
            hfCSVFileName.Value = fupCSVFile.UploadedContentFilePath;
        }
        protected void fupCSVFile_FileRemoved( object sender, EventArgs e )
        {
            hfCSVFileName.Value = ""; // nullify the file name to be processed.
        }

        protected void btnStart_Click( object sender, EventArgs e )
        {
            string csvFileName = this.Request.MapPath( hfCSVFileName.Value );
            // TODO add logging

            using ( StreamReader csvFileStream = File.OpenText( csvFileName ) )
            {
                CsvReader csvReader = new CsvReader( csvFileStream );
                csvReader.Configuration.HasHeaderRecord = true;
                csvReader.Read();
                string[] fieldHeaders = csvReader.FieldHeaders;
                hfcsvHeaders.Value = String.Join( ",", fieldHeaders );
                Array.ForEach( fieldHeaders, header => {
                    RockDropDownList rockDropDownList = new RockDropDownList();
                    rockDropDownList.Label = header;
                    rockDropDownList.ID = $"ddlCSVHeader{header.Replace(" ", "")}";

                    HtmlGenericControl headerControl = new HtmlGenericControl( "h2" );
                    headerControl.InnerText = header;
                    pnlheaders.Controls.Add( rockDropDownList );
                } );
            }

        }

    }
}
