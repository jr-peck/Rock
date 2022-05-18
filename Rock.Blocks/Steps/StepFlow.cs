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
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;
using Rock.Attribute;
using Rock.Model;
using Rock.Data;
using Rock.ViewModels.Utility;
using Rock.Web.Cache;

namespace Rock.Blocks.Steps
{
    /// <summary>
    /// An example block.
    /// </summary>
    /// <seealso cref="Rock.Blocks.RockObsidianBlockType" />

    [DisplayName( "Step Flow" )]
    [Category( "Obsidian > Steps" )]
    [Description( "Show the flow of individuals as they move through the four step types in the Discipleship Path program." )]
    [IconCssClass( "fa fa-users" )]

    public class StepFlow : RockObsidianBlockType
    {
        #region Attribute Keys

        private static class AttributeKey
        {
            public const string ShowEmailAddress = "ShowEmailAddress";
            public const string Email = "Email";
        }

        #endregion Attribute Keys

        #region PageParameterKeys

        private static class PageParameterKey
        {
            public const string StepProgramId = "ProgramId";
        }

        #endregion PageParameterKeys

        public List<ListItemBag> Campuses { get; set; }
        private int currentColorIndex = 0;
        private string[] defaultColors = { "#ea5545", "#f46a9b", "#ef9b20", "#edbf33", "#ede15b", "#bdcf32", "#87bc45", "#27aeef", "#b33dc6" };



        #region Base Overrides

        /// <summary>
        /// Gets the property values that will be sent to the browser and available to the client side code as it initializes.
        /// </summary>
        /// <returns>
        /// A collection of string/object pairs.
        /// </returns>
        public override object GetObsidianBlockInitialization()
        {
            var rockContext = new RockContext();
            var campusClientService = new Rock.ClientService.Core.Campus.CampusClientService( rockContext, RequestContext.CurrentPerson );
            Campuses = campusClientService.GetCampusesAsListItems();

            return new
            {
                Campuses = Campuses
            };
        }

        #endregion Base Overloads

        #region Block Actions

        /// <summary>
        /// Gets the data for the diagram
        /// </summary>
        /// <param name="startDate">The parameter from client.</param>
        /// <param name="endDate">The parameter from client.</param>
        /// <param name="maxLevels">The parameter from client.</param>
        /// <param name="campus">The parameter from client.</param>
        /// <returns></returns>
        [BlockAction]
        public BlockActionResult GetData( string startDate, string endDate, int maxLevels, string campus )
        {
            currentColorIndex = 0;
            List<StepTypeCache> stepTypes = StepProgramCache.Get( PageParameter( PageParameterKey.StepProgramId ).AsInteger() ).StepTypes;
            var nodeResults = new List<object>();
            int order = 0;

            foreach ( StepTypeCache step in stepTypes )
            {
                nodeResults.Add( new
                {
                    Id = step.Id,
                    Order = ++order,
                    Name = step.Name,
                    Color = step.HighlightColor ?? getNextDefaultColor()
                } );
            }

            var parameters = GetParameters( maxLevels, startDate, endDate, campus );
            var flowEdgeData = new DbService( new RockContext() ).GetDataTableFromSqlCommand( "spSteps_StepFlow", System.Data.CommandType.StoredProcedure, parameters );
            var flowEdgeResults = new List<object>();

            foreach (DataRow flowEdgeRow in flowEdgeData.Rows)
            {
                int level = flowEdgeRow["Level"].ToIntSafe();
                int units = flowEdgeRow["StepCount"].ToIntSafe();
                int sourceId = flowEdgeRow["SourceStepTypeId"].ToIntSafe();
                int targetId = flowEdgeRow["TargetStepTypeId"].ToIntSafe();

                var source = stepTypes.Find( stepType => stepType.Id == sourceId );
                var target = stepTypes.Find( stepType => stepType.Id == targetId );
                
                flowEdgeResults.Add( new
                {
                    Level = level,
                    SourceId = sourceId,
                    TargetId = targetId,
                    Units = units,
                    Tooltip = level > 1 ? buildTooltip(source, target, units, flowEdgeRow["AvgNumberOfDaysBetweenSteps"].ToIntSafe() ) : ""
                } );
            }

            return ActionOk( new
            {
                Parameters = parameters,
                Edges = flowEdgeResults,
                Nodes = nodeResults
            } );
        }

        #endregion Block Actions

        /// <summary>
        /// Get the parameters dictionary for sending in to the DB query
        /// </summary>
        /// <param name="MaxLevels">Depth Number of steps</param>
        /// <param name="DateRangeStartDate"></param>
        /// <param name="DateRangeEndDate"></param>
        /// <param name="Campus">The campus where steps take place</param>
        /// <returns></returns>
        private Dictionary<string, object> GetParameters( int MaxLevels, string DateRangeStartDate, string DateRangeEndDate, string Campus )
        {
            var parameters = new Dictionary<string, object>();

            if (MaxLevels > 0)
            {
                parameters.Add( "MaxLevels", MaxLevels );
            }

            if ( DateRangeStartDate != null )
            {
                parameters.Add( "DateRangeStartDate", DateTime.ParseExact( DateRangeStartDate, "yyyy-MM-dd", new CultureInfo( "en-US" ) ) );
            }

            if ( DateRangeEndDate != null )
            {
                parameters.Add( "DateRangeEndDate", DateTime.ParseExact( DateRangeEndDate, "yyyy-MM-dd", new CultureInfo( "en-US" ) ) );
            }

            if ( Campus != null && new Guid(Campus) != Guid.Empty )
            {
                parameters.Add( "CampusId", CampusCache.GetId( new Guid( Campus ) ) );
            }
            else
            {
                parameters.Add( "CampusId", DBNull.Value );
            }

            parameters.Add( "StepProgramId", PageParameter( PageParameterKey.StepProgramId ).AsInteger() );
            parameters.Add( "DataViewId", DBNull.Value );

            return parameters;
        }

        private string getNextDefaultColor()
        {
            if (currentColorIndex >= defaultColors.Length)
            {
                currentColorIndex = 0;
            }

            return defaultColors[currentColorIndex++];
        }

        private string buildTooltip(StepTypeCache source, StepTypeCache target, int units, Nullable<int> days )
        {
            string dayString = days == null ? "Unknown" : $"{ days }";

            return $"<p><strong>{source.Name} > {target.Name}</strong></p>" +
                $"Steps Taken: {units}<br/>" +
                $"Avg Days Between Steps: {dayString}";
        }
    }
}
