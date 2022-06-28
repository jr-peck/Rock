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

namespace Rock.UniversalSearch
{
    /// <summary>
    /// The methods that must be implemented for an entity to be able to
    /// participate in the content library system.
    /// </summary>
    internal interface IRockContentLibraryIndexable
    {
        /// <summary>
        /// Creates or updates an indexed document for the specified entity.
        /// </summary>
        /// <param name="id">The identifier of the entity to be indexed.</param>
        void IndexContentLibraryDocument( int id );

        /// <summary>
        /// Deletes the specified entity from the index.
        /// </summary>
        /// <param name="id">The identifier of the entity to be deleted from the index.</param>
        void DeleteContentLibraryDocument( int id );

        /// <summary>
        /// Creates or updates all documents that belong to the specified source.
        /// </summary>
        /// <param name="sourceId">The identifier of the source that should be indexed or <c>null</c> if all sources should be indexed.</param>
        void IndexAllContentLibraryDocuments( int? sourceId = null );

        /// <summary>
        /// Deletes all documents that belong to the specified source.
        /// </summary>
        /// <param name="sourceId">The identifier of the source whose documents should be deleted or <c>null</c> if all documents should be deleted.</param>
        void DeleteAllContentLibraryDocuments( int? sourceId = null );
    }
}
