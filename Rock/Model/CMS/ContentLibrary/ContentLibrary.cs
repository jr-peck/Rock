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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;
using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// Represents a Type of <see cref="Rock.Model.ContentLibrary"/>.
    /// </summary>
    [RockDomain( "CMS" )]
    [Table( "ContentLibrary" )]
    [DataContract]
    [Rock.SystemGuid.EntityTypeGuid( Rock.SystemGuid.EntityType.CONTENT_LIBRARY )]
    public partial class ContentLibrary : Model<ContentLibrary>
    {
        #region Entity Properties

        /// <summary>
        /// Gets or sets the name of the ContentLibrary. This property is required.
        /// </summary>
        /// <value>
        /// A <see cref="System.String" /> representing the name of the ContentLibrary.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the library key.
        /// </summary>
        /// <value>
        /// The library key.
        /// </value>
        [MaxLength( 100 )]
        [DataMember]
        public string LibraryKey { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [trending enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [trending enabled]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool TrendingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the trending window day. This property is required.
        /// </summary>
        /// <value>
        /// The trending window day.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int TrendingWindowDay { get; set; }

        /// <summary>
        /// Gets or sets the trending max items. This property is required.
        /// </summary>
        /// <value>
        /// The trending max items.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public int TrendingMaxItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether segments should be enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable segments]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool EnableSegments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether request filters should be enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable request filters]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool EnableRequestFilters { get; set; }

        /// <summary>
        /// Gets or sets the filter settings.
        /// </summary>
        /// <value>
        /// The filter settings.
        /// </value>
        [DataMember]
        public string FilterSettings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether full text search should be enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable full text search]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool FullTextSearchEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether year search should be enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [enable year search]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool YearSearchEnabled { get; set; }

        /// <summary>
        /// Gets or sets the year search filter control.
        /// </summary>
        /// <value>
        /// The year search filter control.
        /// </value>
        /// </value>
        [DataMember]
        public YearSearchFilterControl YearSearchFilterControl { get; set; }

        /// <summary>
        /// Gets or sets the last index date time. This property is required.
        /// </summary>
        /// <value>
        /// The last index date time.
        /// </value>
        [Required]
        [DataMember( IsRequired = true )]
        public DateTime LastIndexDateTime { get; set; }

        /// <summary>
        /// Gets or sets the last index item count.
        /// </summary>
        /// <value>
        /// The last index index item count.
        /// </value>
        [DataMember]
        public int? LastIndexItemCount { get; set; }

        #endregion Entity Properties

        #region Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        #endregion Methods
    }

    #region Entity Configuration

    /// <summary>
    /// Content Library Configuration class.
    /// </summary>
    public partial class ContentLibraryConfiguration : EntityTypeConfiguration<ContentLibrary>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentLibraryConfiguration"/> class.
        /// </summary>
        public ContentLibraryConfiguration()
        {
        }
    }

    #endregion Entity Configuration
}
