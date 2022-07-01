using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CsvHelper;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.CVSImport
{
    [DisplayName( "CSV Import" )]
    [Category( "CSV Import" )]
    [Description( "Block to import data into Rock using the CSV files." )]
    [Rock.SystemGuid.BlockTypeGuid( "EDA8F90D-1201-4AFF-9E6D-A8F6D6F618D9" )]
    public partial class CSVImport : Rock.Web.UI.RockBlock
    {
        private const string ROCK_ATTRIBUTES_OPTION_NAME = "Attributes";
        private const string FIELD_OPTION_NAME = "Field";

        /// <summary>
        /// The list items the fields in the CSV can be mapped to.
        /// </summary>
        private ListItem[] propertiesDropDownList;

        /// <summary>
        /// The properties that should be mapped to by fields in the csv. Not having one of these fields mapped to a csv column will result in an error
        /// </summary>
        private string[] requiredFields = { "Id", "Family Id", "Family Role", "First Name", "Last Name" };

        /// <summary>
        /// It is optional to map these properties to a column in the csv.
        /// </summary>
        private string[] optionalFields = { "Nick Name",
            "Middle Name",
            "Suffix",
            "TitleValueId",
            "Home Phone",
            "Mobile Phone",
            "Is SMS Enabled",
            "Email",
            "Email Preference",
            "Gender",
            "Marital Status",
            "Birthdate",
            "Anniversay Date",
            "Record Status",
            "Inactive Reason",
            "Is Deceased",
            "Connection Status",
            "Grade",
            "Home Address Street 1",
            "Home Address Stree 2",
            "Home Address City",
            "Home Address State",
            "Home Address Postal Code",
            "Home Address Country",
            "Created Date Time",
            "Modified Date Time",
            "Note",
            "Campus Id",
            "Campus Name",
            "Give Individually" };

        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );
            Array.Sort( requiredFields );
            Array.Sort( optionalFields );
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

        protected void rptCSVHeaders_ItemDataBound( object sender, RepeaterItemEventArgs e )
        {
            var ddlCSVHeader = e.Item.FindControl( "ddlCSVHeader" ) as RockDropDownList;
            ddlCSVHeader.Items.AddRange( propertiesDropDownList );
        }

        protected void btnStart_Click( object sender, EventArgs e )
        {
            string csvFileName = this.Request.MapPath( hfCSVFileName.Value );
            // TODO add logging

            this.propertiesDropDownList = CreateListItemsDropDown();

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
                if ( recordsCount > 0 )
                {
                    recordsCount--;
                }
                tdRecordCount.Description = recordsCount.ToString();
            }

            pnlFieldMappingPage.Visible = true;
            pnlLandingPage.Visible = false;
        }

        protected void btnImport_Click( object sender, EventArgs e )
        {
            
        }

        private ListItem[] CreateListItemsDropDown()
        {
            RockContext rockContext = new RockContext();
            int entityTypeIdPerson = EntityTypeCache.GetId<Person>().Value;
            AttributeService attributeService = new AttributeService( rockContext );

            ListItem[] rockAttributeArray = attributeService.GetByEntityTypeId( entityTypeIdPerson )
                .Select( a => a.Name )
                .AsEnumerable()
                .Select( name => new ListItem( name ) )
                .ToArray();
            foreach ( ListItem rockAttribute in rockAttributeArray )
            {
                rockAttribute.Attributes["OptionGroup"] = ROCK_ATTRIBUTES_OPTION_NAME;
            }

            ListItem[] requiredFieldslistItems = requiredFields.Select( name => new ListItem( name ) )
                .ToArray();

            foreach ( ListItem listItem in requiredFieldslistItems )
            {
                listItem.Attributes["OptionGroup"] = FIELD_OPTION_NAME;
            }

            ListItem[] optionalFieldslistItems = optionalFields.Select( name => new ListItem( name ) )
                .ToArray();

            foreach ( ListItem listItem in optionalFieldslistItems )
            {
                listItem.Attributes["OptionGroup"] = FIELD_OPTION_NAME;
            }

            ListItem[] empty = { new ListItem("") };

            return empty
                .Concat(requiredFieldslistItems)
                .Concat( optionalFieldslistItems )
                .Concat( rockAttributeArray )
                .ToArray();
        }
    }
}
