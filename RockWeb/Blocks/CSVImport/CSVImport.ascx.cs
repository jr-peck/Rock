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

            // get the headers
            using ( StreamReader csvFileStream = File.OpenText( csvFileName ) )
            {
                CsvReader csvReader = new CsvReader( csvFileStream );
                csvReader.Configuration.HasHeaderRecord = true;
                csvReader.Read();
                string[] fieldHeaders = csvReader.FieldHeaders;
                rptCSVHeaders.DataSource = fieldHeaders;
                rptCSVHeaders.DataBind();
            }

            // get the number of records in the csv file
            using ( StreamReader csvFileStream = File.OpenText( csvFileName ) )
            {
                int recordsCount = 0;
                while ( csvFileStream.ReadLine() != null )
                {
                    ++recordsCount;
                }
                tdRecordCount.Description = recordsCount.ToString();
            }


            pnlFieldMappingPage.Visible = true;
            pnlLandingPage.Visible = false;
        }
    }
}
