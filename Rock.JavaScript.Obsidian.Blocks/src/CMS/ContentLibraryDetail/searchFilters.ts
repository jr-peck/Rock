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

import { computed, defineComponent, PropType, ref } from "vue";
import Panel from "@Obsidian/Controls/panel";
import { ContentLibraryBag } from "@Obsidian/ViewModels/Blocks/CMS/ContentLibraryDetail/contentLibraryBag";

export default defineComponent({
    name: "CMS.ContentLibraryDetail.SearchFilters",

    components: {
        Panel
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

        // #endregion

        // #region Functions

        // #endregion

        // #region Event Handlers

        // #endregion

        return {
        };
    },

    template: `
<Panel title="Search Filters">
</Panel>
`
});
