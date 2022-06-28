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

using Rock.Data;
using Rock.UniversalSearch;
using Rock.Web.Cache;

namespace Rock.Tasks
{
    /// <summary>
    /// Calls <see cref="IRockContentLibraryIndexable.DeleteContentLibraryDocument(int)"/> for the specified <see cref="IEntity"/>
    /// </summary>
    public sealed class DeleteContentLibraryDocument : BusStartedTask<DeleteContentLibraryDocument.Message>
    {
        /// <summary>
        /// Executes this instance.
        /// </summary>
        /// <param name="message"></param>
        public override void Execute( Message message )
        {
            var entityType = EntityTypeCache.Get( message.EntityTypeId );
            var type = entityType.GetEntityType();

            if ( type == null )
            {
                return;
            }

            if ( Activator.CreateInstance( type, null ) is IRockContentLibraryIndexable indexable )
            {
                indexable.DeleteContentLibraryDocument( message.EntityId );
            }
        }

        /// <summary>
        /// Message that identifies the entity to be processed.
        /// </summary>
        public sealed class Message : BusStartedTaskMessage
        {
            /// <summary>
            /// Gets or sets the entity type identifier.
            /// </summary>
            /// <value>
            /// The entity type identifier.
            /// </value>
            public int EntityTypeId { get; set; }

            /// <summary>
            /// Gets or sets the entity identifier to be processed.
            /// </summary>
            /// <value>
            /// The entity identifier to be processed.
            /// </value>
            public int EntityId { get; set; }
        }
    }
}
