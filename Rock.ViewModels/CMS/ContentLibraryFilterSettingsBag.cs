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

using System.Collections.Generic;

using Rock.Enums.CMS;

namespace Rock.ViewModels.CMS
{
    /// <summary>
    /// Defines the settings used by the Content Library filters.
    /// </summary>
    public class ContentLibraryFilterSettingsBag
    {
        /// <summary>
        /// Gets or sets a value indicating if personalization segments should
        /// be used when indexing and filtering content.
        /// </summary>
        /// <value>
        ///   <c>true</c> if personalization segments should be used; otherwise, <c>false</c>.
        /// </value>
        public bool IsPersonalizationSegmentsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if personalization request filters
        /// should be used when filtering content.
        /// </summary>
        /// <value>
        ///   <c>true</c> if personalization request filters should be used; otherwise, <c>false</c>.
        /// </value>
        public bool IsPersonalizationRequestFiltersEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether full text search should be enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if full text search should be enabled; otherwise, <c>false</c>.
        /// </value>
        public bool FullTextSearchEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether searching content by year
        /// should be enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if searching content by year is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool YearSearchEnabled { get; set; }

        /// <summary>
        /// Gets or sets the label to use for the filter that allows an
        /// individual to search for content by a specific year.
        /// </summary>
        /// <value>
        /// The label to use for the Year search filter.
        /// </value>
        public string YearSearchLabel { get; set; }

        /// <summary>
        /// Gets or sets the year search filter control.
        /// </summary>
        /// <value>
        /// The year search filter control.
        /// </value>
        public ContentLibraryFilterControl YearSearchFilterControl { get; set; }

        /// <summary>
        /// Gets or sets the selection type to use for the Year filter.
        /// </summary>
        /// <value>
        /// The selection type to use for the Year filter.
        /// </value>
        public ContentLibraryFilterSelection YearSearchFilterSelection { get; set; }

        /// <summary>
        /// Gets or sets the attributes that are enabled for filtering
        /// and indexing on the content library.
        /// </summary>
        /// <value>
        /// The attributes that are enable for filtering.
        /// </value>
        public List<ContentLibraryAttributeFilterSettingsBag> AttributeFilters { get; set; }
    }
}
