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
    public partial class AddDataViewSearchResultsAndReportSearchResultPages : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            AddReportsSearchResultsPage();
            AddDataViewSearchResultsPage();
        }

        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DeleteReportsSearchResultsPage();
            DeleteDataViewSearchResultsPage();
        }

        private void AddDataViewSearchResultsPage()
        {
            // Add Page - Internal Name: DataView Search Results - Site: Rock RMS
            RockMigrationHelper.AddPage( true, SystemGuid.Page.DATA_VIEWS, SystemGuid.Layout.FULL_WIDTH_INTERNAL_SITE, "DataView Search Results", "", SystemGuid.Page.DATAVIEW_SEARCH_RESULTS );
            // Add/Update BlockType - Name: DataView Search - Category: Reporting - Path: ~/Blocks/Reporting/DataViewSearch.ascx - EntityType: -
            RockMigrationHelper.UpdateBlockType( "DataView Search", "Handles displaying dataview search results and redirects to the dataview result page (via route ~/reporting/dataViews?) when only one match was found.", "~/Blocks/Reporting/DataViewSearch.ascx", "Reporting", SystemGuid.BlockType.DATAVIEW_SEARCH_RESULTS );
            // Add Block - Block Name: DataView Search - Page Name: DataView Search Results Layout: - Site: Rock RMS
            RockMigrationHelper.AddBlock( true, SystemGuid.Page.DATAVIEW_SEARCH_RESULTS.AsGuid(), null, SystemGuid.Site.SITE_ROCK_INTERNAL.AsGuid(), SystemGuid.BlockType.DATAVIEW_SEARCH_RESULTS.AsGuid(), "DataView Search", "Main", @"", @"", 0, SystemGuid.Block.DATAVIEW_SEARCH_RESULTS );
        }

        private void AddReportsSearchResultsPage()
        {
            // Add Page - Internal Name: Reports Search Results - Site: Rock RMS
            RockMigrationHelper.AddPage( true, SystemGuid.Page.REPORTS_REPORTING, SystemGuid.Layout.FULL_WIDTH_INTERNAL_SITE, "Reports Search Results", "", SystemGuid.Page.REPORT_SEARCH_RESULTS );
            // Add/Update BlockType - Name: Reports Search - Category: Reporting - Path: ~/Blocks/Reporting/ReportSearch.ascx - EntityType: -
            RockMigrationHelper.UpdateBlockType( "Report Search", "Handles displaying report search results and redirects to the report result page (via route ~/reporting/reports?) when only one match was found.", "~/Blocks/Reporting/ReportSearch.ascx", "Reporting", SystemGuid.BlockType.REPORT_SEARCH_RESULTS );
            // Add Block - Block Name: Reports Search - Page Name: Report Search Results Layout: - Site: Rock RMS
            RockMigrationHelper.AddBlock( true, SystemGuid.Page.REPORT_SEARCH_RESULTS.AsGuid(), null, SystemGuid.Site.SITE_ROCK_INTERNAL.AsGuid(), SystemGuid.BlockType.REPORT_SEARCH_RESULTS.AsGuid(), "Report Search", "Main", @"", @"", 0, SystemGuid.Block.REPORT_SEARCH_RESULTS );
        }

        private void DeleteDataViewSearchResultsPage()
        {
            // Remove Block - Name: DataView Search, from Page: DataView Search Results, Site: Rock RMS - from Page: DataView Search Results, Site: Rock RMS
            RockMigrationHelper.DeleteBlock( SystemGuid.Block.DATAVIEW_SEARCH_RESULTS );
            // Delete BlockType - Name: DataView Search - Category: Reporting - Path: ~/Blocks/Reporting/DataViewSearch.ascx - EntityType: -
            RockMigrationHelper.DeleteBlockType( SystemGuid.BlockType.DATAVIEW_SEARCH_RESULTS );
            // Delete Page Internal Name: DataView Search Results Site: Rock RMS Layout: Full Width
            RockMigrationHelper.DeletePage( SystemGuid.Page.DATAVIEW_SEARCH_RESULTS );
        }

        private void DeleteReportsSearchResultsPage()
        {
            // Remove Block - Name: Report Search, from Page: Report Search Results, Site: Rock RMS - from Page: Report Search Results, Site: Rock RMS
            RockMigrationHelper.DeleteBlock( SystemGuid.Block.REPORT_SEARCH_RESULTS );
            // Delete BlockType - Name: Report Search - Category: Reporting - Path: ~/Blocks/Reporting/ReportSearch.ascx - EntityType: -
            RockMigrationHelper.DeleteBlockType( SystemGuid.BlockType.REPORT_SEARCH_RESULTS );
            // Delete Page Internal Name: Report Search Results Site: Rock RMS Layout: Full Width
            RockMigrationHelper.DeletePage( SystemGuid.Page.REPORT_SEARCH_RESULTS );
        }
    }
}
