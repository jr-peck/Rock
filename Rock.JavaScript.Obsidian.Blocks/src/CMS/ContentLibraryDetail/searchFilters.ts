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

import { computed, defineComponent, PropType } from "vue";
import Panel from "@Obsidian/Controls/panel";
import RockButton from "@Obsidian/Controls/rockButton";
import SectionHeader from "@Obsidian/Controls/sectionHeader";
import { ContentLibraryBag } from "@Obsidian/ViewModels/Blocks/CMS/ContentLibraryDetail/contentLibraryBag";
import { ListItemBag } from "@Obsidian/ViewModels/Utility/listItemBag";
import { ContentLibraryFilterControl } from "@Obsidian/Enums/CMS/contentLibraryFilterControl";
import { AttributeFilterBag } from "@Obsidian/ViewModels/Blocks/CMS/ContentLibraryDetail/attributeFilterBag";
import SearchFilter from "./searchFilter";
import AttributeSearchFilter from "./attributeSearchFilter";

export default defineComponent({
    name: "CMS.ContentLibraryDetail.SearchFilters",

    components: {
        AttributeSearchFilter,
        Panel,
        RockButton,
        SearchFilter,
        SectionHeader
    },

    props: {
        /** The content library that contains the search filters to display and edit. */
        modelValue: {
            type: Object as PropType<ContentLibraryBag>,
            required: true
        }
    },

    setup(props) {
        // #region Values

        // #endregion

        // #region Computed Values

        const fullTextSearchEnabled = computed((): boolean => {
            return props.modelValue.filterSettings?.fullTextSearchEnabled ?? false;
        });

        const yearSearchEnabled = computed((): boolean => {
            return props.modelValue.filterSettings?.yearSearchEnabled ?? false;
        });

        const yearSearchLabel = computed((): string => {
            return props.modelValue.filterSettings?.yearSearchLabel || "Year";
        });

        const yearSearchFilterControl = computed((): ContentLibraryFilterControl => {
            return props.modelValue.filterSettings?.yearSearchFilterControl ?? ContentLibraryFilterControl.Pills;
        });

        const yearSearchFilterIsMultipleSelection = computed((): boolean => {
            return props.modelValue.filterSettings?.yearSearchFilterIsMultipleSelection ?? false;
        });

        const yearSearchValues = computed((): ListItemBag[] => {
            return [
                {
                    text: "Filter Label",
                    value: yearSearchLabel.value
                },
                {
                    text: "Filter Control",
                    value: yearSearchFilterControl.value === ContentLibraryFilterControl.Dropdown ? "Dropdown" : "Pills"
                },
                {
                    text: "Filter Mode",
                    value: yearSearchFilterIsMultipleSelection.value ? "Multi-Select" : "Single-Select"
                }
            ];
        });

        const attributeFilters = computed((): AttributeFilterBag[] => {
            return props.modelValue.filterSettings?.attributeFilters ?? [];
        });

        // #endregion

        // #region Functions

        // #endregion

        // #region Event Handlers

        // #endregion

        return {
            attributeFilters,
            fullTextSearchEnabled,
            yearSearchEnabled,
            yearSearchValues
        };
    },

    template: `
<Panel title="Search Filters">
    <SectionHeader title="Search Filters"
        description="The configuration below allows you to set various ways your library can be filtered." />

    <SearchFilter :isEnabled="isFullTextSearchEnabled"
        title="Full Text Search"
        description="Uses the content field of the content channel item or description of an Event Item." />

    <SearchFilter :isEnabled="isYearSearchEnabled"
        title="Year"
        description="Uses the content channel item's start date to determine the year of the content."
        :values="yearSearchValues" />
    
    <SectionHeader title="Attribute Filters"
        description="The settings below allow you to provide filters for attributes that you have configured to add to your content library."
        class="margin-t-lg" />

    <AttributeSearchFilter v-for="attribute in attributeFilters" :modelValue="attribute" />
</Panel>
`
});
