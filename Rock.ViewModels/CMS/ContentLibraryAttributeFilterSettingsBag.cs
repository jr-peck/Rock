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

using System;

using Rock.Enums.CMS;

namespace Rock.ViewModels.CMS
{
    /// <summary>
    /// The settings for a single attribute filter configured on a content library.
    /// </summary>
    public class ContentLibraryAttributeFilterSettingsBag
    {
        /// <summary>
        /// Gets or sets the attribute key to use when accessing values
        /// for this filter.
        /// </summary>
        public Guid AttributeKey { get; set; }

        /// <summary>
        /// Gets or sets the label to use for the filter.
        /// </summary>
        /// <value>
        /// The label to use for the Year search filter.
        /// </value>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the search filter control.
        /// </summary>
        /// <value>
        /// The search filter control.
        /// </value>
        public ContentLibraryFilterControl FilterControl { get; set; }

        /// <summary>
        /// Gets or sets the selection type to use for the filter.
        /// </summary>
        /// <value>
        /// The selection type to use for the filter.
        /// </value>
        public ContentLibraryFilterSelection FilterSelection { get; set; }
    }
}
