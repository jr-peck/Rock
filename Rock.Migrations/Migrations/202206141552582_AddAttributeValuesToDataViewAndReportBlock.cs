// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class AddAttributeValuesToDataViewAndReportBlock : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            RockMigrationHelper.AddOrUpdateBlockTypeAttribute( "ADE003C7-649B-466A-872B-B8AC952E7841",
                SystemGuid.FieldType.PAGE_REFERENCE,
                "Search Results Page",
                "SearchResultsPage",
                "Search Results Page",
                "The page to display search results on",
                11,
                "",
                SystemGuid.Attribute.CATEGORY_TREEVIEW_SEARCH_RESULTS );

            // Set the DataViewSearchResults page as the value for the SearcResultsPage attribute value for the CategoryTreeview block on that page
            RockMigrationHelper.AddBlockAttributeValue( true, "6A9111AC-34E7-4103-A12A-9A89C2A14B57", SystemGuid.Attribute.CATEGORY_TREEVIEW_SEARCH_RESULTS, SystemGuid.Page.DATAVIEW_SEARCH_RESULTS );
            // Set the ReportSearchResults page as the value for the SearcResultsPage attribute value for the CategoryTreeview block on that page
            RockMigrationHelper.AddBlockAttributeValue( true, "0F1F8343-A187-4653-9A4A-47D67CE86D71", SystemGuid.Attribute.CATEGORY_TREEVIEW_SEARCH_RESULTS, SystemGuid.Page.REPORT_SEARCH_RESULTS );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            RockMigrationHelper.DeleteBlockAttributeValue( "6A9111AC-34E7-4103-A12A-9A89C2A14B57", SystemGuid.Attribute.CATEGORY_TREEVIEW_SEARCH_RESULTS );
            RockMigrationHelper.DeleteBlockAttributeValue( "0F1F8343-A187-4653-9A4A-47D67CE86D71", SystemGuid.Attribute.CATEGORY_TREEVIEW_SEARCH_RESULTS );
        }
    }
}
