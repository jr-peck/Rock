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
using System.Collections.Generic;

namespace Rock.ViewModels.Blocks.CMS.ContentLibraryDetail
{
    /// <summary>
    /// Identifies a content channel source to be saved.
    /// </summary>
    public class ContentChannelSourceBag
    {
        /// <summary>
        /// Gets or sets the unique identifier of the content channel.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the list of attribute unique identifiers that are
        /// selected for this content channel.
        /// </summary>
        public List<Guid> AttributeGuids { get; set; }
    }
}
