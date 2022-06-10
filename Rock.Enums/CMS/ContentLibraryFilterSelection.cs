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

namespace Rock.Enums.CMS
{
    /// <summary>
    /// Defines the selection type that can be used for filter controls when
    /// rendered on the content library pages.
    /// </summary>
    public enum ContentLibraryFilterSelection
    {
        /// <summary>
        /// A single item of this filter can be selected.
        /// </summary>
        Single = 0,

        /// <summary>
        /// Multiple items of this filter can be selected.
        /// </summary>
        Multiple = 1
    }
}
