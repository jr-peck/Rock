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

namespace Rock.UniversalSearch.IndexModels.Attributes
{
    /// <summary>
    /// Attribute to override the name of the index used for this type.
    /// </summary>
    public class IndexNameAttribute : System.Attribute
    {
        /// <summary>
        /// Gets the name to use for the search index instead of the type name.
        /// </summary>
        /// <value>
        /// The name to use for the search index instead of the type name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Creates a new instance of <see cref="IndexNameAttribute"/>.
        /// </summary>
        /// <param name="name">The name to use for the search index instead of the type name.</param>
        public IndexNameAttribute( string name )
        {
            Name = name;
        }
    }
}
